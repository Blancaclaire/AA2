<script setup lang="ts">
import RegisterFormComponent from '@/components/publicLayout/RegisterFormComponent.vue';
import router from '@/router';
import { useAuthStore } from '@/stores/AuthStore'
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()
const authStore = useAuthStore()

const handleRegisterAction = async (data: any) => {
  if (authStore.role === 'admin') {
    await userStore.postUser(data)
  } else {
    const success = await authStore.register(data)
    if (success) {
      await router.push('/')
    }
  }
}


</script>

<template>
  <h1>Gestión de Usuarios</h1>

  <RegisterFormComponent @submit-register="handleRegisterAction"></RegisterFormComponent>
</template>