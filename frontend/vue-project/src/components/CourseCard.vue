<script setup lang="ts">
defineProps<{ course: any }>()

const levelColor = (level: string) => {
  switch (level) {
    case 'Beginner': return 'success'
    case 'Intermediate': return 'warning'
    case 'Advanced': return 'danger'
    default: return 'secondary'
  }
}
</script>

<template>
  <b-card class="mb-4 h-100 border-0 course-card" no-body>
    <div class="card-header-colored">
      <div class="d-flex justify-content-between align-items-start p-3">
        <b-badge variant="light" class="text-dark">{{ course.categoryName }}</b-badge>
        <b-badge :variant="levelColor(course.level)">{{ course.level }}</b-badge>
      </div>
      <div class="text-center pb-3">
        <span class="display-6">📚</span>
      </div>
    </div>

    <b-card-body class="d-flex flex-column">
      <b-card-title class="fs-5 mb-1">{{ course.title }}</b-card-title>
      <p class="text-muted small mb-3">por {{ course.instructor }}</p>

      <p class="text-secondary small flex-grow-1" style="min-height: 40px;">
        {{ course.description?.length > 100 ? course.description.substring(0, 100) + '...' : course.description }}
      </p>

      <div class="border-top pt-3 mt-auto">
        <div class="d-flex justify-content-between align-items-center mb-2">
          <span class="small">
            <span class="text-warning">⭐</span>
            {{ course.rating > 0 ? course.rating.toFixed(1) : '—' }}
          </span>
          <span class="small text-muted">⏱ {{ course.durationHours }}h</span>
          <span class="small text-muted">👥 {{ course.enrollmentCount }}</span>
        </div>
        <div class="text-end">
          <span class="fs-4 fw-bold card-price">{{ course.price }}€</span>
        </div>
      </div>
    </b-card-body>
  </b-card>
</template>

<style scoped>
.course-card {
  transition: transform 0.2s, box-shadow 0.2s;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.course-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 24px rgba(34, 34, 196, 0.15);
}

.card-header-colored {
  min-height: 120px;
  background: linear-gradient(135deg, #eef1ff 0%, #dce1ff 100%);
  border-bottom: 1px solid #c5cdff;
}

.card-price {
  color: #2222c4;
}
</style>