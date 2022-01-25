import axios from 'axios'

export const values = () =>
  axios.get('/api/Values')
