import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import { adminRoutes } from '@/router/admin'

Vue.use(VueRouter)

const routes = [
  ...adminRoutes,
  {
    path: '/',
    name: 'root',
    redirect: to => {
      return {
        path: '/movie:movieId?'
      }
    }
  },
  {
    path: '/movie/:movieId?',
    name: 'home',
    component: Home,
    props: route => ({ movieId: route.params.movieId })
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/login',
    name: 'login',
    component: () => import(/* webpackChunkName: "login" */ '../views/Login.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  // base: process.env.BASE_URL,
  routes
})

export default router
