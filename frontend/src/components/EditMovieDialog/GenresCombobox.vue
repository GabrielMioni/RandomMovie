<template>
  <combobox-server-side
    v-model="genreSearch"
    :items="genres"
    :selected-items="selectedGenres"
    label="Genres"
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
      selectedGenres: []
    }
  },
  props: {
    value: {
      required: true,
      type: Array
    },
    movieGenres: {
      required: true,
      type: Array
    }
  },
  computed: {
    ...mapGetters('admin', ['genres'])
  },
  mounted () {
    this.selectedGenres = [...this.movieGenres]
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
