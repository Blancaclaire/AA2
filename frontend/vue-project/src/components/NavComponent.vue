<script setup lang="ts">
import { useRoute } from 'vue-router'
import { useAuthStore  } from '@/stores/AuthStore';
import router from '@/router';



const route = useRoute()
const authStore = useAuthStore();

const handleLogout = ()=>{
  authStore.logout()
  router.push('/login')
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
          <BNavItem to="/my-courses" :active="route.path === '/my-courses'">Mis Cursos</BNavItem>
          <BNavItem 
            v-if="authStore.role === 'admin' || authStore.role === 'instructor'" 
            to="/admin/dashboard"
          >Admin</BNavItem>
          <BNavItem @click="handleLogout">Cerrar Sesión</BNavItem>
        </template>
      </BNavbarNav>
    </BCollapse>
  </BNavbar>
</template>