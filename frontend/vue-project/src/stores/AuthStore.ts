import { defineStore } from 'pinia'
import api from '@/api/axios'

export const useAuthStore = defineStore('AuthStore', {
  state: () => ({
    loading: false,
    error: null as string | null,
    token: localStorage.getItem('token') || null,
    role: localStorage.getItem('role') || null,
    userName: localStorage.getItem('userName') || null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    
    async login(credentials: any) {
      this.loading = true
      this.error = null
      try {
        const res = await api.post('/auth/login', credentials)
        this.token = res.data.token
        this.role = res.data.role
        this.userName = res.data.name
        localStorage.setItem('token', res.data.token)
        localStorage.setItem('role', res.data.role)
        localStorage.setItem('userName', res.data.name)
        console.log('hecho login')
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error de conexión'
      } finally {
        this.loading = false
      }
    },
    async register(credentials: any) {
      this.loading = true
      this.error = null
      try {
        const res = await api.post('/auth/register', credentials)
        this.token = res.data.token
        this.role = res.data.role
        this.userName = res.data.name
        localStorage.setItem('token', res.data.token)
        localStorage.setItem('role', res.data.role)
        localStorage.setItem('userName', res.data.name)
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error de conexión'
      } finally {
        this.loading = false
      }
    },
    logout() {
      this.token = null
      this.role = null
      this.userName = null
      localStorage.removeItem('token')
      localStorage.removeItem('role')
      localStorage.removeItem('userName')},


  }
})