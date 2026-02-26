<script setup lang="ts">
import { useRouter } from 'vue-router'
const router = useRouter()
</script>

<template>
  <div class="empty-state text-center py-5">
    <div class="empty-icon mb-3">📭</div>
    <h5 class="text-muted">No tienes cursos matriculados</h5>
    <p class="text-muted small mb-4">Explora el catálogo y empieza a aprender hoy</p>
    <BButton variant="primary" pill @click="router.push('/')">
      Explorar cursos
    </BButton>
  </div>
</template>

<style scoped>
.empty-icon {
  font-size: 3.5rem;
}
</style>