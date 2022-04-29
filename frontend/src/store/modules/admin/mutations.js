
export const SET_DEFAULT_DIRECTORS = (state, directors) => {
  state.defaultDirectors = directors
}

export const SET_GENRES = (state, genres) => {
  console.log({ mutation: genres })
  state.genres = genres
}

export default {
  SET_DEFAULT_DIRECTORS,
  SET_GENRES
}
