import axios from 'axios'

export const getFilters = () =>
  axios.get('/api/Filters/Get')
