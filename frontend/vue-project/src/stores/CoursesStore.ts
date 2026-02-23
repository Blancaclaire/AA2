import {defineStore} from 'pinia'
import api from '@/api/axios'

export const useCourseStore = defineStore('CoursesStore',{

state:()=>({
    loading: false,
    error: null as string | null,
    courses: [] as any[],
}),

actions:{

    async getCourses(){
        this.loading = true
        this.error = null
        try{
            const res = await api.get('/courses')

            this.courses = res.data.items
        }catch (err: any) {
        this.error = err.response?.data?.message || 'Error al cargar cursos'
      } finally {
        this.loading = false
      }
    }

}

})


