<template>
  <combobox-server-side
    v-model="genreSearch"
    :items="genres"
    label="Genres"
    :selected-items="selectedGenres"
    @update:selected-items="updateSelectedItems">
  </combobox-server-side>
</template>

<script>
import ComboboxServerSide from '@/components/ComboboxServerSide.vue'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'GenresCombobox',
  components: { ComboboxServerSide },
  data () {
    return {
      genreSearch: '',
      selectedGenres: [],
      movieLocal: {}
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
  computed: {
    ...mapGetters('admin', ['genres'])
  },
  mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedGenres = [...this.movie.genres]
    if (this.genres.length <= 0) {
      this.setGenres()
    }
  },
  methods: {
    ...mapActions('admin', ['setGenres']),
    updateSelectedItems (value) {
      this.selectedGenres = value
      this.$emit('input', value)
    }
  }
}
</script>

<style scoped>

</style>
