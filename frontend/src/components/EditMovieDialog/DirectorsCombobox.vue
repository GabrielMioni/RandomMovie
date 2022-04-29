<template>
  <combobox-server-side
    v-model="directorSearch"
    :items="displayDirectors"
    item-text="name"
    label="Directors"
    :selected-items="selectedDirectors"
    @update:selected-items="updateSelectedItems">
  </combobox-server-side>
</template>

<script>
import { searchDirectors } from '@/api/filters'
import { mapActions, mapGetters } from 'vuex'
import ComboboxServerSide from '@/components/ComboboxServerSide.vue'

export default {
  name: 'DirectorsCombobox',
  components: { ComboboxServerSide },
  data () {
    return {
      directors: [],
      directorSearch: '',
      directorSearchTimeout: null,
      movieLocal: {},
      selectedDirectors: []
    }
  },
  props: {
    value: {
      type: Array,
      required: true
    },
    movie: {
      required: true,
      type: Object
    }
  },
  mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedDirectors = [...this.movie.directors]
    if (this.defaultDirectors.length <= 0) {
      this.setDefaultDirectors()
    }
  },
  computed: {
    ...mapGetters('admin', ['defaultDirectors']),
    displayDirectors () {
      const directors = this.directorSearch.trim().length > 0
        ? this.directors
        : this.defaultDirectors

      return directors.map(d => {
        const { firstName, lastName } = d
        if (firstName !== lastName) {
          d.name = `${lastName}, ${firstName}`
        }
        return d
      })
    }
  },
  watch: {
    directorSearch () {
      if (this.directorSearchTimeout) {
        clearTimeout(this.directorSearchTimeout)
        this.directorSearchTimeout = null
      }
      this.directorSearchTimeout = setTimeout(() => {
        searchDirectors(this.directorSearch)
          .then(response => {
            this.directors = response.data
          })
          .catch(error => console.error(error))
      }, 500)
    },
    value (values) {
      this.selectedDirectors = [...values]
    }
  },
  methods: {
    ...mapActions('admin', ['setDefaultDirectors']),
    updateSelectedItems (value) {
      this.selectedDirectors = value
      this.$emit('input', value)
    }
  }
}
</script>

<style scoped>

</style>
