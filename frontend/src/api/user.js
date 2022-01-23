import axios from 'axios'

export const login = (userName, password) =>
  axios.post('/api/User/Login', { userName, password })
