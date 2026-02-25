<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useCategoryStore } from '@/stores/CategoryStore'
import { useUserStore } from '@/stores/UserStore'
import { useCourseStore } from '@/stores/CoursesStore'
import { useForm } from 'vee-validate'
import * as yup from 'yup'

const userStore = useUserStore()
const categoryStore = useCategoryStore()
const courseStore = useCourseStore()

const isSubmitting = ref(false)
const submitSuccess = ref(false)

// ✅ Controlar qué campos han sido tocados
const touchedFields = ref({
  title: false,
  description: false,
  instructor: false,
  price: false,
  durationHours: false,
  level: false,
  categoryId: false
})

onMounted(() => {
    categoryStore.getCategories()
    userStore.getUsers()
})

const levelOptions = [
    { value: '', text: 'Selecciona un nivel' },
    { value: 'Beginner', text: 'Principiante' },
    { value: 'Intermediate', text: 'Intermedio' },
    { value: 'Advanced', text: 'Avanzado' },
]

// ✅ Esquema de validación con Yup
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
  price: yup.number()
    .required('El precio es obligatorio')
    .min(0, 'El precio debe ser mayor o igual a 0')
    .typeError('Debe ser un número válido'),
  durationHours: yup.number()
    .required('La duración es obligatoria')
    .min(1, 'La duración debe ser al menos 1 hora')
    .typeError('Debe ser un número válido'),
  level: yup.string()
    .required('El nivel es obligatorio')
    .oneOf(['Beginner', 'Intermediate', 'Advanced'], 'Nivel inválido'),
  categoryId: yup.number()
    .required('La categoría es obligatoria')
    .min(1, 'Selecciona una categoría válida')
    .typeError('Debe seleccionar una categoría')
})

// ✅ Configurar VeeValidate
const { handleSubmit, errors, resetForm, defineField } = useForm({
  validationSchema,
  initialValues: {
    title: '',
    description: '',
    instructor: '',
    price: 0,
    durationHours: 0,
    level: '',
    categoryId: 0
  }
})

// ✅ Usar defineField para compatibilidad con v-model
const [title] = defineField('title')
const [description] = defineField('description')
const [instructor] = defineField('instructor')
const [price] = defineField('price')
const [durationHours] = defineField('durationHours')
const [level] = defineField('level')
const [categoryId] = defineField('categoryId')

// ✅ Funciones para marcar campos como tocados
const markAsTouched = (field: keyof typeof touchedFields.value) => {
  touchedFields.value[field] = true
}

// ✅ Helper para determinar el estado del campo
const getFieldState = (field: keyof typeof touchedFields.value) => {
  if (touchedFields.value[field] && errors.value[field]) {
    return false // Rojo
  }
  return null // Sin estado (normal)
}

// ✅ Mantener tu lógica exacta
const enviarDatos = handleSubmit(async (values) => {
    // Marcar todos los campos como tocados al intentar enviar
    touchedFields.value = {
      title: true,
      description: true,
      instructor: true,
      price: true,
      durationHours: true,
      level: true,
      categoryId: true
    }

    isSubmitting.value = true
    submitSuccess.value = false

    const dataToSubmit = {
        ...values,
        price: parseFloat(values.price.toString()) || 0,
        durationHours: parseInt(values.durationHours.toString()) || 0,
        categoryId: parseInt(values.categoryId.toString()) || 0,
        imageUrl: '',
        isPublished: true,
    }
    
    console.log('Antes de llamar al store:', dataToSubmit)
    const success = await courseStore.postCourse(dataToSubmit)
    console.log('Resultado:', success, 'Error:', courseStore.error)
    
    isSubmitting.value = false

    if (success) {
        submitSuccess.value = true
        await courseStore.getCourses()
        resetForm()
        
        // Resetear campos tocados
        touchedFields.value = {
          title: false,
          description: false,
          instructor: false,
          price: false,
          durationHours: false,
          level: false,
          categoryId: false
        }
        
        setTimeout(() => { submitSuccess.value = false }, 3000)
    }
})
</script>

<template>
    <b-card header="Crear un curso" header-bg-variant="primary" header-text-variant="white" class="shadow-sm">
        <b-form @submit.prevent="enviarDatos">

            <!-- Título -->
            <b-form-group 
                label="Título:" 
                label-for="input-title" 
                class="mb-3"
                :invalid-feedback="errors.title"
                :state="getFieldState('title')"
            >
                <b-form-input 
                    id="input-title" 
                    v-model="title" 
                    type="text" 
                    placeholder="Programación Vue"
                    :state="getFieldState('title')"
                    @blur="markAsTouched('title')"
                ></b-form-input>
            </b-form-group>

            <!-- Descripción -->
            <b-form-group 
                label="Descripción:" 
                label-for="input-description" 
                class="mb-3"
                :invalid-feedback="errors.description"
                :state="getFieldState('description')"
            >
                <b-form-input 
                    id="input-description" 
                    v-model="description" 
                    type="text" 
                    placeholder="Descripción del curso"
                    :state="getFieldState('description')"
                    @blur="markAsTouched('description')"
                ></b-form-input>
            </b-form-group>

            <!-- Categoría -->
            <b-form-group 
                label="Categoría:" 
                label-for="input-category" 
                class="mb-3"
                :invalid-feedback="errors.categoryId"
                :state="getFieldState('categoryId')"
            >
                <b-form-select 
                    id="input-category" 
                    v-model="categoryId"
                    :state="getFieldState('categoryId')"
                    @blur="markAsTouched('categoryId')"
                >
                    <option :value="0">Selecciona una categoría</option>
                    <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
                        {{ cat.name }}
                    </option>
                </b-form-select>
            </b-form-group>

            <!-- Instructor -->
            <b-form-group 
                label="Instructor:" 
                label-for="input-instructor" 
                class="mb-3"
                :invalid-feedback="errors.instructor"
                :state="getFieldState('instructor')"
            >
                <b-form-select 
                    id="input-instructor" 
                    v-model="instructor"
                    :state="getFieldState('instructor')"
                    @blur="markAsTouched('instructor')"
                >
                    <option value="">Selecciona un instructor</option>
                    <option v-for="u in userStore.users.filter(u => u.role === 'instructor' || u.role === 'admin')"
                        :key="u.id" :value="u.name">
                        {{ u.name }}
                    </option>
                </b-form-select>
            </b-form-group>

            <!-- Precio -->
            <b-form-group 
                label="Precio:" 
                label-for="input-price" 
                class="mb-3"
                :invalid-feedback="errors.price"
                :state="getFieldState('price')"
            >
                <b-form-input 
                    id="input-price" 
                    v-model="price" 
                    type="number" 
                    step="0.01"
                    placeholder="14.99"
                    :state="getFieldState('price')"
                    @blur="markAsTouched('price')"
                ></b-form-input>
            </b-form-group>

            <!-- Duración -->
            <b-form-group 
                label="Duración (horas):" 
                label-for="input-duration" 
                class="mb-3"
                :invalid-feedback="errors.durationHours"
                :state="getFieldState('durationHours')"
            >
                <b-form-input 
                    id="input-duration" 
                    v-model="durationHours" 
                    type="number" 
                    placeholder="10"
                    :state="getFieldState('durationHours')"
                    @blur="markAsTouched('durationHours')"
                ></b-form-input>
            </b-form-group>

            <!-- Nivel -->
            <b-form-group 
                label="Nivel:" 
                label-for="input-level" 
                class="mb-3"
                :invalid-feedback="errors.level"
                :state="getFieldState('level')"
            >
                <b-form-select 
                    id="input-level" 
                    v-model="level" 
                    :options="levelOptions"
                    :state="getFieldState('level')"
                    @blur="markAsTouched('level')"
                />
            </b-form-group>

            <b-button 
                type="submit" 
                :variant="submitSuccess ? 'success' : 'primary'" 
                :disabled="isSubmitting"
                class="w-100 mt-3"
            >
                <span v-if="isSubmitting">
                    <b-spinner small class="me-1"></b-spinner>
                    Añadiendo...
                </span>
                <span v-else-if="submitSuccess">
                    ¡Curso añadido!
                </span>
                <span v-else>
                    Añadir
                </span>
            </b-button>

        </b-form>
    </b-card>
</template>

<style scoped>
.shadow-sm {
    background-color: var(--bs-tertiary-bg) !important;
}
</style>