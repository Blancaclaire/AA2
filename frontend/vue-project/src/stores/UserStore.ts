import { defineStore } from 'pinia'
import api from '@/api/axios'

export const useUserStore = defineStore('UserStore', {
    state: () => ({
        users: [] as any[],
        courses: [] as any[],
        loading: false,
        error: null as string | null,
        userId: localStorage.getItem('userId') || null,

    }),

    actions: {
        async getUserCourses() {
            this.loading = true
            try {
                const res = await api.get('/users/me/enrollments')
                this.courses = res.data
            } catch (err: any) {
                this.error = err.response?.data?.message || 'Error al cargar categorías'
            } finally {
                this.loading = false
            }
        }
    }
})