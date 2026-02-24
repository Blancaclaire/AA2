<script setup lang="ts">
import RegisterFormComponent from '@/components/publicLayout/RegisterFormComponent.vue';
import { useAuthStore } from '@/stores/AuthStore'

const authStore = useAuthStore()
const handleRegisterAction = (data: any) => {
  authStore.register(data)
}
</script>

<template>
  <h1>Gestión de Usuarios</h1>

  <RegisterFormComponent @submit-register="handleRegisterAction"></RegisterFormComponent>
</template>