
export const SET_DEFAULT_DIRECTORS = (state, directors) => {
  state.defaultDirectors = directors
}

export const SET_GENRES = (state, genres) => {
  state.genres = genres
}

export const SET_COUNTRIES = (state, countries) => {
  state.countries = countries
}

export default {
  SET_COUNTRIES,
  SET_DEFAULT_DIRECTORS,
  SET_GENRES
}
