<template>
  <div class="flex flex-col mb-4">
    <label :for="id" class="mb-1 text-sm font-medium text-gray-700">
      {{ label }}
    </label>
    <input
      :id="id"
      :type="type"
      :value="modelValue"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
      :step="step"
      :min="min"
      :max="max"
      class="px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-2 focus:border-transparent transition-colors"
      :class="error ? 'border-red-500 focus:ring-red-500' : 'border-gray-300 focus:ring-emerald-500'"
    />
    <span v-if="error" class="mt-1 text-xs text-red-500 font-medium">
      {{ error }}
    </span>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  id: string;
  label: string;
  modelValue: string | number;
  type?: string;
  error?: string;
  step?: string;
  min?: string | number;
  max?: string | number;
}>();

defineEmits<{
  (e: 'update:modelValue', value: string | number): void;
}>();
</script>