import actions from './actions'
import mutations from './mutations'

const state = {
  user: {
    email: null,
    expires: null,
    role: null,
    userName: null
  }
}

export default {
  namespaced: true,
  state,
  actions,
  mutations
}
