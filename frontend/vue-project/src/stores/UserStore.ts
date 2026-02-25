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
        async postUser(credentials: any) {
            this.loading = true
            try {
                const res = await api.post('/users', credentials)
                this.user = res.data
                return true
            } catch (err: any) {
                this.error = err.response?.data?.message || 'Error al registrar nuevo usuario'
                return false
            } finally {
                this.loading = false
            }
        },
        async getUsers() {
            this.loading = true
            try {
                const res = await api.get('/users')
                this.users = res.data
            } catch (err: any) {
                this.error = err.response?.data?.message || 'Error al cargar los usuarios'
            } finally {
                this.loading = false
            }
        },
        async deleteUser(id: number) {
            this.loading = true
            try {
                const res = await api.delete(`/users/${id}`)
                console.log(`'Usuario ${res.data} eliminado exitosamente`)
                return true
            } catch (err: any) {
                this.error = err.response?.data?.message || 'Error al borrar el usuarios'
                return false
            } finally {
                this.loading = false
            }
        },

        async updateUser(id: number, body : any) {
            this.loading = true
            try {
                const res = await api.patch(`/users/${id}`, body)
                console.log(`'Usuario ${res.data} actualizado`)
                return true
            } catch (err: any) {
                this.error = err.response?.data?.message || 'Error al actualizar el usuarios'
                return false
            } finally {
                this.loading = false
            }
        },

    },
}
)