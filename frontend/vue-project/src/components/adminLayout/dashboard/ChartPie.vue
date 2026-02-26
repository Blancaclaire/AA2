<script setup lang="ts">
import { computed } from 'vue'
import { Doughnut } from 'vue-chartjs'
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js'

ChartJS.register(ArcElement, Tooltip, Legend)

const props = defineProps<{
  title: string
  labels: string[]
  data: number[]
}>()

const chartData = computed(() => ({
  labels: props.labels,
  datasets: [
    {
      data: props.data,
      backgroundColor: [
        '#6366f1',
        '#8b5cf6',
        '#ec4899',
        '#f59e0b',
        '#10b981',
        '#3b82f6'
      ],
      borderWidth: 0
    }
  ]
}))

const chartOptions = computed(() => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'bottom' as const
    },
    title: {
      display: true,
      text: props.title,
      font: {
        size: 16,
        weight: 'bold' as const
      }
    }
  }
}))
</script>

<template>
  <b-card class="chart-card shadow-sm">
    <div style="height: 300px;">
      <Doughnut :data="chartData" :options="chartOptions" />
    </div>
  </b-card>
</template>

<style scoped>
.chart-card {
  background-color: var(--bs-tertiary-bg) !important;
}
</style>