<script setup lang="ts">
import { ref } from 'vue'

// Definimos el evento que el hijo enviará al padre
const emit = defineEmits<{
  (e: 'submit-login', payload: any): void
}>()

const showPassword = ref(false)
const form = ref({
  email: '',
  password: ''
})

const handleSubmit = () => {
  // Enviamos los datos al componente padre
  emit('submit-login', { ...form.value })
}
</script>

<template>
  <b-card 
    header="Acceso al Sistema" 
    header-bg-variant="primary" 
    header-text-variant="white"
    class="shadow-sm"
  >
    <b-form @submit.prevent="handleSubmit">
      <b-form-group label="Correo Electrónico:" label-for="input-email" class="mb-3">
        <b-form-input
          id="input-email"
          v-model="form.email"
          type="email"
          placeholder="ejemplo@correo.com"
          required
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Contraseña:" label-for="input-password" class="mb-3">
        <b-input-group>
          <b-form-input
            id="input-password"
            v-model="form.password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="Ingresa tu contraseña"
            required
          ></b-form-input>
          <b-input-group-text>
            <b-button 
              variant="link" 
              class="p-0 text-decoration-none" 
              @click="showPassword = !showPassword"
            >
              <span v-if="showPassword">Ocultar</span>
              <span v-else>Mostrar</span>
            </b-button>
          </b-input-group-text>
        </b-input-group>
      </b-form-group>

      <b-button type="submit" variant="primary" class="w-100">
        Iniciar Sesión
      </b-button>
    </b-form>
  </b-card>
</template>