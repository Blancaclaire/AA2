<script setup lang="ts">
defineProps<{
  title: string
  value: string | number
  icon?: string
  trend?: number 
  color?: string
}>()
</script>

<template>
  <b-card class="kpi-card h-100 shadow-sm">
    <div class="d-flex justify-content-between align-items-start">
      <div>
        <p class="text-muted mb-1 small">{{ title }}</p>
        <h3 class="mb-0 fw-bold" :class="`text-${color || 'primary'}`">
          {{ value }}
        </h3>
        
        <div v-if="trend !== undefined" class="mt-2">
          <span :class="trend >= 0 ? 'text-success' : 'text-danger'" class="small">
            <span v-if="trend >= 0">▲</span>
            <span v-else>▼</span>
            {{ Math.abs(trend) }}%
          </span>
          <span class="text-muted small ms-1">vs mes anterior</span>
        </div>
      </div>
      
    </div>
  </b-card>
</template>

<style scoped>
.kpi-card {
  border: none;
  border-left: 4px solid var(--bs-primary);
  transition: transform 0.2s;
  background-color: var(--bs-tertiary-bg) !important;
}

.kpi-card:hover {
  transform: translateY(-4px);
}

.kpi-icon {
  width: 50px;
  height: 50px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
}
</style>