<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/AuthStore'

const authStore = useAuthStore()
const route = useRoute()
const router = useRouter()

const handleLogout = () => {
  authStore.logout()
  setTimeout(() => {
    router.push('/login')
  }, 100)
}
</script>

<template>
  <BNavbar v-b-color-mode="'dark'" toggleable="lg" variant="dark">
    <BNavbarBrand to="/admin/dashboard">CursosApp Admin</BNavbarBrand>
    <BNavbarToggle target="admin-nav" />
    <BCollapse id="admin-nav" is-nav>
      <BNavbarNav>
        <BNavItem to="/admin/dashboard" :active="route.path === '/admin/dashboard'">Dashboard</BNavItem>
        <BNavItem to="/admin/courses" :active="route.path === '/admin/courses'">Cursos</BNavItem>
        <BNavItem to="/admin/categories" :active="route.path === '/admin/categories'">Categorías</BNavItem>
        <BNavItem to="/admin/users" :active="route.path === '/admin/users'">Usuarios</BNavItem>
      </BNavbarNav>
      <BNavbarNav class="ms-auto">
        <BNavItem @click="handleLogout">Cerrar Sesión</BNavItem>
      </BNavbarNav>
    </BCollapse>
  </BNavbar>
</template>