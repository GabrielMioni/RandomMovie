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
import { mapActions, mapGetters } from 'vuex'

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
  mounted () {
    if (this.countries.length <= 0) {
      this.setCountries()
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
  },
  methods: {
    ...mapActions('admin', ['setCountries'])
  }
}
</script>

<style scoped>

</style>
