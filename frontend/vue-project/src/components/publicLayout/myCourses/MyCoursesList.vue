<script setup lang="ts">
import { useRouter } from 'vue-router'

const props = defineProps<{ courses: any[] }>()
const router = useRouter()

const inProgress = (courses: any[]) => courses.filter(c => !c.isCompleted)
const completed = (courses: any[]) => courses.filter(c => c.isCompleted)

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
  <div>

    <!-- EN PROGRESO -->
    <div v-if="inProgress(courses).length > 0" class="mb-5">
      <div class="section-header mb-3">
        <h5 class="section-title">En progreso <BBadge variant="primary" pill class="ms-2">{{ inProgress(courses).length }}</BBadge></h5>
      </div>

      <BRow class="g-4">
        <BCol v-for="course in inProgress(courses)" :key="course.courseId" md="6" lg="4">
          <BCard class="course-card h-100 border-0 shadow-sm" no-body>

            <div class="card-top-bar bg-primary"></div>

            <BCardBody class="p-4 d-flex flex-column">
              <div class="d-flex justify-content-between align-items-start mb-3">
                <BBadge variant="warning" pill class="text-dark">En progreso</BBadge>
                <span class="text-muted small">{{ formatDate(course.enrolledAt) }}</span>
              </div>

              <h6 class="course-title mb-3">{{ course.courseTitle }}</h6>

              <div class="mt-auto">
                <div class="d-flex justify-content-between mb-1">
                  <span class="small text-muted fw-semibold">Progreso</span>
                  <span class="small fw-bold" :class="`text-${progressVariant(course.progressPercent)}`">
                    {{ course.progressPercent }}%
                  </span>
                </div>
                <BProgress
                  :value="course.progressPercent"
                  :variant="progressVariant(course.progressPercent)"
                  height="6px"
                  class="mb-4"
                  animated
                />

                <BButton
                  variant="primary"
                  class="w-100 fw-bold"
                  pill
                  @click="router.push(`/detail/${course.courseId}`)"
                >
                  Continuar 
                </BButton>
              </div>
            </BCardBody>
          </BCard>
        </BCol>
      </BRow>
    </div>

    <!-- COMPLETADOS -->
    <div v-if="completed(courses).length > 0">
      <div class="section-header mb-3">
        <h5 class="section-title">Completados <BBadge variant="success" pill class="ms-2">{{ completed(courses).length }}</BBadge></h5>
      </div>

      <BRow class="g-4">
        <BCol v-for="course in completed(courses)" :key="course.courseId" md="6" lg="4">
          <BCard class="course-card h-100 border-0 shadow-sm" no-body>

            <div class="card-top-bar bg-success"></div>

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
                <BProgress
                  :value="100"
                  variant="success"
                  height="6px"
                  class="mb-4"
                />

                <BButton
                  variant="outline-success"
                  class="w-100 fw-bold"
                  pill
                  @click="router.push(`/detail/${course.courseId}`)"
                >
                  Ver curso 
                </BButton>
              </div>
            </BCardBody>
          </BCard>
        </BCol>
      </BRow>
    </div>

    <!-- SIN CURSOS -->
    <div v-if="courses.length === 0" class="empty-state text-center py-5">
      <div class="empty-icon mb-3">📭</div>
      <h5 class="text-muted">No tienes cursos matriculados</h5>
      <p class="text-muted small mb-4">Explora el catálogo y empieza a aprender hoy</p>
      <BButton variant="primary" pill @click="router.push('/')">
        Explorar cursos
      </BButton>
    </div>

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

.course-card {
  border-radius: 14px !important;
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
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
  color: #111827;
  line-height: 1.4;
}

.empty-icon {
  font-size: 3.5rem;
}
</style>