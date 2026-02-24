<script setup lang="ts">
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/AuthStore';
import { useRouter } from 'vue-router'
import { useThemeStore } from '@/stores/ThemeStore'

const themeStore = useThemeStore()
const route = useRoute()
const authStore = useAuthStore();
const router = useRouter()


const handleLogout = () => {
  authStore.logout()
  window.location.href = '/'
}

</script>

<template>
  <BNavbar v-b-color-mode="'dark'" toggleable="lg" variant="primary">
    <BNavbarBrand to="/">CursosApp</BNavbarBrand>
    <BNavbarToggle target="nav-collapse" />
    <BCollapse id="nav-collapse" is-nav>
      <BNavbarNav>
        <BNavItem to="/" :active="route.path === '/'">Inicio</BNavItem>
      </BNavbarNav>

      <BNavbarNav class="ms-auto">
        <!-- Sin login -->
        <template v-if="!authStore.isAuthenticated">
          <BNavItem to="/login" :active="route.path === '/login'">Login</BNavItem>
          <BNavItem to="/register" :active="route.path === '/register'">Registro</BNavItem>
        </template>

        <!-- Con login -->
        <template v-else>
          <BNavItem to="/myCourses" :active="route.path === '/myCourses'">Mis Cursos</BNavItem>
          <BNavItem v-if="authStore.role === 'admin' || authStore.role === 'instructor'" to="/admin/dashboard">Admin
          </BNavItem>
          <BNavItem @click="themeStore.toggleTheme">
            {{ themeStore.theme === 'light' ? 'Modo Oscuro' : 'Modo Claro' }}
          </BNavItem>
          <BNavItem @click="handleLogout">Cerrar Sesión</BNavItem>
        </template>
      </BNavbarNav>
    </BCollapse>
  </BNavbar>
</template>
