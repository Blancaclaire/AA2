<script setup lang="ts">
import CourseForm from '@/components/adminLayout/courses/CourseForm.vue';
import { useCourseStore } from '@/stores/CoursesStore';
import { useAuthStore } from '@/stores/AuthStore'
import CourseFilters from '@/components/publicLayout/home/CoursesFilter.vue'
import ListCoursesComponent from '@/components/publicLayout/home/ListCoursesComponent.vue';
import { onMounted } from 'vue';

const courseStore = useCourseStore()

onMounted(() => {
    courseStore.getCourses()
})
const authStore = useAuthStore()

const onFilter = (params: any) => {
  courseStore.getCourses(params)
}

</script>

<template>
  <h1>Gestión de Cursos</h1>

  <CourseForm></CourseForm>
  <h2>Cursos disponibles</h2>
  <CourseFilters @filter="onFilter" />
  <ListCoursesComponent :courses="courseStore.courses"></ListCoursesComponent>
</template>