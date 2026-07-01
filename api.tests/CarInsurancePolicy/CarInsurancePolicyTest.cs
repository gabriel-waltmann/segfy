using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using FluentAssertions;
using api.CarInsurancePolicy;

namespace api.tests.CarInsurancePolicy;

public class CarInsurancePolicyTest
{
    private readonly Mock<ICarInsurancePolicyRepository> _repositoryMock;
    private readonly CarInsurancePolicyService _service;

    public CarInsurancePolicyTest()
    {
        _repositoryMock = new Mock<ICarInsurancePolicyRepository>();
        _service = new CarInsurancePolicyService(_repositoryMock.Object);
    }

    [Fact]
    public void ShouldCreateCarInsurancePolicyWithCorrectNumberPatternAndSave()
    {
        // Arrange
        var anoAtual = DateTime.Now.Year.ToString();
        var apoliceRequest = new CarInsurancePolicyEntity
        {
            CpfCnpj = "12345678909",
            VehiclePlate = "ABC-1234",
            Price = 150.50m,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            Status = CarInsurancePolicyStatus.Ativa
        };

        // Simulamos que, ao salvar, o banco retorna a própria apólice
        _repositoryMock.Setup(r => r.Create(It.IsAny<CarInsurancePolicyEntity>()))
                       .Returns((CarInsurancePolicyEntity a) => a);

        // Act
        // ISSO VAI FALHAR porque o serviço lança NotImplementedException
        var resultado = _service.Create(apoliceRequest);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Number.Should().NotBeNullOrEmpty();
        
        // Verifica o padrão: SEG-YYYY-XXXX
        resultado.Number.Should().StartWith($"SEG-{anoAtual}-");
        resultado.Number.Length.Should().Be(13); // SEG (3) + - (1) + YYYY (4) + - (1) + XXXX (4) = 13
        
        // Verifica se o repositório foi chamado exatamente 1 vez
        _repositoryMock.Verify(r => r.Create(It.IsAny<CarInsurancePolicyEntity>()), Times.Once);
    }

    [Fact]
    public void ShouldListCarInsurancePolicyNextDueDate()
    {
        // Arrange
        var apolicesMockadas = new List<CarInsurancePolicyEntity>
        {
            new() { Number = "SEG-2023-0001", EndDate = DateTime.Now.AddDays(15), StartDate = DateTime.Now, CpfCnpj = "00099988877", Price = 123, Status = CarInsurancePolicyStatus.Ativa, VehiclePlate = "OTM2022" },
            new() { Number = "SEG-2023-0002", EndDate = DateTime.Now.AddDays(29), StartDate = DateTime.Now, CpfCnpj = "00099988877", Price = 123, Status = CarInsurancePolicyStatus.Ativa, VehiclePlate = "OTM2022" }
        };

        // Configuramos o mock para retornar a lista quando pedirem apólices vencendo em 30 dias
        _repositoryMock.Setup(r => r.List(30))
                       .Returns(apolicesMockadas);

        // Act
        // ISSO VAI FALHAR porque o método não está implementado
        var resultado = _service.List(30);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado.Select(a => a.Number).Should().Contain("SEG-2023-0001");
        
        // Verifica se o serviço de fato consultou passando '30' como parâmetro
        _repositoryMock.Verify(r => r.List(30), Times.Once);
    }
}