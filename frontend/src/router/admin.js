export const adminRoutes = [
  {
    path: '/admin',
    name: 'admin',
    component: () => import(/* webpackChunkName: "login" */ '../views/admin/AdminHome.vue'),
    beforeEnter (to, from, next) {
      const { expires, role } = JSON.parse(localStorage.getItem('user'))

      const now = new Date().getTime()
      const expiresDate = new Date(expires).getTime()
      const isExpired = now > expiresDate

      if (role !== 'Admin' || isExpired) {
        next({ name: 'login' })
        return
      }
      next()
    }
  }
]
