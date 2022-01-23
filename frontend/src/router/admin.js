import store from '@/store'

export const adminRoutes = [
  {
    path: '/admin',
    name: 'admin',
    component: () => import(/* webpackChunkName: "login" */ '../views/admin/AdminHome.vue'),
    beforeEnter (to, from, next) {
      if (store.getters['user/role'] !== 'Admin') {
        next({ name: 'home' })
        return
      }
      next()
    }
  }
]
