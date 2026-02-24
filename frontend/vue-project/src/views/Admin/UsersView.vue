<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ListUsersComponent from '@/components/adminLayout/users/ListUsersComponent.vue'
import RegisterFormComponent from '@/components/publicLayout/RegisterFormComponent.vue'
import { useAuthStore } from '@/stores/AuthStore'
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()
const authStore = useAuthStore()

const pendingDeleteId = ref<number | null>(null)

onMounted(() => {
  userStore.getUsers()
})

const handleRegisterAction = async (data: any) => {
  if (authStore.role === 'admin') {
    const success = await userStore.postUser(data)
    if (success) await userStore.getUsers()
  }
}

const handleDeleteUser = (id: number) => {
  pendingDeleteId.value = id
}

const confirmDelete = async () => {
  if (pendingDeleteId.value !== null) {
    const success = await userStore.deleteUser(pendingDeleteId.value)
    if (success) await userStore.getUsers()
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
        <h2 class="fw-bold mb-1">Gestión de Usuarios</h2>
        <p class="text-muted mb-0">{{ userStore.users.length }} usuarios registrados</p>
      </div>
    </div>

    <!-- Alerta de eliminación -->
    <div v-if="pendingDeleteId" class="alert alert-danger d-flex align-items-center justify-content-between mb-4">
      <span>¿Estás seguro de que quieres eliminar este usuario?</span>
      <div class="d-flex gap-2">
        <b-button variant="danger" size="sm" @click="confirmDelete">Sí, eliminar</b-button>
        <b-button variant="outline-secondary" size="sm" @click="cancelDelete">Cancelar</b-button>
      </div>
    </div>

    <b-row>
      <!-- Columna izquierda: Formulario -->
      <b-col lg="4" class="mb-4">
        <div class="position-sticky" style="top: 1rem;">
          <RegisterFormComponent @submit-register="handleRegisterAction" />
        </div>
      </b-col>

      <!-- Columna derecha: Filtros + Lista -->
      <b-col lg="8">
        <!-- Aquí irá el componente de filtros -->
        <!-- <UserFiltersComponent /> -->

        <ListUsersComponent :users="userStore.users" @delete-user="handleDeleteUser" />
      </b-col>
    </b-row>
  </div>
</template>