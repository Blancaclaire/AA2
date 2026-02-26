<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import ListUsersComponent from '@/components/adminLayout/users/ListUsersComponent.vue'
import RegisterFormComponent from '@/components/publicLayout/RegisterFormComponent.vue'
import UpdateUserModal from '@/components/adminLayout/users/UpdateUserModal.vue'
import { useAuthStore } from '@/stores/AuthStore'
import { useUserStore } from '@/stores/UserStore'
import UserFiltersComponent from '@/components/adminLayout/users/UserFiltersComponent.vue'

const userStore = useUserStore()
const authStore = useAuthStore()

onMounted(() => {
  userStore.getUsers()
})


const handleRegisterAction = async (data: any) => {
  if (authStore.role === 'admin') {
    const success = await userStore.postUser(data)
    if (success) await userStore.getUsers()
  }
}

//Borrar usuarios

const pendingDeleteId = ref<number | null>(null)

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

//Modal para editar usuario

const showEditModal = ref(false)
const editingUser = ref<any | null>(null)
const handleEditUser = (user: any) => {
  editingUser.value = user
  showEditModal.value = true
}

const handleSaveUser = async (payload: { id: number; role: string }) => {
  const success = await userStore.updateUser(payload.id, {
    role: payload.role,
  })
  if (success) await userStore.getUsers()
  showEditModal.value = false
  editingUser.value = null
}


//filtrar usuarios en memoria

const search = ref('')
const roleFilter = ref('')

const filteredUsers = computed(() => {
  return userStore.users.filter(u => {
    const matchesSearch = !search.value ||
      u.name.toLowerCase().includes(search.value.toLowerCase()) ||
      u.email.toLowerCase().includes(search.value.toLowerCase())

    const matchesRole = !roleFilter.value || u.role === roleFilter.value
    
    return matchesSearch && matchesRole
  })
})

const onFilter = (params: { search: string; role: string }) => {
  search.value = params.search
  roleFilter.value = params.role
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

    <b-row>
      <b-col lg="4" class="mb-4">
        <div class="position-sticky" style="top: 1rem;">
          <RegisterFormComponent @submit-register="handleRegisterAction" />
        </div>
      </b-col>

      <b-col lg="8">
        <UserFiltersComponent @filter="onFilter" />
        <ListUsersComponent 
          :users="filteredUsers" 
          @delete-user="handleDeleteUser" 
          @update-user="handleEditUser"
        />
      </b-col>
    </b-row>

    <UpdateUserModal
      :show="showEditModal"
      :user="editingUser"
      @close="showEditModal = false"
      @save="handleSaveUser"
    />

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
        <p class="mb-0 fs-5">¿Estás seguro de que quieres eliminar este usuario?</p>
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