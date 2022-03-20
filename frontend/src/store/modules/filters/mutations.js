
export const SET_CONFIGURATION_DETAILS = (state, payload) => {
  const {
    baseUrls: { baseUrl, secureBaseUrl },
    backDropSizes,
    logoSizes,
    posterSizes
  } = payload

  state.baseUrl = baseUrl
  state.secureBaseUrl = secureBaseUrl
  state.backdropSizes = backDropSizes
  state.logoSizes = logoSizes
  state.posterSizes = posterSizes
}

export default {
  SET_CONFIGURATION_DETAILS
}
