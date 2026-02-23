<script setup lang="ts">
import { useRouter } from 'vue-router'

const props = defineProps<{ course: any }>()
const router = useRouter()

const levelColor = (level: string) => {
  switch (level) {
    case 'Beginner': return { bg: '#d1fae5', text: '#065f46', label: 'Principiante' }
    case 'Intermediate': return { bg: '#fef3c7', text: '#92400e', label: 'Intermedio' }
    case 'Advanced': return { bg: '#fee2e2', text: '#991b1b', label: 'Avanzado' }
    default: return { bg: '#f3f4f6', text: '#374151', label: level }
  }
}

</script>

<template>
  <div class="hero">
    <div class="hero-inner">
      <button class="btn-back" @click="router.back()">← Volver</button>

      <div class="hero-meta">
        <span class="category-tag">{{ course.categoryName }}</span>
        <span
          class="level-tag"
          :style="{ background: levelColor(course.level).bg, color: levelColor(course.level).text }"
        >
          {{ levelColor(course.level).label }}
        </span>
      </div>

      <h1 class="hero-title">{{ course.title }}</h1>
      <p class="hero-instructor">por <strong>{{ course.instructor }}</strong></p>

      <div class="hero-stats">
        <div class="stat">
          <span class="stat-icon">⭐</span>
          <span class="stat-value">{{ course.rating > 0 ? course.rating.toFixed(1) : '—' }}</span>
          <span class="stat-label">Valoración</span>
        </div>
        <div class="stat-divider"></div>
        <div class="stat">
          <span class="stat-icon">⏱</span>
          <span class="stat-value">{{ course.durationHours }}h</span>
          <span class="stat-label">Duración</span>
        </div>
        <div class="stat-divider"></div>
        <div class="stat">
          <span class="stat-icon">👥</span>
          <span class="stat-value">{{ course.enrollmentCount }}</span>
          <span class="stat-label">Alumnos</span>
        </div>
      </div>
    </div>

    <div class="hero-deco">📚</div>
  </div>
</template>

<style scoped>
.hero {
  background: linear-gradient(135deg, #1a1a6e 0%, #2222c4 60%, #3b3bff 100%);
  color: white;
  padding: 3rem 2rem 4rem;
  position: relative;
  overflow: hidden;
}

.hero::before {
  content: '';
  position: absolute;
  inset: 0;
  background-image: radial-gradient(rgba(255,255,255,0.06) 1px, transparent 1px);
  background-size: 28px 28px;
}

.hero-inner {
  max-width: 860px;
  margin: 0 auto;
  position: relative;
  z-index: 1;
}

.hero-deco {
  position: absolute;
  right: 6%;
  top: 50%;
  transform: translateY(-50%);
  font-size: 8rem;
  opacity: 0.12;
  z-index: 0;
  pointer-events: none;
}

.btn-back {
  background: rgba(255,255,255,0.15);
  border: 1px solid rgba(255,255,255,0.3);
  color: white;
  border-radius: 8px;
  padding: 0.4rem 1rem;
  font-size: 0.85rem;
  cursor: pointer;
  margin-bottom: 1.5rem;
  transition: background 0.2s;
}

.btn-back:hover {
  background: rgba(255,255,255,0.25);
}

.hero-meta {
  display: flex;
  gap: 0.75rem;
  margin-bottom: 1rem;
}

.category-tag {
  background: rgba(255,255,255,0.2);
  border: 1px solid rgba(255,255,255,0.3);
  border-radius: 20px;
  padding: 0.25rem 0.85rem;
  font-size: 0.8rem;
  font-weight: 600;
}

.level-tag {
  border-radius: 20px;
  padding: 0.25rem 0.85rem;
  font-size: 0.8rem;
  font-weight: 700;
}

.hero-title {
  font-size: clamp(1.6rem, 4vw, 2.6rem);
  font-weight: 800;
  line-height: 1.2;
  margin-bottom: 0.5rem;
  letter-spacing: -0.5px;
}

.hero-instructor {
  font-size: 0.95rem;
  opacity: 0.85;
  margin-bottom: 2rem;
}

.hero-stats {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.stat {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.15rem;
}

.stat-icon { font-size: 1.1rem; }
.stat-value { font-size: 1.2rem; font-weight: 700; }
.stat-label { font-size: 0.72rem; opacity: 0.75; text-transform: uppercase; letter-spacing: 0.5px; }

.stat-divider {
  width: 1px;
  height: 40px;
  background: rgba(255,255,255,0.25);
}
</style>