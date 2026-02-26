<script setup lang="ts">
import { onMounted } from 'vue'
import { useCourseStore } from '@/stores/CoursesStore'
import CourseFilters from '@/components/publicLayout/home/CoursesFilter.vue'
import ListCoursesComponent from '@/components/publicLayout/home/ListCoursesComponent.vue'

const courseStore = useCourseStore()


onMounted(() => {
    courseStore.getCourses()
})

const onFilter = (params: any) => {
  courseStore.getCourses(params)
}

</script>

<template>
    <div class="container py-4 mb-5"> 
    <CourseFilters @filter="onFilter" />
    <ListCoursesComponent :courses="courseStore.courses" />
    </div>
</template>