<script setup lang="ts">
import { reactive, onMounted } from 'vue'
import { useCategoryStore } from '@/stores/CategoryStore'

const emit = defineEmits<{ (e: 'filter', params: any): void }>()
const categoryStore = useCategoryStore()

onMounted(() => {
  categoryStore.getCategories()
})

const filters = reactive({
  Query: '',
  CategoryId: '',
  Level: '',
  SortBy: 'createdAt',
  SortOrder: 'desc',
})

const apply = () => {
  const params: any = {}
  if (filters.Query) params.Query = filters.Query
  if (filters.CategoryId) params.CategoryId = filters.CategoryId
  if (filters.Level) params.Level = filters.Level
  params.SortBy = filters.SortBy
  params.SortOrder = filters.SortOrder
  emit('filter', params)
}

const clear = () => {
  filters.Query = ''
  filters.CategoryId = ''
  filters.Level = ''
  filters.SortBy = 'createdAt'
  filters.SortOrder = 'desc'
  emit('filter', {})
}
</script>

<template>
  <div class="filter-card mb-4 p-4 rounded-3">
    <b-row class="g-3 align-items-end">
      <b-col md="3">
        <label class="form-label filter-label small">Buscar</label>
        <b-form-input v-model="filters.Query" placeholder="Título, descripción..."
          class="filter-input" @keyup.enter="apply" />
      </b-col>

      <b-col md="2">
        <label class="form-label filter-label small">Categoría</label>
        <b-form-select v-model="filters.CategoryId" class="filter-input">
          <option value="">Todas</option>
          <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
            {{ cat.name }}
          </option>
        </b-form-select>
      </b-col>

      <b-col md="2">
        <label class="form-label filter-label small">Nivel</label>
        <b-form-select v-model="filters.Level" class="filter-input">
          <option value="">Todos</option>
          <option value="Beginner">Principiante</option>
          <option value="Intermediate">Intermedio</option>
          <option value="Advanced">Avanzado</option>
        </b-form-select>
      </b-col>

      <b-col md="2">
        <label class="form-label filter-label small">Ordenar por</label>
        <b-form-select v-model="filters.SortBy" class="filter-input">
          <option value="createdAt">Fecha</option>
          <option value="price">Precio</option>
          <option value="rating">Valoración</option>
          <option value="title">Título</option>
        </b-form-select>
      </b-col>

      <b-col md="2" class="d-flex gap-2">
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

.filter-input::placeholder {
  color: var(--bs-secondary-color);
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