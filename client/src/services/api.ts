// src/services/api.ts
import type { CarInsurancePolicy } from '@/types/CarInsurancePolicy';
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:8080', 
  headers: {
    'Content-Type': 'application/json',
  },
});

export default {
  getPolicies() {
    return apiClient.get<CarInsurancePolicy[]>('/api/car-insurance-policies');
  },
  getExpiringPolicies(days: number = 30) {
    return apiClient.get<CarInsurancePolicy[]>(`/api/car-insurance-policies/expiring?days=${days}`);
  },
  createPolicy(policyData: CarInsurancePolicy) {
    return apiClient.post<CarInsurancePolicy>('/api/car-insurance-policies', policyData);
  },
  deletePolicy(number: string) {
    return apiClient.delete(`/api/car-insurance-policies/${number}`);
  }
};