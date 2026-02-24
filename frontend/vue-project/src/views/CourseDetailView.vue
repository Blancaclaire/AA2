<script setup lang="ts">
import CourseHeroComponent from '@/components/publicLayout/detail/CourseHeroComponent.vue'
import EnrollComponent from '@/components/publicLayout/detail/EnrollComponent.vue'
import { useCourseStore } from '@/stores/CoursesStore'
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const courseStore = useCourseStore()
const courseId = Number(route.params.id)

onMounted(() => {
  courseStore.getCoursesById(courseId)
})

const levelColor = (level: string) => {
  switch (level) {
    case 'Beginner': return { bg: '#d1fae5', text: '#065f46', label: 'Principiante' }
    case 'Intermediate': return { bg: '#fef3c7', text: '#92400e', label: 'Intermedio' }
    case 'Advanced': return { bg: '#fee2e2', text: '#991b1b', label: 'Avanzado' }
    default: return { bg: '#f3f4f6', text: '#374151', label: level }
  }
}
</script>

<template>
  <!-- LOADING -->
  <div v-if="courseStore.loading" class="loading-screen">
    <div class="spinner"></div>
    <p>Cargando curso...</p>
  </div>

  <!-- DETAIL -->
  <div v-else-if="courseStore.course" class="detail-page">
    <CourseHeroComponent :course="courseStore.course"></CourseHeroComponent>
    <EnrollComponent :course-id="courseId"></EnrollComponent>
  </div>
</template>

<style scoped>
</style>