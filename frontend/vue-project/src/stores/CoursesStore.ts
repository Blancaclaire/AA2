import { defineStore } from 'pinia'
import api from '@/api/axios'

export const useCourseStore = defineStore('CoursesStore', {

  state: () => ({
    loading: false,
    error: null as string | null,
    courses: [] as any[],
    course: null as any | null,
  }),

  actions: {

    async getCourses(params = {}) {
      this.loading = true
      this.error = null
      try {
        const res = await api.get('/courses', { params })
        this.courses = res.data.items

      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al cargar cursos'
      } finally {
        this.loading = false
      }
    },

    async getCoursesById(id: number) {

      this.loading = true
      this.error = null
      try {
        const res = await api.get(`/courses/${id}`)
        this.course = res.data
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al cargar cursos'
      } finally {
        this.loading = false
      }
    },

    async enrollUser(id: number) {
      this.loading = true
      this.error = null
      try {
        const res = await api.post(`/courses/${id}/enroll`)
        console.log('Matriculación exitosa', res.data)
      } catch (err: any) {
        if (err.response?.status === 409) {
          this.error = 'Ya estás matriculado en este curso'
        } else if (err.response?.status === 401) {
          this.error = 'Debes iniciar sesión para matricularte'
        } else {
          this.error = err.response?.data?.message || 'Error al matricularse'
        }
      } finally {
        this.loading = false
      }
    },

    async postCourse(body: any) {
      this.loading = true
      this.error = null
      try {
        const res = await api.post(`/courses`, body)
        this.course = res.data
        return true
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al crear curso'
        return false
      } finally {
        this.loading = false
      }
    },

    async deleteCourse(id: number) {
      this.loading = true
      try {
        const res = await api.delete(`/courses/${id}`)
        console.log(`'Curso ${res.data} eliminado exitosamente`)
        return true
      } catch (err: any) {
        this.error = err.response?.data?.message || 'Error al borrar el curso'
        return false
      } finally {
        this.loading = false
      }
    },


  }


})




