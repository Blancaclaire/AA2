<script setup lang="ts">
import { RouterView, useRoute } from 'vue-router'
import { useUIStore } from '@/stores/UIStore'
import NavComponent from '@/components/publicLayout/NavComponent.vue'
import FooterComponent from '@/components/publicLayout/FooterComponent.vue'
import AdminHeader from '@/components/adminLayout/AdminHeader.vue'
import AdminFooter from '@/components/adminLayout/AdminFooter.vue'

const route = useRoute()
const uiStore = useUIStore()
</script>

<template>
  <div :class="uiStore.theme" class="d-flex flex-column min-vh-100">

    <template v-if="route.meta.layout === 'blank'">
      <router-view />
    </template>

    <template v-else-if="route.meta.layout === 'public'">
      <NavComponent />
      <b-container class="mt-4 flex-grow-1">
        <router-view />
      </b-container>
      <FooterComponent />
    </template>

    <template v-else-if="route.meta.layout === 'admin'">
      <AdminHeader />
      <b-container fluid class="mt-4 flex-grow-1">
        <router-view />
      </b-container>
      <AdminFooter />
    </template>

  </div>
</template>

