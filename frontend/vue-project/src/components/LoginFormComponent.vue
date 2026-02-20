<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits(['submit-login'])

const showPassword = ref(false)

//Datos del formulario
const form = ref({
  email: '',
  password: ''
})

const enviarDatos = () => {
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
    <b-form @submit.prevent="enviarDatos">
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

    <b-button @click="enviarDatos" variant="primary" class="w-100 mt-3">
      Inicio Sesión
    </b-button>
    </b-form>
  </b-card>
</template>