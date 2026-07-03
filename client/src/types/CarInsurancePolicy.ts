export interface CarInsurancePolicy {
  number?: string | null;
  cpfCnpj: string;
  vehiclePlate: string;
  price: number;
  startDate: string;
  endDate: string;
  status: 0 | 1 | 2; // Exact values from your CarInsurancePolicyStatus schema
}