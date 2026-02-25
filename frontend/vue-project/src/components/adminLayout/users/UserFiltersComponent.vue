<script setup lang="ts">
import { reactive } from 'vue'

const emit = defineEmits<{ (e: 'filter', params: { search: string; role: string }): void }>()

const filters = reactive({
  search: '',
  role: '',
})

const apply = () => {
  emit('filter', { ...filters })
}

const clear = () => {
  filters.search = ''
  filters.role = ''
  emit('filter', { search: '', role: '' })
}
</script>

<template>
  <div class="filter-card mb-4 p-4 rounded-3">
    <b-row class="g-3 align-items-end">
      <b-col md="5">
        <label class="form-label filter-label small">Buscar</label>
        <b-form-input
          v-model="filters.search"
          placeholder="Nombre o email..."
          class="filter-input"
          @keyup.enter="apply"
        />
      </b-col>

      <b-col md="3">
        <label class="form-label filter-label small">Rol</label>
        <b-form-select v-model="filters.role" class="filter-input">
          <option value="">Todos</option>
          <option value="admin">Administrador</option>
          <option value="instructor">Instructor</option>
          <option value="student">Estudiante</option>
        </b-form-select>
      </b-col>

      <b-col md="4" class="d-flex gap-2">
        <b-button class="btn-search fw-bold" @click="apply">Buscar</b-button>
        <b-button class="btn-clear" @click="clear">Limpiar</b-button>
      </b-col>
    </b-row>
  </div>
</template>

<style scoped>
.filter-card {
  background: var(--bs-tertiary-bg);
  border: 1px solid var(--bs-border-color);
  box-shadow: 0 2px 12px rgba(34, 34, 196, 0.08);
}

.filter-label {
  color: var(--bs-primary);
  font-weight: 600;
}

.filter-input {
  background: var(--bs-body-bg);
  border: 1px solid var(--bs-border-color);
  color: var(--bs-body-color);
  border-radius: 8px;
}

.filter-input:focus {
  background: var(--bs-body-bg);
  border-color: var(--bs-primary);
  color: var(--bs-body-color);
  box-shadow: 0 0 0 0.2rem rgba(34, 34, 196, 0.15);
}

.filter-input option {
  background: var(--bs-body-bg);
  color: var(--bs-body-color);
}

.btn-search {
  background: var(--bs-primary);
  border: none;
  color: white;
  border-radius: 8px;
  padding: 0.375rem 1rem;
}

.btn-search:hover {
  background: var(--bs-primary);
  filter: brightness(0.85);
}

.btn-clear {
  background: transparent;
  border: 1px solid var(--bs-primary);
  color: var(--bs-primary);
  border-radius: 8px;
  padding: 0.375rem 1rem;
}

.btn-clear:hover {
  background: var(--bs-primary);
  color: white;
}
</style>