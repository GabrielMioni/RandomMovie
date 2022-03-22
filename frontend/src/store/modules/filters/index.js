import actions from './actions'
import mutations from './mutations'
import getters from './getters'

const state = {
  countries: [],
  decades: [],
  directors: [],
  genres: [],
  selectedCountries: [],
  selectedDecades: [],
  selectedDirectors: [],
  selectedGenres: []
}

export default {
  namespaced: true,
  state,
  actions,
  mutations,
  getters
}
