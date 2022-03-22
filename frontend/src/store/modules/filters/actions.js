import { getFilters } from '@/api/filters'

const setFilters = ({ commit }) => {
  getFilters().then(response => {
    const filters = response.data
    commit('SET_FILTERS', filters)
  })
}

const setSelectedFilters = ({ commit }, payload) => {
  commit('SET_SELECTED_FILTERS', payload)
}

export default {
  setFilters,
  setSelectedFilters
}
