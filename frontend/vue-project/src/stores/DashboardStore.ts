import { defineStore } from 'pinia'
import api from '@/api/axios'


interface DashboardKpis {
  totalCourses: number
  publishedCourses: number
  totalCategories: number
  totalUsers: number
  totalEnrollments: number
  totalRevenue: number
  averageRating: number
  averageDurationHours: number
}

interface ChartPoint {
  label: string
  value: number
}

interface DashboardResponse {
  kpis: DashboardKpis
  enrollmentsByCategory: ChartPoint[]
  coursesByLevel: ChartPoint[]
  revenueByMonth: ChartPoint[]
  topCoursesByRating: ChartPoint[]
}

export const useDashboardStore = defineStore('DashboardStore', {
  state: () => ({
    loading: false,
    error: null as string | null,
    
    
    kpis: {
      totalCourses: 0,
      publishedCourses: 0,
      totalCategories: 0,
      totalUsers: 0,
      totalEnrollments: 0,
      totalRevenue: 0,
      averageRating: 0,
      averageDurationHours: 0
    } as DashboardKpis,
    
    
    enrollmentsByCategory: [] as ChartPoint[],
    coursesByLevel: [] as ChartPoint[],
    revenueByMonth: [] as ChartPoint[],
    topCoursesByRating: [] as ChartPoint[]
  }),

  getters: {
    // Formatear moneda en euros
    formattedRevenue: (state) => {
      return new Intl.NumberFormat('es-ES', {
        style: 'currency',
        currency: 'EUR'
      }).format(state.kpis.totalRevenue)
    },
    
    // Formatear rating con decimales
    formattedRating: (state) => {
      return state.kpis.averageRating.toFixed(1)
    },
    
    // Formatear duración promedio
    formattedAvgDuration: (state) => {
      return `${state.kpis.averageDurationHours.toFixed(1)}h`
    },
    
    // Matriculaciones por Categoría
    categoryChartData: (state) => ({
      labels: state.enrollmentsByCategory.map(item => item.label),
      data: state.enrollmentsByCategory.map(item => item.value)
    }),
    
    // Cursos por Nivel
    levelChartData: (state) => ({
      labels: state.coursesByLevel.map(item => item.label),
      data: state.coursesByLevel.map(item => item.value)
    }),
    
    // Datos para top cursos
    topCoursesChartData: (state) => ({
      labels: state.topCoursesByRating.map(item => item.label),
      data: state.topCoursesByRating.map(item => item.value)
    })
  },

  actions: {
   
    async fetchDashboardData() {
      this.loading = true
      this.error = null
      
      try {
        const response = await api.get<DashboardResponse>('/dashboard')
        
        this.kpis = response.data.kpis
        this.enrollmentsByCategory = response.data.enrollmentsByCategory
        this.coursesByLevel = response.data.coursesByLevel
        this.revenueByMonth = response.data.revenueByMonth
        this.topCoursesByRating = response.data.topCoursesByRating
        
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al cargar el dashboard'
        console.error('Error fetching dashboard:', err)
      } finally {
        this.loading = false
      }
    }
  }
})