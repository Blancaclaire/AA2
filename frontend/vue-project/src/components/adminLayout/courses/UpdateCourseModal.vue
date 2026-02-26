<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useCategoryStore } from '@/stores/CategoryStore'
import { useForm } from 'vee-validate'
import * as yup from 'yup'

const props = defineProps<{
  show: boolean
  course: any | null
}>()

const emit = defineEmits<{
  'close': []
  'save': [payload: any]
}>()

const categoryStore = useCategoryStore()

// Controlar qué campos han sido tocados
const touchedFields = ref({
  title: false,
  description: false,
  instructor: false,
  categoryId: false,
  level: false,
  durationHours: false,
  price: false
})

onMounted(() => {
  categoryStore.getCategories()
})

// validación con Yup
const validationSchema = yup.object({
  title: yup.string()
    .required('El título es obligatorio')
    .min(5, 'El título debe tener al menos 5 caracteres')
    .max(100, 'El título no puede exceder 100 caracteres'),
  description: yup.string()
    .required('La descripción es obligatoria')
    .min(10, 'La descripción debe tener al menos 10 caracteres')
    .max(500, 'La descripción no puede exceder 500 caracteres'),
  instructor: yup.string()
    .required('El instructor es obligatorio'),
  level: yup.string()
    .required('El nivel es obligatorio')
    .oneOf(['Beginner', 'Intermediate', 'Advanced'], 'Nivel inválido'),
  categoryId: yup.number()
    .required('La categoría es obligatoria')
    .min(1, 'Selecciona una categoría válida')
    .typeError('Debe seleccionar una categoría'),
  price: yup.number()
    .required('El precio es obligatorio')
    .min(0, 'El precio debe ser positivo')
    .typeError('Debe ser un número válido'),
  durationHours: yup.number()
    .required('La duración es obligatoria')
    .min(1, 'La duración debe ser al menos 1 hora')
    .typeError('Debe ser un número válido')
})

const { handleSubmit, errors, setValues, defineField } = useForm({
  validationSchema,
  initialValues: {
    title: '',
    description: '',
    instructor: '',
    level: '',
    categoryId: 0,
    price: 0,
    durationHours: 0
  }
})

const [title] = defineField('title')
const [description] = defineField('description')
const [instructor] = defineField('instructor')
const [level] = defineField('level')
const [categoryId] = defineField('categoryId')
const [price] = defineField('price')
const [durationHours] = defineField('durationHours')

watch(() => props.course, (newCourse) => {
  if (newCourse) {
    setValues({
      title: newCourse.title || '',
      description: newCourse.description || '',
      instructor: newCourse.instructor || '',
      level: newCourse.level || '',
      categoryId: newCourse.categoryId || 0,
      price: newCourse.price || 0,
      durationHours: newCourse.durationHours || 0
    })

    touchedFields.value = {
      title: false,
      description: false,
      instructor: false,
      categoryId: false,
      level: false,
      durationHours: false,
      price: false
    }
  }
}, { immediate: true })

const levelOptions = [
  { value: '', text: 'Selecciona un nivel' },
  { value: 'Beginner', text: 'Principiante' },
  { value: 'Intermediate', text: 'Intermedio' },
  { value: 'Advanced', text: 'Avanzado' }
]


const markAsTouched = (field: keyof typeof touchedFields.value) => {
  touchedFields.value[field] = true
}


const getFieldState = (field: keyof typeof touchedFields.value) => {
  if (touchedFields.value[field] && errors.value[field]) {
    return false 
  }
  return null 
}

const handleSave = handleSubmit((values) => {
  touchedFields.value = {
    title: true,
    description: true,
    instructor: true,
    categoryId: true,
    level: true,
    durationHours: true,
    price: true
  }

  const payload = {
    ...values,
    imageUrl: '',
    isPublished: true
  }
  
  
  emit('save', payload)
})
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
    <b-form-group 
      label="Título:" 
      label-for="edit-title" 
      class="mb-3"
      :invalid-feedback="errors.title"
      :state="getFieldState('title')"
    >
      <b-form-input 
        id="edit-title" 
        v-model="title" 
        type="text"
        placeholder="Nombre del curso"
        :state="getFieldState('title')"
        @blur="markAsTouched('title')"
      ></b-form-input>
    </b-form-group>

    <!-- Descripción -->
    <b-form-group 
      label="Descripción:" 
      label-for="edit-description" 
      class="mb-3"
      :invalid-feedback="errors.description"
      :state="getFieldState('description')"
    >
      <b-form-textarea 
        id="edit-description" 
        v-model="description" 
        rows="3"
        placeholder="Descripción del curso"
        :state="getFieldState('description')"
        @blur="markAsTouched('description')"
      ></b-form-textarea>
    </b-form-group>

    <!-- Instructor -->
    <b-form-group 
      label="Instructor:" 
      label-for="edit-instructor" 
      class="mb-3"
      :invalid-feedback="errors.instructor"
      :state="getFieldState('instructor')"
    >
      <b-form-input 
        id="edit-instructor" 
        v-model="instructor" 
        type="text"
        placeholder="Nombre del instructor"
        :state="getFieldState('instructor')"
        @blur="markAsTouched('instructor')"
      ></b-form-input>
    </b-form-group>

    <!-- Categoría -->
    <b-form-group 
      label="Categoría:" 
      label-for="edit-category" 
      class="mb-3"
      :invalid-feedback="errors.categoryId"
      :state="getFieldState('categoryId')"
    >
      <b-form-select 
        id="edit-category" 
        v-model="categoryId"
        :state="getFieldState('categoryId')"
        @blur="markAsTouched('categoryId')"
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
        <b-form-group 
          label="Nivel:" 
          label-for="edit-level" 
          class="mb-3"
          :invalid-feedback="errors.level"
          :state="getFieldState('level')"
        >
          <b-form-select 
            id="edit-level" 
            v-model="level"
            :options="levelOptions"
            :state="getFieldState('level')"
            @blur="markAsTouched('level')"
          ></b-form-select>
        </b-form-group>
      </b-col>

      <!-- Duración -->
      <b-col md="6">
        <b-form-group 
          label="Duración (horas):" 
          label-for="edit-duration" 
          class="mb-3"
          :invalid-feedback="errors.durationHours"
          :state="getFieldState('durationHours')"
        >
          <b-form-input 
            id="edit-duration" 
            v-model="durationHours" 
            type="number"
            min="0"
            step="0.5"
            placeholder="Ej: 10"
            :state="getFieldState('durationHours')"
            @blur="markAsTouched('durationHours')"
          ></b-form-input>
        </b-form-group>
      </b-col>
    </b-row>

    <b-row>
      <!-- Precio -->
      <b-col md="6">
        <b-form-group 
          label="Precio (€):" 
          label-for="edit-price" 
          class="mb-3"
          :invalid-feedback="errors.price"
          :state="getFieldState('price')"
        >
          <b-form-input 
            id="edit-price" 
            v-model="price" 
            type="number"
            min="0"
            step="0.01"
            placeholder="Ej: 49.99"
            :state="getFieldState('price')"
            @blur="markAsTouched('price')"
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