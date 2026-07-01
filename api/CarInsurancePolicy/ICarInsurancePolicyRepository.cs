namespace api.CarInsurancePolicy;

public interface ICarInsurancePolicyRepository
{
    List<CarInsurancePolicyEntity> List(int days);
    CarInsurancePolicyEntity Get(string number);
    CarInsurancePolicyEntity Create(CarInsurancePolicyEntity dto);
    CarInsurancePolicyEntity Update(string id, CarInsurancePolicyEntity dto);
    CarInsurancePolicyEntity Delete(string id);
}