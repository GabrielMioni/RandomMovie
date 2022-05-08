import { searchDirectors, getGenres, getCountries } from '@/api/filters'
import { searchPeople } from '@/api/meta'

const setAllFiltersData = async ({ dispatch }) => {
  await Promise.all([
    dispatch('setDefaultDirectors'),
    dispatch('setDefaultCredits'),
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

const setDefaultCredits = async ({ commit, state }) => {
  if (state.defaultCredits.length > 0) {
    return
  }

  searchPeople().then(response => {
    const credits = response.data
    commit('SET_DEFAULT_CREDITS', credits)
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
  setDefaultCredits,
  setGenres,
  setCountries
}
