import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  countries: [],
  decades: [],
  directors: [],
  genres: []
}

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
}
