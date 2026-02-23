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
  <b-card class="mb-4 border-0 shadow-sm">
    <b-row class="g-3 align-items-end">
      <b-col md="3">
        <b-form-group label="Buscar" class="mb-0">
          <b-form-input v-model="filters.Query" placeholder="Título, descripción..." @keyup.enter="apply" />
        </b-form-group>
      </b-col>

      <b-col md="2">
        <b-form-group label="Categoría" class="mb-0">
          <b-form-select v-model="filters.CategoryId">
            <option value="">Todas</option>
            <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
              {{ cat.name }}
            </option>
          </b-form-select>
        </b-form-group>
      </b-col>

      <b-col md="2">
        <b-form-group label="Nivel" class="mb-0">
          <b-form-select v-model="filters.Level">
            <option value="">Todos</option>
            <option value="Beginner">Principiante</option>
            <option value="Intermediate">Intermedio</option>
            <option value="Advanced">Avanzado</option>
          </b-form-select>
        </b-form-group>
      </b-col>

      <b-col md="2">
        <b-form-group label="Ordenar por" class="mb-0">
          <b-form-select v-model="filters.SortBy">
            <option value="createdAt">Fecha</option>
            <option value="price">Precio</option>
            <option value="rating">Valoración</option>
            <option value="title">Título</option>
          </b-form-select>
        </b-form-group>
      </b-col>

      <b-col md="1">
        <b-form-group label="Orden" class="mb-0">
          <b-form-select v-model="filters.SortOrder">
            <option value="desc">↓</option>
            <option value="asc">↑</option>
          </b-form-select>
        </b-form-group>
      </b-col>

      <b-col md="2" class="d-flex gap-2">
        <b-button variant="primary" @click="apply">Buscar</b-button>
        <b-button variant="outline-secondary" @click="clear">Limpiar</b-button>
      </b-col>
    </b-row>
  </b-card>
</template>