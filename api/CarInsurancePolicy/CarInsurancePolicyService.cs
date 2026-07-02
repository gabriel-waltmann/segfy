using System;
using System.Collections.Generic;

namespace api.CarInsurancePolicy;

public class CarInsurancePolicyService(ICarInsurancePolicyRepository repository)
{
  private readonly ICarInsurancePolicyRepository _repository = repository;

  public IEnumerable<CarInsurancePolicyEntity> List()
  {
    return _repository.List();
  }

  public IEnumerable<CarInsurancePolicyEntity> List(int days)
  {
    return _repository.List(days);
  }

  public CarInsurancePolicyEntity? Get(string number)
  {
    return _repository.GetByNumber(number);
  }

  public CarInsurancePolicyEntity Create(CarInsurancePolicyEntity novaApolice)
  {
    novaApolice.Number = GenerateNumber();
    return _repository.Create(novaApolice);
  }

  public CarInsurancePolicyEntity? Update(string number, CarInsurancePolicyEntity apolice)
  {
    apolice.Number = number;
    return _repository.Update(apolice);
  }

  public bool Delete(string number)
  {
    return _repository.Delete(number);
  }

  private static string GenerateNumber()
  {
    var year = DateTime.Now.Year;
    var sequence = Random.Shared.Next(1, 10000);
    return $"SEG-{year}-{sequence:D4}";
  }
}
