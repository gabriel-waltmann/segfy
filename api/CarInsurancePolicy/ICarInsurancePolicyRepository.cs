namespace api.CarInsurancePolicy;

public interface ICarInsurancePolicyRepository
{
    IEnumerable<CarInsurancePolicyEntity> List();
    IEnumerable<CarInsurancePolicyEntity> List(int days);
    CarInsurancePolicyEntity? GetByNumber(string number);
    CarInsurancePolicyEntity Create(CarInsurancePolicyEntity entity);
    CarInsurancePolicyEntity? Update(CarInsurancePolicyEntity entity);
    bool Delete(string number);
}
