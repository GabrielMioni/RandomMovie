// import AdminMovies from '../views/admin/AdminMovies'

export const adminRoutes = [
  {
    path: '/admin',
    name: 'admin',
    component: () => import(/* webpackChunkName: "admin" */ '../views/admin/AdminHome.vue'),
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
    },
    redirect: to => {
      return {
        path: '/admin/movies'
      }
    },
    children: [
      {
        path: 'movies',
        name: 'adminMovies',
        component: () => import(/* webpackChunkName: "adminMovies" */ '../views/admin/AdminMovies.vue')
      },
      {
        path: 'filters',
        name: 'adminFilters',
        component: () => import(/* webpackChunkName: "adminFilters" */ '../views/admin/AdminFilters.vue')
      }
    ]
  }
]
