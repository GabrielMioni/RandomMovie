import { searchDirectors, getGenres, getCountries } from '@/api/filters'

const setAllFiltersData = async ({ dispatch }) => {
  await Promise.all([
    dispatch('setDefaultDirectors'),
    dispatch('setGenres'),
    dispatch('setCountries')
  ])
}

const setDefaultDirectors = async ({ commit, state }) => {
  if (state.defaultDirectors.length > 0) {
    return
  }

  searchDirectors().then(response => {
    const directors = response.data
    commit('SET_DEFAULT_DIRECTORS', directors)
  })
}

const setGenres = async ({ commit, state }) => {
  if (state.genres.length > 0) {
    return
  }
  getGenres().then(response => {
    const genres = response.data
    commit('SET_GENRES', genres)
  })
}

const setCountries = async ({ commit, state }) => {
  if (state.countries.length > 0) {
    return
  }
  getCountries().then(response => {
    const countries = response.data
    commit('SET_COUNTRIES', countries)
  })
}

export default {
  setAllFiltersData,
  setDefaultDirectors,
  setGenres,
  setCountries
}
