import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    // --- Public ---
  {
    path: '/',
    name: 'home',
    meta: { layout: 'public' },
    component: () => import('../views/HomeView.vue'),
  },

  // --- Blank ---
  {
    path: '/login',
    name: 'login',
    meta: { layout: 'blank' },
    component: () => import('../views/LoginView.vue'),
  },
  {
    path: '/register',
    name: 'register',
    meta: { layout: 'blank' },
    component: () => import('../views/RegisterView.vue'),
  },

    // --- Admin (header admin + footer admin) ---
    {
      path: '/admin/dashboard',
      name: 'admin-dashboard',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/admin/DashboardView.vue'),
    },
    {
      path: '/admin/courses',
      name: 'admin-courses',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/admin/CoursesView.vue'),
    },
    {
      path: '/admin/categories',
      name: 'admin-categories',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/admin/CategoriesView.vue'),
    },
    {
      path: '/admin/users',
      name: 'admin-users',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/admin/UsersView.vue'),
    },
  ],
})

export default router