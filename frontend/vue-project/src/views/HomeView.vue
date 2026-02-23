<script setup lang="ts">
import { onMounted } from 'vue'
import { useCourseStore } from '@/stores/CoursesStore'
import CourseFilters from '@/components/CoursesFilter.vue'
import ListCoursesComponent from '@/components/ListCoursesComponent.vue'

const courseStore = useCourseStore()

onMounted(() => {
    courseStore.getCourses()
})

const onFilter = (params: any) => {
  courseStore.getCourses(params)
}

</script>

<template>
    <CourseFilters @filter="onFilter" />
    <ListCoursesComponent :courses="courseStore.courses" />
</template>