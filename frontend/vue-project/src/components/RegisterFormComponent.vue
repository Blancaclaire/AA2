<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits(['submit-register'])
const showPassword = ref(false)

const form = ref({
  role: '',
  name: '',
  email: '',
  password: ''
})

// Tip: Asegúrate de que los 'value' coincidan con lo que tu backend o NavComponent esperan (minúsculas)
const roleOptions = ref([
  { value: '', text: 'Selecciona un rol' },
  { value: 'admin', text: 'Administrador' },
  { value: 'instructor', text: 'Instructor' }, 
  { value: 'student', text: 'Alumno' }
])

const enviarDatos = () => {
  emit('submit-register', { ...form.value })
}
</script>

<template>
  <b-card header="Regístrate" header-bg-variant="primary" header-text-variant="white" class="shadow-sm">
    <b-form @submit.prevent="enviarDatos">

      <b-form-group label="Rol:" label-for="input-role" class="mb-3">
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

      <b-button type="submit" variant="primary" class="w-100 mt-3">
        Registro
      </b-button>
    </b-form>
  </b-card>
</template>