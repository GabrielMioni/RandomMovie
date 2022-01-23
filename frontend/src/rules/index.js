export const isRequired = (message) => (v) =>
  !!v || (message || 'This field is required')
