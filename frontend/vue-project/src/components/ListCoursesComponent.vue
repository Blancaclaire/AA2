<script setup lang="ts">
import CourseCard from '@/components/CourseCard.vue'

defineProps<{ courses: any[] }>()
</script>

<template>
  <b-row>
    <b-col v-for="course in courses" :key="course.id" md="4">
      <CourseCard :course="course" />
    </b-col>
  </b-row>
</template>