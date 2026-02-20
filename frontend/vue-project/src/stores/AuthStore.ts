import { defineStore } from 'pinia'
import api from '@/api/axios'

export const useAuthStore = defineStore('AuthStore', {
  state: () => ({
    loading: false,
    error: null as string | null,
    user: null
  }),
  actions: {
    async login(credentials: any) {
      this.loading = true
      this.error = null
      try {
        // ESTA ES LA PETICIÓN QUE TE FALTA
        const response = await api.post('/auth/login', credentials)
        console.log('Respuesta del servidor:', response.data)
        
        // Guardar el token que devuelve tu controlador de .NET
        localStorage.setItem('token', response.data.token)
        this.user = response.data.user
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error de conexión'
        console.error('Error al loguear:', err)
      } finally {
        this.loading = false
      }
    }
  }
})