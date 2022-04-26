import { searchDirectors } from '@/api/filters'

const setDefaultDirectors = ({ commit }) => {
  searchDirectors().then(response => {
    const directors = response.data
    commit('SET_DEFAULT_DIRECTORS', directors)
  })
}

export default {
  setDefaultDirectors
}
