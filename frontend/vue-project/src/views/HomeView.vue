<script setup lang="ts">
import { onMounted } from 'vue'
import { useCourseStore } from '@/stores/CoursesStore'
import ListCoursesComponent from '@/components/ListCoursesComponent.vue'

const courseStore = useCourseStore()

onMounted(() => {
  courseStore.getCourses()
})

</script>

<template>
  <h1>Cursos disponibles</h1>
  <ListCoursesComponent :courses="courseStore.courses" />
</template>