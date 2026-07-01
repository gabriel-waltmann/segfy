using System;
using System.Collections.Generic;

namespace api.CarInsurancePolicy;

public class CarInsurancePolicyService(ICarInsurancePolicyRepository repository) {
  private readonly ICarInsurancePolicyRepository _repository = repository;

  public IEnumerable<CarInsurancePolicyEntity> List(int days)
  {
    throw new NotImplementedException("to iimplement");
  }

  public CarInsurancePolicyEntity Get(CarInsurancePolicyEntity novaApolice)
  {
    throw new NotImplementedException("to iimplement");
  }

  public CarInsurancePolicyEntity Create(CarInsurancePolicyEntity novaApolice)
  {
    throw new NotImplementedException("to iimplement");
  }

  public CarInsurancePolicyEntity Update(CarInsurancePolicyEntity novaApolice)
  {
    throw new NotImplementedException("to iimplement");
  }
    
  public void Update(string number)
  {
    throw new NotImplementedException("to iimplement");
  }
}