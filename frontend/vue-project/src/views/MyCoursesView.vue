<script setup lang="ts">
import MyCourseList from '@/components/publicLayout/myCourses/MyCoursesList.vue';
import { useUserStore } from '@/stores/UserStore';
import { onMounted, reactive } from 'vue';

const userStore = useUserStore();

onMounted(()=> {
    userStore.getUserCourses()
})

</script>

<template>

    <MyCoursesList :courses="userStore.courses"></MyCoursesList>


</template>

