import { getFilters } from '@/api/filters'

const storeFilters = ({ commit }) => {
  getFilters().then(response => {
    const { data } = response
    console.log(data)
  })
}

export default {
  storeFilters
}
