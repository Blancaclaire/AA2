<script setup lang="ts">
import CourseForm from '@/components/adminLayout/courses/CourseForm.vue';
import { useCourseStore } from '@/stores/CoursesStore';
import { useAuthStore } from '@/stores/AuthStore'


const authStore = useAuthStore()

</script>

<template>
  <h1>Gestión de Cursos</h1>

  <CourseForm></CourseForm>
</template>