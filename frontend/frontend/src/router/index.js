
import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Register from '../views/Register.vue'
import Profile from '../views/Profile.vue'
import Courses from '../views/Courses.vue'
import store from '../store'

const routes = [
  { path: '/', redirect: '/profile' },
  { path: '/login', component: Login, meta: { guest: true } },
  { path: '/register', component: Register, meta: { guest: true } },
  { path: '/profile', component: Profile, meta: { requiresAuth: true } },
  { path: '/courses', component: Courses, meta: { requiresAuth: true } }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const isLoggedIn = store.getters.isLoggedIn
  if (to.meta.requiresAuth && !isLoggedIn) return next('/login')
  if (to.meta.guest && isLoggedIn) return next('/courses')
  next()
})

export default router
