<template>
  <v-autocomplete
    v-model="selectedCountryLocal"
    :items="countries"
    item-text="name"
    :search-input.sync="countrySearch"
    return-object
    outlined
    label="Countries">
  </v-autocomplete>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'CountrySelect',
  data () {
    return {
      countrySearch: ''
    }
  },
  props: {
    value: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters('admin', ['countries']),
    selectedCountryLocal: {
      get () {
        return this.value
      },
      set (value) {
        this.$emit('input', value)
      }
    }
  }
}
</script>

<style scoped>

</style>
