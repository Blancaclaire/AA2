<script setup lang="ts">
import { useCourseStore } from '@/stores/CoursesStore'
import { useAuthStore } from '@/stores/AuthStore'
import { useRoute, useRouter } from 'vue-router';

const router = useRouter()
const route = useRoute()
const props = defineProps<{ courseId: number }>()
const courseStore = useCourseStore()
const authStore = useAuthStore()

const handleEnroll = async () => {
  await courseStore.enrollUser(props.courseId)
  if (!courseStore.error) {
    router.push('/myCourses')
  }
}

const handleToHome = ()=>{
    router.push(`/`)
}

</script>

<template>
  <BCard class="enroll-card shadow-sm" no-body>
    <BCardBody class="p-4">

      <!-- PRECIO -->
      <div class="enroll-price mb-4">
        {{ courseStore.course?.price }}€
      </div>

      <!-- NO AUTENTICADO -->
      <template v-if="!authStore.isAuthenticated">
        <BButton to="/login" variant="dark" size="lg" class="w-100 mb-2 fw-bold" pill>
          Inicia sesión para matricularte
        </BButton>
        <p class="text-center text-muted small mt-2 mb-0">
          ¿No tienes cuenta?
          <RouterLink to="/register" class="text-primary fw-semibold text-decoration-none">
            Regístrate gratis
          </RouterLink>
        </p>
      </template>

      <!-- YA MATRICULADO -->
      <template v-else-if="courseStore.course?.isEnrolled">
        <BAlert variant="success" show class="d-flex align-items-center gap-2 py-2 mb-3">
          <span class="fs-5">✅</span>
          <span class="fw-bold">Ya estás matriculado</span>
        </BAlert>

        <div class="mb-3">
          <div class="d-flex justify-content-between mb-1">
            <span class="small text-muted fw-semibold">Tu progreso</span>
            <span class="small fw-bold text-success">{{ courseStore.course.userProgress }}%</span>
          </div>
          <BProgress
            :value="courseStore.course.userProgress"
            variant="success"
            height="8px"
            animated
            striped
          />
        </div>

        <BButton variant="success" size="lg" class="w-100 fw-bold" pill @click="handleToHome" >
          Continuar aprendiendo
        </BButton>
      </template>

      <!-- PUEDE MATRICULARSE -->
      <template v-else>
        <BButton
          variant="primary"
          size="lg"
          class="w-100 fw-bold mb-2"
          pill
          :disabled="courseStore.loading"
          @click="handleEnroll"
        >
          <BSpinner v-if="courseStore.loading" small class="me-2" />
          {{ courseStore.loading ? 'Procesando...' : 'Matricularme ahora' }}
        </BButton>

        <BAlert v-if="courseStore.error" variant="danger" show class="py-2 text-center small mt-2">
          ⚠️ {{ courseStore.error }}
        </BAlert>
      </template>

      <hr class="my-4" />

      <!-- GARANTÍAS -->
      <ul class="list-unstyled mb-0 d-flex flex-column gap-2">
        <li class="small text-secondary">✅ Acceso de por vida</li>
        <li class="small text-secondary">✅ Certificado al completar</li>
        <li class="small text-secondary">✅ Soporte del instructor</li>
        <li class="small text-secondary">✅ Contenido actualizado</li>
      </ul>

      <hr class="my-4" />

      <!-- INFO CURSO -->
      <div class="d-flex justify-content-between align-items-center mb-2">
        <span class="small text-muted">Categoría</span>
        <BBadge variant="light" class="text-dark fw-semibold">{{ courseStore.course?.categoryName }}</BBadge>
      </div>
      <div class="d-flex justify-content-between align-items-center mb-2">
        <span class="small text-muted">Duración</span>
        <span class="small fw-semibold text-dark">{{ courseStore.course?.durationHours }} horas</span>
      </div>
      <div class="d-flex justify-content-between align-items-center">
        <span class="small text-muted">Alumnos</span>
        <span class="small fw-semibold text-dark">{{ courseStore.course?.enrollmentCount }}</span>
      </div>

    </BCardBody>
  </BCard>
</template>

<style scoped>
.enroll-card {
  border: 1px solid #e5e7eb;
  border-radius: 16px;
  position: sticky;
  top: 1.5rem;
}

.enroll-price {
  font-size: 2.4rem;
  font-weight: 800;
  color: #2222c4;
  letter-spacing: -1px;
}
</style>