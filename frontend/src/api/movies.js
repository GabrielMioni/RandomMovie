import axios from 'axios'

export const getRandomMovie = (params = {}) =>
  axios.post('/api/Movies/GetRandomMovie', params)
