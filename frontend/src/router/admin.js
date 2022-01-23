import store from '@/store'

export const adminRoutes = [
  {
    path: '/admin',
    name: 'admin',
    component: () => import(/* webpackChunkName: "login" */ '../views/admin/AdminHome.vue'),
    beforeEnter (to, from, next) {
      const expires = store.getters['user/expires']

      const now = new Date().getTime()
      const expiresDate = new Date(expires).getTime()
      const isExpired = now > expiresDate

      if (store.getters['user/role'] !== 'Admin' || isExpired) {
        next({ name: 'login' })
        return
      }
      next()
    }
  }
]
