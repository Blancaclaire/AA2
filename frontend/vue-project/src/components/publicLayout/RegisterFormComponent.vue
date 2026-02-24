<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/AuthStore'

const authStore = useAuthStore()
const emit = defineEmits(['submit-register'])
const showPassword = ref(false)
const isSubmitting = ref(false)
const submitSuccess = ref(false)

const form = ref({
  role: '',
  name: '',
  email: '',
  password: ''
})

const roleOptions = ref([
  { value: '', text: 'Selecciona un rol' },
  { value: 'admin', text: 'Administrador' },
  { value: 'instructor', text: 'Instructor' }
])

const enviarDatos = async () => {
  isSubmitting.value = true
  submitSuccess.value = false

  const dataToSubmit = {
    ...form.value,
    role: authStore.role === 'admin' ? form.value.role : 'student'
  }

  console.log('Datos a enviar:', dataToSubmit)

  emit('submit-register', dataToSubmit)

  await new Promise(resolve => setTimeout(resolve, 500))

  isSubmitting.value = false
  submitSuccess.value = true

  // Limpiar formulario
  const userName = form.value.name
  form.value = {
    role: '',
    name: '',
    email: '',
    password: ''
  }

  // Resetear estado de éxito después de 3 segundos
  setTimeout(() => {
    submitSuccess.value = false
  }, 3000)
}
</script>

<template>
  <b-card header="Regístrate" header-bg-variant="primary" header-text-variant="white" class="shadow-sm">
    <b-form @submit.prevent="enviarDatos">

      <b-form-group v-if="authStore.role === 'admin'" label="Rol:" label-for="input-role" class="mb-3">
        <b-form-select id="input-role" v-model="form.role" :options="roleOptions" required></b-form-select>
      </b-form-group>

      <b-form-group label="Nombre:" label-for="input-name" class="mb-3">
        <b-form-input id="input-name" v-model="form.name" type="text" placeholder="Clara" required></b-form-input>
      </b-form-group>

      <b-form-group label="Correo Electrónico:" label-for="input-email" class="mb-3">
        <b-form-input id="input-email" v-model="form.email" type="email" placeholder="ejemplo@correo.com" required
          autocomplete="email"></b-form-input>
      </b-form-group>

      <b-form-group label="Contraseña:" label-for="input-password" class="mb-3">
        <b-input-group>
          <b-form-input id="input-password" v-model="form.password" :type="showPassword ? 'text' : 'password'"
            placeholder="Ingresa tu contraseña" required autocomplete="new-password"></b-form-input>
          <b-input-group-text>
            <b-button variant="link" class="p-0 text-decoration-none" @click="showPassword = !showPassword">
              <span v-if="showPassword">Ocultar</span>
              <span v-else>Mostrar</span>
            </b-button>
          </b-input-group-text>
        </b-input-group>
      </b-form-group>


      <b-button type="submit" :variant="submitSuccess ? 'success' : 'primary'" :disabled="isSubmitting"
        class="w-100 mt-3">
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
.shadow-sm{
background-color: var(--bs-tertiary-bg) !important;
}
</style>