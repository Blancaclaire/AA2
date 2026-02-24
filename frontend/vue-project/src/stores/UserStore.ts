import { defineStore } from 'pinia'
import api from '@/api/axios'

export const useUserStore = defineStore('UserStore', {
    state: () => ({
        users: [] as any[],
        user: null as string | null,
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
                this.error = err.response?.data?.message || 'Error al cargar los cursos del usuario'
            } finally {
                this.loading = false
            }
        },
        async postUser(credentials: any){
            this.loading = true
            try{
                const res = await api.post('/users', credentials)
                this.user = res.data
                return true
            }catch (err: any) {
                this.error = err.response?.data?.message || 'Error al registrar nuevo usuario'
                return false
            } finally {
                this.loading = false
            }
        },

        },
    }
)