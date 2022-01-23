import axios from 'axios'
const localStorageUserKey = 'user'

const setUserData = ({ commit }, payload) => {
  const { token, ...userData } = payload
  axios.defaults.headers.common.Authorization = `Bearer ${token}`
  localStorage.setItem(localStorageUserKey, JSON.stringify(payload))
  commit('SET_USER_DATA', userData)
}

export default {
  setUserData
}
