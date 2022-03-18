import axios from 'axios'

export const getConfigurationDetails = () => {
  return axios.get('/api/MetaData/Get')
}
