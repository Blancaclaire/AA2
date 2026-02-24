<script setup lang="ts">
import { useRouter } from 'vue-router'

const props = defineProps<{ course: any }>()
const router = useRouter()

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })
}

const progressVariant = (percent: number) => {
  if (percent >= 75) return 'success'
  if (percent >= 40) return 'warning'
  return 'primary'
}
</script>

<template>
  <!-- EN PROGRESO -->
  <BCard v-if="!course.isCompleted" class="course-card h-100 border-0 shadow-sm" no-body>

    <BCardBody class="p-4 d-flex flex-column">
      <div class="d-flex justify-content-between align-items-start mb-3">
        <BBadge variant="warning" pill class="text-dark">En progreso</BBadge>
        <span class="text-muted small">{{ formatDate(course.enrolledAt) }}</span>
      </div>

      <h6 class="course-title mb-3">{{ course.courseTitle }}</h6>
      <div class="mt-auto">
        <div class="d-flex justify-content-between mb-1">
          <span class="small text-muted fw-semibold">Progreso</span>
          <span class="small fw-bold" :class="`text-${progressVariant(course.progressPercent)}`">{{ course.progressPercent }}%</span>
        </div>
        <BProgress :value="course.progressPercent" :variant="progressVariant(course.progressPercent)" height="6px" class="mb-4" animated />
        <BButton variant="primary" class="w-100 fw-bold" pill @click="router.push(`/detail/${course.courseId}`)">Continuar</BButton>
      </div>
    </BCardBody>
  </BCard>

  <!-- COMPLETADO -->
  <BCard v-else class="course-card h-100 border-0 shadow-sm" no-body>
    <BCardBody class="p-4 d-flex flex-column">

      <div class="d-flex justify-content-between align-items-start mb-3">
        <BBadge variant="success" pill>Completado</BBadge>
        <span class="text-muted small">{{ formatDate(course.enrolledAt) }}</span>
      </div>

      <h6 class="course-title mb-3">{{ course.courseTitle }}</h6>

      <div class="mt-auto">
        <div class="d-flex justify-content-between mb-1">
          <span class="small text-muted fw-semibold">Progreso</span>
          <span class="small fw-bold text-success">100%</span>
        </div>
        <BProgress :value="100" variant="success" height="6px" class="mb-4" />
        <BButton variant="outline-success" class="w-100 fw-bold" pill @click="router.push(`/detail/${course.courseId}`)">Ver curso</BButton>
      </div>
      
    </BCardBody>
  </BCard>
</template>

<style scoped>
.course-card {
  border-radius: 14px !important;
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  background-color: var(--bs-tertiary-bg) !important;
}
.course-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 24px rgba(34, 34, 196, 0.12) !important;
}
.card-top-bar {
  height: 5px;
  width: 100%;
}
.course-title {
  font-size: 0.97rem;
  font-weight: 700;
  color: var(--bs-body-color);
  line-height: 1.4;
}
</style>