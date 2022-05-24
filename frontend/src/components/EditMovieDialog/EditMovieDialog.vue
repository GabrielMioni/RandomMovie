<template>
  <v-dialog
    max-width="1200"
    :fullscreen="$vuetify.breakpoint.smAndDown"
    :persistent="submitting"
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
        <v-form :disabled="submitting">
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
            <v-row dense>
              <credits-combobox
                v-model="selectedCredits"
                :movie-credits="movie.credits">
              </credits-combobox>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          plain
          depressed
          :disabled="submitting"
          tile
          @click="open = false">
          Cancel
        </v-btn>
        <v-btn
          class="primary"
          :disabled="submitting"
          :loading="submitting"
          depressed
          tile
          @click="submitEditMovie">
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
import CreditsCombobox from '@/components/EditMovieDialog/CreditsCombobox.vue'
import { editMovie } from '@/api/movies'
import { mapActions } from 'vuex'

export default {
  name: 'EditMovieDialog',
  components: {
    CountryAutocomplete,
    DirectorsCombobox,
    GenresCombobox,
    CreditsCombobox
  },
  data () {
    return {
      movieLocal: {},
      selectedCountry: this.movie.country,
      selectedDirectors: [],
      selectedGenres: [],
      selectedCredits: [],
      directorSearchTimeout: null,
      inputElm: null,
      submitting: false
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
  async mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedDirectors = this.movie.directors
    this.selectedCountry = this.movie.country
    this.selectedCredits = this.movie.credits
    this.selectedGenres = this.movie.genres
    await this.setAllFiltersData()
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
  },
  methods: {
    ...mapActions('admin', ['setAllFiltersData']),
    buildMovieData () {
      return {
        movieId: this.movieLocal.id,
        countryId: this.selectedCountry.id,
        creditIds: this.selectedCredits.map(c => c.id),
        directorIds: this.selectedDirectors.map(d => d.id),
        genreIds: this.selectedGenres.map(g => g.id),
        title: this.movieLocal.title,
        year: parseInt(this.movieLocal.year)
      }
    },
    submitEditMovie () {
      this.submitting = true
      const movieData = this.buildMovieData()
      let updatedMovie = null

      editMovie(movieData)
        .then(response => {
          updatedMovie = response.data
        })
        .catch(e => console.error(e))
        .finally(() => {
          setTimeout(() => {
            this.submitting = false
            if (updatedMovie !== null) {
              this.$emit('update:movie', updatedMovie)
            }
          }, 500)
        })
    }
  }
}
</script>

<style lang="scss" scoped>

</style>
