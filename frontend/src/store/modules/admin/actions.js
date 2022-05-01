import { searchDirectors, getGenres, getCountries } from '@/api/filters'

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

const setCountries = ({ commit }) => {
  getCountries().then(response => {
    const countries = response.data
    commit('SET_COUNTRIES', countries)
  })
}

export default {
  setDefaultDirectors,
  setGenres,
  setCountries
}
