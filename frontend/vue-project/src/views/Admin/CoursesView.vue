<script setup lang="ts">
import CourseForm from '@/components/adminLayout/courses/CourseForm.vue';
import { useCourseStore } from '@/stores/CoursesStore';
import { useAuthStore } from '@/stores/AuthStore'
import CourseFilters from '@/components/publicLayout/home/CoursesFilter.vue'
import ListCoursesComponent from '@/components/publicLayout/home/ListCoursesComponent.vue';
import { onMounted, ref } from 'vue';

const courseStore = useCourseStore()

onMounted(() => {
  courseStore.getCourses()
})
const authStore = useAuthStore()

const onFilter = (params: any) => {
  courseStore.getCourses(params)
}

//Borrar cursos
const pendingDeleteId = ref<number | null>(null)

const handleDeleteUser = (id: number) => {
  pendingDeleteId.value = id
}

const confirmDelete = async () => {
  if (pendingDeleteId.value !== null) {
    const success = await courseStore.deleteCourse(pendingDeleteId.value)
    if (success) await courseStore.getCourses()
    pendingDeleteId.value = null
  }
}

const cancelDelete = () => {
  pendingDeleteId.value = null
}
</script>

<template>
  <div class="container py-4 mb-5">
    <!-- Header -->
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h2 class="fw-bold mb-1">Gestión de Cursos</h2>
        <p class="text-muted mb-0">{{ courseStore.courses.length }} cursos disponibles</p>
      </div>
    </div>

    <b-row>
      <b-col lg="4" class="mb-4">
        <div class="position-sticky" style="top: 1rem;">
          <CourseForm />
        </div>
      </b-col>

      <b-col lg="8">
        <CourseFilters @filter="onFilter" />
        <ListCoursesComponent 
          :courses="courseStore.courses" 
          @delete-course="handleDeleteUser"
        />
      </b-col>
    </b-row>

    <b-modal 
      :model-value="pendingDeleteId !== null"
      title="Confirmar eliminación"
      centered
      @hidden="cancelDelete"
    >
      <div class="text-center py-3">
        <div class="mb-3">
          <b-icon icon="exclamation-triangle-fill" font-scale="3" variant="danger"></b-icon>
        </div>
        <p class="mb-0 fs-5">¿Estás seguro de que quieres eliminar este curso?</p>
        <p class="text-muted small mt-2">Esta acción no se puede deshacer</p>
      </div>

      <template #footer>
        <b-button variant="secondary" @click="cancelDelete">
          Cancelar
        </b-button>
        <b-button variant="danger" @click="confirmDelete">
          Sí, eliminar
        </b-button>
      </template>
    </b-modal>
  </div>
</template>