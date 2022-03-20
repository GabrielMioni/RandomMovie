import { getFilters } from '@/api/filters'

const setFilters = ({ commit }) => {
  getFilters().then(response => {
    const filters = response.data
    commit('SET_FILTERS', filters)
  })
}

export default {
  setFilters
}
