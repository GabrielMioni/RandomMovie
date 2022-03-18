import { getConfigurationDetails } from '@/api/meta'

const setConfigurationDetails = ({ commit }) => {
  const config = getConfigurationDetails()

  config.then(response => {
    const { data } = response
    commit('SET_CONFIGURATION_DETAILS', data)
  })
}

export default {
  setConfigurationDetails
}
