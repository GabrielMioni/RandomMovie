import axios from 'axios'

export const getFilters = () =>
  axios.get('/api/Filters/Get')

export const searchDirectors = (search = '') => {
  const searchEncoded = search.trim().length > 0 ? encodeURI(search.trim()) : ''
  return axios.get(`/api/Filters/Directors?search=${searchEncoded}`)
}

export const getGenres = () =>
  axios.get('/api/Filters/Genres')
