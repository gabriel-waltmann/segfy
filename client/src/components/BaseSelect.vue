<template>
  <div class="flex flex-col mb-4">
    <label :for="id" class="mb-1 text-sm font-medium text-gray-700">
      {{ label }}
    </label>
    <select
      :id="id"
      :value="modelValue"
      @change="$emit('update:modelValue', Number(($event.target as HTMLSelectElement).value))"
      class="px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-2 focus:border-transparent transition-colors bg-white"
      :class="error ? 'border-red-500 focus:ring-red-500' : 'border-gray-300 focus:ring-emerald-500'"
    >
      <option v-for="option in options" :key="option.value" :value="option.value">
        {{ option.label }}
      </option>
    </select>
    <span v-if="error" class="mt-1 text-xs text-red-500 font-medium">
      {{ error }}
    </span>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  id: string;
  label: string;
  modelValue: number | string;
  options: { value: number | string; label: string }[];
  error?: string;
}>();

defineEmits<{
  (e: 'update:modelValue', value: number): void;
}>();
</script>