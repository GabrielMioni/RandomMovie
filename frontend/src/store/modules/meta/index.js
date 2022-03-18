import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  baseUrl: null,
  secureBaseUrl: null,
  backdropSizes: [],
  logoSizes: [],
  posterSizes: []
}

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
}
