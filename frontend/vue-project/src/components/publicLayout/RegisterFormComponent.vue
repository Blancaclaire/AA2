<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/AuthStore'
import { useForm } from 'vee-validate'
import * as yup from 'yup'

const authStore = useAuthStore()
const emit = defineEmits(['submit-register'])
const showPassword = ref(false)
const isSubmitting = ref(false)
const submitSuccess = ref(false)

const touchedFields = ref({
  role: false,
  name: false,
  email: false,
  password: false
})

const roleOptions = ref([
  { value: '', text: 'Selecciona un rol' },
  { value: 'admin', text: 'Administrador' },
  { value: 'instructor', text: 'Instructor' }
])

const validationSchema = yup.object({
  role: authStore.role === 'admin' 
    ? yup.string().required('El rol es obligatorio')
    : yup.string(),
  name: yup.string()
    .required('El nombre es obligatorio')
    .min(3, 'El nombre debe tener al menos 3 caracteres')
    .max(50, 'El nombre no puede exceder 50 caracteres'),
  email: yup.string()
    .required('El email es obligatorio')
    .email('Debe ser un email válido'),
  password: yup.string()
    .required('La contraseña es obligatoria')
    .min(6, 'La contraseña debe tener al menos 6 caracteres')
})

const { handleSubmit, errors, resetForm, defineField } = useForm({
  validationSchema,
  initialValues: {
    role: '',
    name: '',
    email: '',
    password: ''
  }
})

const [role] = defineField('role')
const [name] = defineField('name')
const [email] = defineField('email')
const [password] = defineField('password')


const markAsTouched = (field: keyof typeof touchedFields.value) => {
  touchedFields.value[field] = true
}


const getFieldState = (field: keyof typeof touchedFields.value) => {

  if (touchedFields.value[field] && errors.value[field]) {
    return false // Rojo
  }
  return null // Sin estado 
}

const enviarDatos = handleSubmit(async (values) => {

  touchedFields.value = {
    role: true,
    name: true,
    email: true,
    password: true
  }

  isSubmitting.value = true
  submitSuccess.value = false

  const dataToSubmit = {
    ...values,
    role: authStore.role === 'admin' ? values.role : 'student'
  }

  console.log('Datos a enviar:', dataToSubmit)

  emit('submit-register', dataToSubmit)

  await new Promise(resolve => setTimeout(resolve, 500))

  isSubmitting.value = false
  submitSuccess.value = true

  resetForm()
  
  touchedFields.value = {
    role: false,
    name: false,
    email: false,
    password: false
  }

  setTimeout(() => {
    submitSuccess.value = false
  }, 3000)
})
</script>

<template>
  <b-card header="Regístrate" header-bg-variant="primary" header-text-variant="white" class="shadow-sm">
    <b-form @submit.prevent="enviarDatos">

      <!-- Rol (solo admin) -->
      <b-form-group 
        v-if="authStore.role === 'admin'" 
        label="Rol:" 
        label-for="input-role" 
        class="mb-3"
        :invalid-feedback="errors.role"
        :state="getFieldState('role')"
      >
        <b-form-select 
          id="input-role" 
          v-model="role" 
          :options="roleOptions"
          :state="getFieldState('role')"
          @blur="markAsTouched('role')"
        ></b-form-select>
      </b-form-group>

      <!-- Nombre -->
      <b-form-group 
        label="Nombre:" 
        label-for="input-name" 
        class="mb-3"
        :invalid-feedback="errors.name"
        :state="getFieldState('name')"
      >
        <b-form-input 
          id="input-name" 
          v-model="name" 
          type="text" 
          placeholder="Clara"
          :state="getFieldState('name')"
          @blur="markAsTouched('name')"
        ></b-form-input>
      </b-form-group>

      <!-- Email -->
      <b-form-group 
        label="Correo Electrónico:" 
        label-for="input-email" 
        class="mb-3"
        :invalid-feedback="errors.email"
        :state="getFieldState('email')"
      >
        <b-form-input 
          id="input-email" 
          v-model="email" 
          type="email" 
          placeholder="ejemplo@correo.com"
          autocomplete="email"
          :state="getFieldState('email')"
          @blur="markAsTouched('email')"
        ></b-form-input>
      </b-form-group>

      <!-- Contraseña -->
      <b-form-group 
        label="Contraseña:" 
        label-for="input-password" 
        class="mb-3"
        :invalid-feedback="errors.password"
        :state="getFieldState('password')"
      >
        <b-input-group>
          <b-form-input 
            id="input-password" 
            v-model="password" 
            :type="showPassword ? 'text' : 'password'"
            placeholder="Ingresa tu contraseña"
            autocomplete="new-password"
            :state="getFieldState('password')"
            @blur="markAsTouched('password')"
          ></b-form-input>
          <b-input-group-text>
            <b-button variant="link" class="p-0 text-decoration-none" @click="showPassword = !showPassword">
              <span v-if="showPassword">Ocultar</span>
              <span v-else>Mostrar</span>
            </b-button>
          </b-input-group-text>
        </b-input-group>
      </b-form-group>

      <b-button 
        type="submit" 
        :variant="submitSuccess ? 'success' : 'primary'" 
        :disabled="isSubmitting"
        class="w-100 mt-3"
      >
        <span v-if="isSubmitting">
          <b-spinner small class="me-1"></b-spinner>
          Registrando...
        </span>
        <span v-else-if="submitSuccess">
          ¡Usuario registrado!
        </span>
        <span v-else>
          Registro
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