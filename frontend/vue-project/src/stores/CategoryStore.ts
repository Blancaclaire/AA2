import {defineStore} from 'pinia'
import api from '@/api/axios'

export const useCategoryStore = defineStore('CategoryStore', {
  state: () => ({
    categories: [] as any[],
    loading: false,
    error: null as string | null,
  }),
  actions: {
    async getCategories() {
      this.loading = true
      try {
        const res = await api.get('/categories')
        this.categories = res.data
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al cargar categorías'
      } finally {
        this.loading = false
      }
    },
  },
})