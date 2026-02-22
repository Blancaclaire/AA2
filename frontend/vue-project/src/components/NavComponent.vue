<script setup lang="ts">
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <BNavbar v-b-color-mode="'dark'" toggleable="lg" variant="primary">
    <BNavbarBrand :href="'/home'">CursosApp</BNavbarBrand>
    <BNavbarToggle target="nav-collapse" />
    <BCollapse id="nav-collapse" is-nav>
      <BNavbarNav>
        <BNavItem href="/home" :active="route.path === '/home'">Inicio</BNavItem>
        <BNavItem href="/courses" :active="route.path === '/courses'">Cursos</BNavItem>
      </BNavbarNav>
      <BNavbarNav class="ms-auto">
        <BNavItem href="/login">Login</BNavItem>
      </BNavbarNav>
    </BCollapse>
  </BNavbar>
</template>