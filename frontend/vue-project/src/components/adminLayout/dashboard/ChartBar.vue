<script setup lang="ts">
import { computed } from 'vue'
import { Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
} from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

const props = defineProps<{
  title: string
  labels: string[]
  data: number[]
  backgroundColor?: string
}>()

const chartData = computed(() => ({
  labels: props.labels,
  datasets: [
    {
      label: props.title,
      backgroundColor: props.backgroundColor || '#6366f1',
      data: props.data,
      borderRadius: 8
    }
  ]
}))


const chartOptions = computed(() => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false
    },
    title: {
      display: true,
      text: props.title,
      font: {
        size: 16,
        weight: 'bold' as const
      }
    }
  },
  scales: {
    y: {
      beginAtZero: true
    }
  }
}))
</script>

<template>
  <b-card class="chart-card shadow-sm">
    <div style="height: 300px;">
      <Bar :data="chartData" :options="chartOptions" />
    </div>
  </b-card>
</template>

<style scoped>
.chart-card {
  background-color: var(--bs-tertiary-bg) !important;
}
</style>