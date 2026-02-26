<script setup lang="ts">
import CourseCard from '@/components/publicLayout/home/CourseCard.vue'

const emit = defineEmits(['delete-course','update-course'])

defineProps<{ courses: any[] }>()
</script>

<template>
  <b-row class="g-4">
    <b-col v-for="course in courses" :key="course.id" md="6" lg="4">
      <CourseCard :course="course" @delete-course="emit('delete-course', $event)"
      @update-course="emit('update-course', $event)" />
    </b-col>
  </b-row> 
</template>