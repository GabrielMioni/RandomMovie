<template>
  <v-dialog
    max-width="1200"
    :fullscreen="$vuetify.breakpoint.smAndDown"
    v-model="open">
    <v-card>
      <v-toolbar
        color="primary"
        class="mb-4"
        flat>
        <v-toolbar-title class="white--text">
          Edit Movie
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn
          color="white"
          icon
          @click="open = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-text>
        <v-form>
          <v-container>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="movieLocal.title"
                  label="Title"
                  outlined>
                </v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  v-model="movieLocal.year"
                  label="Year"
                  outlined>
                </v-text-field>
              </v-col>
            </v-row>
            <v-row dense>
              <directors-combobox
                v-model="selectedDirectors"
                :movie-directors="movie.directors">
              </directors-combobox>
            </v-row>
            <v-row dense>
              <genres-combobox
                v-model="selectedGenres"
                :movie-genres="movie.genres">
              </genres-combobox>
            </v-row>
            <v-row dense>
              <country-autocomplete
                v-model="selectedCountry">
              </country-autocomplete>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          plain
          depressed
          tile
          @click="open = false">
          Cancel
        </v-btn>
        <v-btn
          class="primary"
          depressed
          tile>
          Submit
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import DirectorsCombobox from '@/components/EditMovieDialog/DirectorsCombobox.vue'
import GenresCombobox from '@/components/EditMovieDialog/GenresCombobox.vue'
import CountryAutocomplete from '@/components/EditMovieDialog/CountryAutocomplete.vue'

export default {
  name: 'EditMovieDialog',
  components: {
    CountryAutocomplete,
    DirectorsCombobox,
    GenresCombobox
  },
  data () {
    return {
      movieLocal: {},
      selectedCountry: this.movie.country,
      selectedDirectors: [],
      selectedGenres: [],
      directorSearchTimeout: null,
      inputElm: null
    }
  },
  props: {
    value: {
      required: true,
      type: Boolean
    },
    movie: {
      required: true,
      type: Object
    }
  },
  mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedDirectors = this.movie.directors
    this.selectedCountry = this.movie.country
  },
  computed: {
    open: {
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

<style lang="scss" scoped>

</style>
