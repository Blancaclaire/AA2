import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/AuthStore'

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
    {
      path: '/detail/:id',
      name: 'detail',
      meta: { layout: 'public' },
      component: () => import('../views/CourseDetailView.vue'),
    },
    {
      path: '/myCourses',
      name: 'myCourses',
      meta: { layout: 'public', requiresAuth: true},
      component: () => import('../views/MyCoursesView.vue'),
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

    // --- Admin ---
    {
      path: '/admin/dashboard',
      name: 'admin-dashboard',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/Admin/DashboardView.vue'),
    },
    {
      path: '/admin/courses',
      name: 'admin-courses',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/Admin/CoursesView.vue'),
    },
    {
      path: '/admin/categories',
      name: 'admin-categories',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/Admin/CategoriesView.vue'),
    },
    {
      path: '/admin/users',
      name: 'admin-users',
      meta: { layout: 'admin', requiresAuth: true },
      component: () => import('../views/Admin/UsersView.vue'),
    },
  ],
})


router.beforeEach((to, from, next) => {
  const auth = useAuthStore()

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    next({ name: 'login' })
  } else {
    next()
  }
})

export default router