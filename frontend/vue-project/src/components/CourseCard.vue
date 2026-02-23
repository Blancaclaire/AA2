<script setup lang="ts">
defineProps<{ course: any }>()
</script>

<template>
  <b-card class="mb-4 shadow-sm h-100 border-0">
    <b-card-img
      v-if="course.imageUrl"
      :src="course.imageUrl"
      top
      style="height: 180px; object-fit: cover;"
    />
    <div v-else class="bg-primary bg-opacity-10 d-flex align-items-center justify-content-center" style="height: 180px;">
      <span class="text-primary fs-1">📚</span>
    </div>

    <b-card-body>
      <div class="d-flex justify-content-between align-items-start mb-2">
        <b-badge variant="info">{{ course.categoryName }}</b-badge>
        <b-badge variant="secondary">{{ course.level }}</b-badge>
      </div>

      <b-card-title class="fs-5">{{ course.title }}</b-card-title>

      <b-card-text class="text-muted" style="min-height: 48px;">
        {{ course.description?.length > 80 ? course.description.substring(0, 80) + '...' : course.description }}
      </b-card-text>

      <div class="text-muted small mb-2">
        <span>👨‍🏫 {{ course.instructor }}</span>
        <span class="ms-3">⏱ {{ course.durationHours }}h</span>
      </div>

      <div class="d-flex justify-content-between align-items-center mb-3">
        <span class="text-warning">
          ⭐ {{ course.rating > 0 ? course.rating.toFixed(1) : 'Sin valorar' }}
        </span>
        <span class="text-muted small">{{ course.enrollmentCount }} estudiantes</span>
      </div>

      <div class="d-flex justify-content-between align-items-center">
        <span class="fs-4 fw-bold text-primary">{{ course.price }}€</span>
        <b-button variant="primary" size="sm">Ver detalles</b-button>
      </div>
    </b-card-body>
  </b-card>
</template>