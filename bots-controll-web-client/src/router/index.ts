import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import BotsPage from '../modules/bots/BotsPage.vue'
import Login from '@/modules/login/components/Login.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: BotsPage
  },
  {
    path: '/about',
    name: 'about',
    component: () => import('../views/AboutView.vue')
  },
  {
    path: '/login',
    name: 'login',
    component: Login
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
