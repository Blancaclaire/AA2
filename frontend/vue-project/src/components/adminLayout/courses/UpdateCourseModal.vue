<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useCategoryStore } from '@/stores/CategoryStore'

const props = defineProps<{
  show: boolean
  course: any | null
}>()

const emit = defineEmits<{
  'close': []
  'save': [payload: any]
}>()

const categoryStore = useCategoryStore()


onMounted(() => {
  categoryStore.getCategories()
})

const formData = ref({
  title: '',
  description: '',
  level: '',
  categoryId: 0,
  price: 0,
  durationHours: 0,
  instructor: ''
})

watch(() => props.course, (newCourse) => {
  if (newCourse) {
    formData.value = {
      title: newCourse.title || '',
      description: newCourse.description || '',
      level: newCourse.level || '',
      categoryId: newCourse.categoryId || 0,
      price: newCourse.price || 0,
      durationHours: newCourse.durationHours || 0,
      instructor: newCourse.instructor || ''
    }
  }
}, { immediate: true })

const levelOptions = [
  { value: '', text: 'Selecciona un nivel' },
  { value: 'Beginner', text: 'Principiante' },
  { value: 'Intermediate', text: 'Intermedio' },
  { value: 'Advanced', text: 'Avanzado' }
]

const handleSave = () => {
  const payload = {
    title: formData.value.title,
    description: formData.value.description,
    level: formData.value.level,
    categoryId: formData.value.categoryId,
    price: formData.value.price,
    durationHours: formData.value.durationHours,
    instructor: formData.value.instructor,
    imageUrl: ''  // Siempre vacío
  }
  
  console.log('📦 Payload completo:', payload)
  
  emit('save', payload)
}
</script>

<template>
  <b-modal 
    :model-value="show" 
    title="Editar Curso" 
    size="lg"
    centered
    @hidden="emit('close')"
  >
    <!-- Título -->
    <b-form-group label="Título:" label-for="edit-title" class="mb-3">
      <b-form-input 
        id="edit-title" 
        v-model="formData.title" 
        type="text"
        placeholder="Nombre del curso"
        required
      ></b-form-input>
    </b-form-group>

    <!-- Descripción -->
    <b-form-group label="Descripción:" label-for="edit-description" class="mb-3">
      <b-form-textarea 
        id="edit-description" 
        v-model="formData.description" 
        rows="3"
        placeholder="Descripción del curso"
        required
      ></b-form-textarea>
    </b-form-group>

    <!-- Instructor -->
    <b-form-group label="Instructor:" label-for="edit-instructor" class="mb-3">
      <b-form-input 
        id="edit-instructor" 
        v-model="formData.instructor" 
        type="text"
        placeholder="Nombre del instructor"
        required
      ></b-form-input>
    </b-form-group>

    <!-- Categoría -->
    <b-form-group label="Categoría:" label-for="edit-category" class="mb-3">
      <b-form-select 
        id="edit-category" 
        v-model="formData.categoryId" 
        required
      >
        <option :value="0">Selecciona una categoría</option>
        <option 
          v-for="cat in categoryStore.categories" 
          :key="cat.id" 
          :value="cat.id"
        >
          {{ cat.name }}
        </option>
      </b-form-select>
    </b-form-group>

    <b-row>
      <!-- Nivel -->
      <b-col md="6">
        <b-form-group label="Nivel:" label-for="edit-level" class="mb-3">
          <b-form-select 
            id="edit-level" 
            v-model="formData.level"
            :options="levelOptions"
            required
          ></b-form-select>
        </b-form-group>
      </b-col>

      <!-- Duración -->
      <b-col md="6">
        <b-form-group label="Duración (horas):" label-for="edit-duration" class="mb-3">
          <b-form-input 
            id="edit-duration" 
            v-model.number="formData.durationHours" 
            type="number"
            min="0"
            step="0.5"
            placeholder="Ej: 10"
            required
          ></b-form-input>
        </b-form-group>
      </b-col>
    </b-row>

    <b-row>
      <!-- Precio -->
      <b-col md="6">
        <b-form-group label="Precio (€):" label-for="edit-price" class="mb-3">
          <b-form-input 
            id="edit-price" 
            v-model.number="formData.price" 
            type="number"
            min="0"
            step="0.01"
            placeholder="Ej: 49.99"
            required
          ></b-form-input>
        </b-form-group>
      </b-col>
    </b-row>

    <template #footer>
      <b-button variant="secondary" @click="emit('close')">
        Cancelar
      </b-button>
      <b-button variant="primary" @click="handleSave">
        Guardar Cambios
      </b-button>
    </template>
  </b-modal>
</template>