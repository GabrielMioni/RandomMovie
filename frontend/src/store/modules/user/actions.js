import axios from 'axios'
const localStorageUserKey = 'user'

const setUserData = ({ commit }, payload) => {
  const { token, ...userData } = payload
  axios.defaults.headers.common.Authorization = `Bearer ${token}`
  localStorage.setItem(localStorageUserKey, JSON.stringify(payload))
  commit('SET_USER_DATA', userData)
}

const removeUserData = ({ commit }) => {
  delete axios.defaults.headers.common.Authorization
  localStorage.removeItem(localStorageUserKey)
  commit('SET_USER_DATA', {
    email: null,
    expires: null,
    role: null,
    token: null,
    userName: null
  })
}

export default {
  setUserData,
  removeUserData
}
