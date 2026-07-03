<template>
  <div class="max-w-3xl mx-auto p-6 font-sans text-gray-800">
    <h1 class="text-3xl font-bold mb-8 text-center text-gray-900">Car Insurance Policies</h1>

    <div class="bg-white p-6 rounded-lg shadow-md mb-8 border border-gray-100">
      <h2 class="text-xl font-semibold mb-6 border-b pb-2">Create New Policy</h2>
      <form @submit.prevent="submitPolicy">
        
        <div class="flex flex-col">
          <BaseInput
            id="cpfCnpj"
            label="CPF/CNPJ"
            v-model="newPolicy.cpfCnpj"
            :error="errors.cpfCnpj"
            type="text"
          />
          <BaseInput
            id="vehiclePlate"
            label="Placa Do Veiculo"
            v-model="newPolicy.vehiclePlate"
            :error="errors.vehiclePlate"
            type="text"
          />
          <BaseInput
            id="price"
            label="Valor"
            v-model.number="newPolicy.price"
            :error="errors.price"
            type="number"
            step="0.01"
          />
          <BaseSelect
            id="status"
            label="Status da Apólice"
            v-model="newPolicy.status"
            :options="statusOptions"
            :error="errors.status"
          />
          <BaseInput
            id="startDate"
            label="Start Date"
            v-model="newPolicy.startDate"
            :error="errors.startDate"
            type="date"
          />
          <BaseInput
            id="endDate"
            label="End Date"
            v-model="newPolicy.endDate"
            :error="errors.endDate"
            type="date"
          />
        </div>

        <button 
          type="submit"
          class="w-full mt-4 px-4 py-2 bg-emerald-500 hover:bg-emerald-600 text-white font-semibold rounded-md shadow transition-colors"
        >
          Save Policy
        </button>
      </form>
    </div>

    <div class="bg-white p-6 rounded-lg shadow-md border border-gray-100">
      <h2 class="text-xl font-semibold mb-4 border-b pb-2">Current Policies</h2>
      <ul v-if="policies.length > 0" class="divide-y divide-gray-200">
        <li v-for="policy in policies" :key="policy.number ?? undefined" class="py-4 flex justify-between items-center">
          <div>
            <p class="font-medium text-gray-900">Plate: <span class="uppercase">{{ policy.vehiclePlate }}</span></p>
            <p class="text-sm text-gray-600">Price: ${{ policy.price.toFixed(2) }} | Status: <strong>{{ getStatusLabel(policy.status) }}</strong></p>
          </div>

          <button 
            @click="removePolicy(policy.number)" 
            class="px-3 py-1 bg-red-100 text-red-600 hover:bg-red-200 hover:text-red-700 rounded-md font-medium transition-colors"
          >
            Delete
          </button>
        </li>
      </ul>
      <p v-else class="text-gray-500 text-center py-4">No policies found.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import api from './services/api';
import type { CarInsurancePolicy } from './types/CarInsurancePolicy';
import BaseInput from './components/BaseInput.vue';
import BaseSelect from './components/BaseSelect.vue'; 

const policies = ref<CarInsurancePolicy[]>([]);
const errors = ref<Record<string, string>>({});

const newPolicy = ref<CarInsurancePolicy>({
  cpfCnpj: '',
  vehiclePlate: '',
  price: 0,
  startDate: '',
  endDate: '',
  status: 0
});

const statusOptions = [
  { value: 0, label: 'Ativa' },
  { value: 1, label: 'Cancelada' },
  { value: 2, label: 'Expirada' }
];

const getStatusLabel = (statusValue: number): string => {
  const option = statusOptions.find(opt => opt.value === statusValue);
  return option ? option.label : 'Desconhecido';
};

const validateForm = (): boolean => {
  errors.value = {};
  let isValid = true;

  // 1. Validate Plate
  const plateRegex = /^[A-Z]{3}-?\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$/i;
  if (!newPolicy.value.vehiclePlate) {
    errors.value.vehiclePlate = "Placa do veículo é obrigatória.";
    isValid = false;
  } else if (!plateRegex.test(newPolicy.value.vehiclePlate.trim())) {
    errors.value.vehiclePlate = "Placa inválida. Use ABC-1234 ou ABC1D23.";
    isValid = false;
  }

  // 2. Validate CPF/CNPJ
  const digitsOnly = newPolicy.value.cpfCnpj.replace(/\D/g, '');
  if (!digitsOnly) {
    errors.value.cpfCnpj = "CPF/CNPJ é obrigatório.";
    isValid = false;
  } else if (digitsOnly.length === 11 && !isValidCpf(digitsOnly)) {
    errors.value.cpfCnpj = "CPF inválido.";
    isValid = false;
  } else if (digitsOnly.length === 14 && !isValidCnpj(digitsOnly)) {
    errors.value.cpfCnpj = "CNPJ inválido.";
    isValid = false;
  } else if (digitsOnly.length !== 11 && digitsOnly.length !== 14) {
    errors.value.cpfCnpj = "Deve conter 11 (CPF) ou 14 (CNPJ) dígitos.";
    isValid = false;
  }

  // 3. Validate Price
  if (newPolicy.value.price <= 0) {
    errors.value.price = "Preço deve ser maior que zero.";
    isValid = false;
  }

  // 4. Validate Dates
  if (!newPolicy.value.startDate) {
    errors.value.startDate = "Data de início é obrigatória.";
    isValid = false;
  }
  if (!newPolicy.value.endDate) {
    errors.value.endDate = "Data de fim é obrigatória.";
    isValid = false;
  } else if (new Date(newPolicy.value.endDate) <= new Date(newPolicy.value.startDate)) {
    errors.value.endDate = "Data de fim deve ser posterior à de início.";
    isValid = false;
  }

  // 5. Validate Status
  if (![0, 1, 2].includes(newPolicy.value.status)) {
    errors.value.status = "Status inválido.";
    isValid = false;
  }

  return isValid;
};

// CPF Calculation (TypeScript equivalent of your C# logic)
const isValidCpf = (cpf: string) => {
  if (/^(\d)\1+$/.test(cpf)) return false;
  const numbers = cpf.split('').map(Number);
  const calcCheckDigit = (digits: number[], len: number, startWeight: number) => {
    let sum = 0;
    for (let i = 0; i < len; i++) sum += digits[i] * (startWeight - i);
    const rem = sum % 11;
    return rem < 2 ? 0 : 11 - rem;
  };
  if (calcCheckDigit(numbers, 9, 10) !== numbers[9]) return false;
  return calcCheckDigit(numbers, 10, 11) === numbers[10];
};

// CNPJ Calculation (TypeScript equivalent of your C# logic)
const isValidCnpj = (cnpj: string) => {
  if (/^(\d)\1+$/.test(cnpj)) return false;
  const numbers = cnpj.split('').map(Number);
  const calcCheckDigit = (digits: number[], weights: number[]) => {
    let sum = 0;
    for (let i = 0; i < weights.length; i++) sum += digits[i] * weights[i];
    const rem = sum % 11;
    return rem < 2 ? 0 : 11 - rem;
  };
  if (calcCheckDigit(numbers, [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]) !== numbers[12]) return false;
  return calcCheckDigit(numbers, [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]) === numbers[13];
};

// --- API Interactions ---

const loadPolicies = async () => {
  try {
    const response = await api.getPolicies();
    policies.value = response.data;
  } catch (error) {
    console.error("Failed to load policies:", error);
  }
};

const submitPolicy = async () => {
  if (!validateForm()) return; // Stop submission if frontend validation fails

  try {
    const payload: CarInsurancePolicy = {
      ...newPolicy.value,
      startDate: new Date(newPolicy.value.startDate).toISOString(),
      endDate: new Date(newPolicy.value.endDate).toISOString()
    };

    await api.createPolicy(payload);
    
    newPolicy.value = { cpfCnpj: '', vehiclePlate: '', price: 0, startDate: '', endDate: '', status: 0 };
    errors.value = {}; // Clear errors on success
    await loadPolicies();
  } catch (error: any) {
    console.error("Failed to create policy:", error);
    // If backend validation catches something the frontend missed
    if (error.response?.status === 400) {
      alert("Validation failed on the server. Check your data.");
    }
  }
};

const removePolicy = async (number: string | null | undefined) => {
  if (!number) return; 
  if (!confirm("Are you sure you want to delete this policy?")) return;
  
  try {
    await api.deletePolicy(number);
    await loadPolicies();
  } catch (error) {
    console.error("Failed to delete policy:", error);
  }
};

onMounted(() => {
  loadPolicies();
});
</script>