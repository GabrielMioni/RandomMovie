<template>
  <v-combobox
    v-model="selectedDirectorsLocal"
    :items="displayDirectors"
    chips
    deletable-chips
    item-text="name"
    multiple
    open-on-clear
    outlined
    ref="combobox"
    return-object
    @change="updateCombo">
  </v-combobox>
</template>

<script>
import { searchDirectors } from '@/api/filters'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'DirectorsCombobox',
  data () {
    return {
      directors: [],
      directorSearch: '',
      movieLocal: {},
      selectedDirectorsLocal: []
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
    if (this.defaultDirectors.length <= 0) {
      this.setDefaultDirectors()
    }

    this.addInputEventListener()
  },
  beforeDestroy () {
    this.removeInputEventListener()
  },
  computed: {
    ...mapGetters('admin', ['defaultDirectors']),
    displayDirectors () {
      const directors = this.directorSearch.trim().length > 0
        ? this.directors
        : this.defaultDirectors

      const existingDirectorIds = this.selectedDirectorsLocal.map(d => d.id)

      return directors.map(d => {
        const { firstName, lastName } = d
        if (firstName !== lastName) {
          d.name = `${lastName}, ${firstName}`
        }
        return d
      }).filter(d => !existingDirectorIds.includes(d.id))
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
      this.selectedDirectorsLocal = [...values]
    }
  },
  methods: {
    ...mapActions('admin', ['setDefaultDirectors']),
    updateCombo () {
      if (this.selectedDirectorsLocal.length <= 0) {
        this.directorSearch = ''
      }
      this.selectedDirectorsLocal = this.selectedDirectorsLocal.filter(director => director.id !== undefined)
      this.$emit('input', this.selectedDirectorsLocal)
    },
    setInputValue (event) {
      this.directorSearch = event.target.value
    },
    addInputEventListener () {
      const combobox = this.$refs.combobox.$el
      this.inputElm = combobox.querySelector('input[type="text"]')
      this.inputElm.addEventListener('input', this.setInputValue)
    },
    removeInputEventListener () {
      this.inputElm.removeEventListener('input', this.setInputValue)
    }
  }
}
</script>

<style scoped>

</style>
