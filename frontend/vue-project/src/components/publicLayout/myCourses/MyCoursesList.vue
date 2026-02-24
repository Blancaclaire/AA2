<script setup lang="ts">
import MyCourseCard from '@/components/publicLayout/myCourses/MyCourseCard.vue'
import EmptyState from '@/components/publicLayout/myCourses/EmptyState.vue'
import { computed } from 'vue'

const props = defineProps<{ courses: any[] }>()

const inProgress = computed(() => props.courses.filter(c => !c.isCompleted))
const completed = computed(() => props.courses.filter(c => c.isCompleted))
</script>

<template>
  <div>
    <!-- EN PROGRESO -->
    <div v-if="inProgress.length > 0" class="mb-5">
      <div class="section-header mb-3">
        <h5 class="section-title">
          En progreso
          <BBadge variant="primary" pill class="ms-2">{{ inProgress.length }}</BBadge>
        </h5>
      </div>
      <BRow class="g-4">
        <BCol v-for="course in inProgress" :key="course.courseId" md="6" lg="4">
          <MyCourseCard :course="course" />
        </BCol>
      </BRow>
    </div>

    <!-- COMPLETADOS -->
    <div v-if="completed.length > 0">
      <div class="section-header mb-3">
        <h5 class="section-title">
          Completados
          <BBadge variant="success" pill class="ms-2">{{ completed.length }}</BBadge>
        </h5>
      </div>
      <BRow class="g-4">
        <BCol v-for="course in completed" :key="course.courseId" md="6" lg="4">
          <MyCourseCard :course="course" />
        </BCol>
      </BRow>
    </div>

    <!-- SIN CURSOS -->
    <EmptyState v-if="courses.length === 0" />

  </div>
</template>

<style scoped>
.section-title {
  font-size: 1rem;
  font-weight: 700;
  color: #111827;
  display: flex;
  align-items: center;
}
.section-header {
  border-bottom: 2px solid #eef1ff;
  padding-bottom: 0.75rem;
}
</style>