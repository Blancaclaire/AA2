<script setup lang="ts">

const props = defineProps<{ user: any }>()
const emit = defineEmits(['delete-user', 'update-user'])

const getRoleBadgeVariant = (role: string) => {
  switch (role) {
    case 'admin': return 'danger'
    case 'instructor': return 'warning'
    default: return 'info'
  }
}

const getRoleLabel = (role: string) => {
  switch (role) {
    case 'admin': return 'Administrador'
    case 'instructor': return 'Instructor'
    default: return 'Estudiante'
  }
}

const getInitials = (name: string) => {
  return name
    .split(' ')
    .map(w => w[0])
    .join('')
    .toUpperCase()
    .slice(0, 2)
}

const formatDate = (date: string | null) => {
  if (!date) return 'Nunca'
  return new Date(date).toLocaleDateString('es-ES', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  })

}



</script>

<template>
  <b-card class="user-card shadow-sm border-0 h-100" no-body>
    <b-card-body class="d-flex flex-column">
      <!-- Header-->
      <div class="d-flex align-items-center mb-3">
        <div class="user-avatar me-3" :class="`avatar-${user.role}`">
          {{ getInitials(user.name) }}
        </div>
        <div class="flex-grow-1 min-width-0">
          <h6 class="mb-0 fw-bold text-truncate">{{ user.name }}</h6>
          <small class="text-muted text-truncate d-block">{{ user.email }}</small>
        </div>
        <b-badge :variant="getRoleBadgeVariant(user.role)" class="ms-2 align-self-start">
          {{ getRoleLabel(user.role) }}
        </b-badge>
      </div>

      <!-- Stats -->
      <div class="d-flex justify-content-between text-muted small mb-3 px-1">
        <div>
          <span class="fw-semibold d-block">{{ user.enrollmentCount ?? 0 }}</span>
          <span>Cursos</span>
        </div>
        <div class="text-center">
          <span class="fw-semibold d-block">{{ formatDate(user.createdAt) }}</span>
          <span>Registro</span>
        </div>
        <div class="text-end">
          <span class="fw-semibold d-block">{{ formatDate(user.lastLoginAt) }}</span>
          <span>Último acceso</span>
        </div>
      </div>

      <!-- Status -->
      <div class="d-flex align-items-center mb-3">
        <span class="status-dot me-2" :class="user.isActive ? 'bg-success' : 'bg-secondary'"></span>
        <small :class="user.isActive ? 'text-success' : 'text-muted'">
          {{ user.isActive ? 'Activo' : 'Inactivo' }}
        </small>
      </div>

      <!-- Action Buttons -->
      <div class="mt-auto d-flex gap-2">
        <b-button variant="outline-primary" size="sm" class="flex-grow-1" @click="emit('update-user', user)">
          Editar
        </b-button>
        <b-button variant="outline-danger" size="sm" class="flex-grow-1" @click="emit('delete-user', user.id)">
          Eliminar
        </b-button>
      </div>
    </b-card-body>
  </b-card>
</template>

<style scoped>
.user-card {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  background-color: var(--bs-tertiary-bg) !important;
}

.user-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 0.5rem 1.5rem rgba(0, 0, 0, 0.12) !important;
}

.user-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.85rem;
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

.status-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  display: inline-block;
  flex-shrink: 0;
}

.min-width-0 {
  min-width: 0;
}
</style>