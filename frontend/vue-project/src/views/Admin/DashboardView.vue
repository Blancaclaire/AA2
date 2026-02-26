<script setup lang="ts">
import { onMounted } from 'vue'
import { useDashboardStore } from '@/stores/DashboardStore'
import KpiCard from '@/components/adminLayout/dashboard/KpiCard.vue'
import ChartBar from '@/components/adminLayout/dashboard/ChartBar.vue'
import ChartPie from '@/components/adminLayout/dashboard/ChartPie.vue'

const dashboardStore = useDashboardStore()

onMounted(() => {
  dashboardStore.fetchDashboardData()
})
</script>

<template>
  <div class="container-fluid py-4">

    <h2 class="mb-4">Dashboard</h2>
    <div>
      <b-row class="g-3 mb-4">
        <b-col md="3">
          <KpiCard
            title="Total Cursos"
            :value="dashboardStore.kpis.totalCourses"
          />
        </b-col>
        
        <b-col md="3">
          <KpiCard
            title="Total Usuarios"
            :value="dashboardStore.kpis.totalUsers"
          />
        </b-col>
        
        <b-col md="3">
          <KpiCard
            title="Matriculaciones"
            :value="dashboardStore.kpis.totalEnrollments"

          />
        </b-col>
        
        <b-col md="3">
          <KpiCard
            title="Ingresos"
            :value="dashboardStore.formattedRevenue"
          />
        </b-col>
      </b-row>

      <b-row class="g-3">
        <b-col md="6">
          <ChartBar
            title="Matriculaciones por Categoría"
            :labels="dashboardStore.categoryChartData.labels"
            :data="dashboardStore.categoryChartData.data"
          />
        </b-col>

        <b-col md="6">
          <ChartPie
            title="Cursos por Nivel"
            :labels="dashboardStore.levelChartData.labels"
            :data="dashboardStore.levelChartData.data"
          />
        </b-col>

        <b-col md="12">
          <ChartBar
            title="Top Cursos por Rating"
            :labels="dashboardStore.topCoursesChartData.labels"
            :data="dashboardStore.topCoursesChartData.data"
          />
        </b-col>
      </b-row>
    </div>
  </div>
</template>