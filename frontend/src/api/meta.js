import axios from 'axios'

export const getConfigurationDetails = () => {
  return axios.get('/api/MetaData/Get')
}

export const searchPeople = (search = '') => {
  const searchEncoded = search.trim().length > 0 ? encodeURI(search.trim()) : ''
  return axios.get(`/api/MetaData/People?search=${searchEncoded}`)
}
