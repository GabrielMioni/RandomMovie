import Vue from 'vue'
import Vuex from 'vuex'
import user from './modules/user'
import meta from './modules/meta'
import filters from './modules/filters'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    user,
    meta,
    filters
  }
})
