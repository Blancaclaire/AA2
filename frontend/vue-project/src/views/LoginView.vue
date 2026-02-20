<script setup lang="ts">
import LoginForm from '@/components/LoginFormComponent.vue'

const handleLoginAction = (data: any) => {
  console.log('Login data:', data)
}
</script>

<template>
  <b-container fluid class="p-0 overflow-hidden">
    <b-row class="g-0 vh-100">
      
      <b-col md="6" class="d-none d-md-flex info-section flex-column justify-content-center p-5 text-white">
        <div class="px-lg-5">
          <h1 class="display-3 fw-bold mb-4">CursosApp</h1>
          <p class="lead fs-4">La plataforma definitiva para la gestión de contenidos educativos.</p>
          <div class="mt-5">
            <div class="d-flex align-items-center mb-3">
              <div class="feature-dot me-3"></div>
              <span>Acceso total a tus cursos</span>
            </div>
            <div class="d-flex align-items-center mb-3">
              <div class="feature-dot me-3"></div>
              <span>Panel de administración intuitivo</span>
            </div>
          </div>
        </div>
      </b-col>

      <b-col cols="12" md="6" class="bg-white d-flex align-items-center justify-content-center p-4 p-md-5">
        <div style="width: 100%; max-width: 500px;">
          <div class="mb-5 text-center text-md-start">
            <h2 class="display-5 fw-bold text-dark">Bienvenido</h2>
            <p class="text-muted fs-5">Introduce tus credenciales para acceder</p>
          </div>
          
          <LoginForm @submit-login="handleLoginAction" />
          
          <p class="mt-5 text-center text-muted small">
            &copy; 2024 CursosApp S.L. Todos los derechos reservados.
          </p>
        </div>
      </b-col>

    </b-row>
  </b-container>
</template>

<style scoped>

.info-section {
  background: linear-gradient(135deg, #2222c4 0%, #1a1a2e 100%);
  position: relative;
}

.feature-dot {
  width: 12px;
  height: 12px;
  background-color: #00d4ff;
  border-radius: 50%;
}

.info-section::before {
  content: "";
  position: absolute;
  top: 0; left: 0; right: 0; bottom: 0;
  opacity: 0.1;
  background-image: radial-gradient(#ffffff 1px, transparent 1px);
  background-size: 30px 30px;
}
</style>