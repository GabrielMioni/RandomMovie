
export const SET_FILTERS = (state, payload) => {
  const { countries, decades, directors, genres } = payload
  state.countries = countries
  state.decades = decades
  state.directors = directors
  state.genres = genres
}

export default {
  SET_FILTERS
}
