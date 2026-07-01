namespace api.CarInsurancePolicy;

public class CarInsurancePolicyEntity
{
    public string? Number { get; set; } // pattern: SEG-YYYY-XXXX
    public required string CpfCnpj { get; set; }
    public required string VehiclePlate { get; set; }
    public required decimal Price { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required CarInsurancePolicyStatus Status { get; set; }
}