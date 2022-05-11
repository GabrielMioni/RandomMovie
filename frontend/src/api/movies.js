import axios from 'axios'

export const getRandomMovie = (params = {}) =>
  axios.post('/api/Movies/GetRandomMovie', params)

export const getMovieById = (movieId) =>
  axios.get(`/api/Movies/${movieId}`)

export const getMovies = (params = {}) =>
  axios.post('/api/Movies/GetMovies', params)

export const editMovie = (params = {}) =>
  axios.post('/api/EditMovie', params)
