<script setup lang="ts">
import MyCourseList from '@/components/publicLayout/myCourses/MyCoursesList.vue';
import { useUserStore } from '@/stores/UserStore';
import { onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute()
const userStore = useUserStore()

const loadCourses = () => {
  userStore.getUserCourses()
}

onMounted(loadCourses)
watch(() => route.fullPath, loadCourses)

</script>

<template>

    <MyCoursesList :courses="userStore.courses"></MyCoursesList>


</template>

