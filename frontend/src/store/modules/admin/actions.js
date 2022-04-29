import { searchDirectors, getGenres } from '@/api/filters'

const setDefaultDirectors = ({ commit }) => {
  searchDirectors().then(response => {
    const directors = response.data
    commit('SET_DEFAULT_DIRECTORS', directors)
  })
}

const setGenres = ({ commit }) => {
  getGenres().then(response => {
    const genres = response.data
    commit('SET_GENRES', genres)
  })
}

export default {
  setDefaultDirectors,
  setGenres
}
