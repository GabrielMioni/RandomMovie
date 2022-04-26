import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  defaultDirectors: []
}

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
}
