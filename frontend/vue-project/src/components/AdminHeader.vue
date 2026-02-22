<script setup lang="ts">
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <BNavbar v-b-color-mode="'dark'" toggleable="lg" variant="dark">
    <BNavbarBrand href="/admin/dashboard">CursosApp Admin</BNavbarBrand>
    <BNavbarToggle target="admin-nav" />
    <BCollapse id="admin-nav" is-nav>
      <BNavbarNav>
        <BNavItem href="/admin/dashboard" :active="route.path === '/admin/dashboard'">Dashboard</BNavItem>
        <BNavItem href="/admin/courses" :active="route.path === '/admin/courses'">Cursos</BNavItem>
        <BNavItem href="/admin/categories" :active="route.path === '/admin/categories'">Categorías</BNavItem>
        <BNavItem href="/admin/users" :active="route.path === '/admin/users'">Usuarios</BNavItem>
      </BNavbarNav>
      <BNavbarNav class="ms-auto">
        <BNavItem href="/">Cerrar Sesión</BNavItem>
      </BNavbarNav>
    </BCollapse>
  </BNavbar>
</template>