<script setup lang="ts">
import CourseCard from '@/components/publicLayout/home/CourseCard.vue'

defineProps<{ courses: any[] }>()
</script>

<template>
  <b-row class="g-4">
    <b-col v-for="course in courses" :key="course.id" md="6" lg="4">
      <CourseCard :course="course" />
    </b-col>
  </b-row> 
</template>