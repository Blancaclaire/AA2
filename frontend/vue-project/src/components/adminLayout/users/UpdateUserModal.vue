<script setup lang="ts">
import { ref, watch } from 'vue'

const props = defineProps<{
  show: boolean
  user: any | null
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'save', payload: { id: number; role: string }): void
}>()

const selectedRole = ref('')

const roleOptions = [
  { value: 'student', text: 'Estudiante' },
  { value: 'instructor', text: 'Instructor' },
  { value: 'admin', text: 'Administrador' },
]

watch(
   //vigila 
  () => props.user,
  //define que hacer cuando cambia
  (u) => {
    if (u) {
      selectedRole.value = u.role
    }
  },
  { immediate: true }
)

const handleSave = () => {
  if (!props.user) return
  emit('save', {
    id: props.user.id,
    role: selectedRole.value,
  })
}
</script>

<template>
  <div
    v-if="show"
    class="modal-backdrop-custom"
    @click.self="emit('close')"
  >
    <div class="modal-dialog-custom">
      <BCard no-body class="border-0 shadow">
        <!-- Header -->
        <div class="modal-header-custom d-flex justify-content-between align-items-center p-3">
          <h6 class="mb-0 fw-bold text-white">Editar Usuario</h6>
          <BButton
            variant="link"
            class="text-white p-0 text-decoration-none fs-5"
            @click="emit('close')"
          >
            ✕
          </BButton>
        </div>

        <BCardBody class="p-4">
          <!-- Info del usuario -->
          <div v-if="user" class="d-flex align-items-center mb-4">
            <div class="user-avatar-modal me-3" :class="`avatar-${user.role}`">
              {{ user.name?.split(' ').map((w: string) => w[0]).join('').toUpperCase().slice(0, 2) }}
            </div>
            <div>
              <h6 class="mb-0 fw-bold">{{ user.name }}</h6>
              <small class="text-muted">{{ user.email }}</small>
            </div>
          </div>

          <!-- Rol -->
          <BFormGroup label="Rol:" label-for="edit-role" class="mb-3">
            <BFormSelect
              id="edit-role"
              v-model="selectedRole"
              :options="roleOptions"
            />
          </BFormGroup>

          <!-- Botones -->
          <div class="d-flex gap-2 justify-content-end">
            <BButton variant="outline-secondary" @click="emit('close')">
              Cancelar
            </BButton>
            <BButton variant="primary" @click="handleSave">
              Guardar cambios
            </BButton>
          </div>
        </BCardBody>
      </BCard>
    </div>
  </div>
</template>

<style scoped>
.modal-backdrop-custom {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}

.modal-dialog-custom {
  width: 100%;
  max-width: 440px;
  margin: 1rem;
}

.modal-header-custom {
  background: linear-gradient(135deg, #2222c4 0%, #1a1a2e 100%);
  border-radius: 0.375rem 0.375rem 0 0;
}

.user-avatar-modal {
  width: 44px;
  height: 44px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.8rem;
  color: #fff;
  flex-shrink: 0;
}

.avatar-admin {
  background: linear-gradient(135deg, #dc3545, #b02a37);
}
.avatar-instructor {
  background: linear-gradient(135deg, #ffc107, #e0a800);
  color: #212529;
}
.avatar-student {
  background: linear-gradient(135deg, #0d6efd, #0a58ca);
}
</style>