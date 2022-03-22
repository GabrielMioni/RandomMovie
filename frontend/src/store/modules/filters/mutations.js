
export const SET_FILTERS = (state, payload) => {
  const { countries, decades, directors, genres } = payload
  state.countries = countries
  state.decades = decades
  state.directors = directors
  state.genres = genres
}

const SET_SELECTED_FILTERS = (state, payload) => {
  const { type, ids } = payload

  switch (type) {
    case 'countries':
      state.selectedCountries = ids
      break
    case 'decades':
      state.selectedDecades = ids
      break
    case 'directors':
      state.selectedDirectors = ids
      break
    case 'genres':
      state.selectedGenres = ids
      break
    default:
      break
  }
}

export default {
  SET_FILTERS,
  SET_SELECTED_FILTERS
}
