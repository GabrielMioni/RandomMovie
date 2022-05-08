import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  countries: [],
  defaultDirectors: [],
  defaultCredits: [],
  genres: []
}

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
}
