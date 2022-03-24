<template>
  <v-container>
    <v-row>
      <v-col
        :cols="12"
        class="d-flex justify-center">
        <v-card
          width="80%"
          min-height="800px">
          <v-card-text class="fill-height">
            <v-container fill-height fluid>
              <v-row>
                <v-spacer></v-spacer>
                <v-col cols="2">
                  <v-btn
                    tile
                    outlined>
                    <v-icon>
                      mdi-plus
                    </v-icon>
                    Filters
                  </v-btn>
                </v-col>
              </v-row>
              <v-row class="fill-height">
                <v-col
                  v-if="movieLoading"
                  class="align-center col d-flex justify-center">
                  <v-progress-circular
                    :size="50"
                    indeterminate>
                  </v-progress-circular>
                </v-col>
                <template
                  v-else-if="initialized">
                  <v-col>
                    <movie-image
                      :movie-details="movie.meta"
                      lg>
                    </movie-image>
                  </v-col>
                  <v-col>
                    <v-row class="d-flex fill-height flex-column">
                      <v-col
                        cols="11"
                        class="d-flex align-center">
                        <div>
                          <h1 class="pb-3">{{ movie.title }}</h1>
                          <p>
                            Directed by {{ directedBy }} • {{ movie.year }} • {{ movie.country.name }} <br>
                            {{ originalLanguage }}
                          </p>
                          <p>{{ movie.meta ? movie.meta.overview : '' }}</p>
                        </div>
                      </v-col>
                      <v-col class="d-flex flex-column justify-end pa-0">
                        <v-btn
                          @click="clickGetMovie"
                          width="100%">
                          Find another movie
                        </v-btn>
                      </v-col>
                    </v-row>
                  </v-col>
                </template>
                <template v-else>
                  <v-col
                    cols="12"
                    v-ripple
                    style="cursor: pointer"
                    @click="clickGetMovie">
                    <div class="d-flex flex-column align-center justify-center fill-height">
                      <v-icon
                        x-large
                        class="mb-3">
                        mdi-filmstrip
                      </v-icon>
                      Click to get a random movie.
                    </div>
                  </v-col>
                  <v-spacer></v-spacer>
                </template>
              </v-row>
            </v-container>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MovieImage from '@/components/MovieImage'
import { getRandomMovie } from '@/api/movies'
import { mapGetters } from 'vuex'

export default {
  name: 'RandomMovie',
  components: {
    MovieImage
  },
  data () {
    return {
      movie: {
        country: null,
        decade: null,
        directors: [],
        genres: [],
        meta: null,
        title: '',
        year: 0
      },
      movieLoading: false,
      initialized: false
    }
  },
  mounted () {
    this.$root.$on('applyFilters', () => {
      this.clickGetMovie()
    })
  },
  computed: {
    ...mapGetters('filters', ['selectedGenres', 'selectedDecades', 'selectedDirectors', 'selectedCountries']),
    directedBy () {
      const directedByString = this.movie.directors.map(director => director.name).join(', ')
      const lastIndexComma = directedByString.lastIndexOf(',')
      if (lastIndexComma < 0) {
        return directedByString
      }
      return `${directedByString.substring(0, lastIndexComma)} and ${directedByString.substring(lastIndexComma + 1)}`
    },
    originalLanguage () {
      if (this.movie.meta !== null) {
        const language = new Intl.DisplayNames(['en'], { type: 'language' })
        return language.of(this.movie.meta.originalLanguage)
      }
      return ''
    }
  },
  methods: {
    clickGetMovie () {
      const params = {
        genreIds: this.selectedGenres,
        decadeIds: this.selectedDecades,
        countryIds: this.selectedCountries,
        directorIds: this.selectedDirectors
      }

      this.movieLoading = true

      getRandomMovie(params)
        .then(response => {
          this.initialized = true
          this.movie = response.data
        })
        .catch(error => console.error(error))
        .finally(() => {
          setTimeout(() => {
            this.movieLoading = false
          }, 1000)
        })
    }
  }
}
</script>

<style scoped>
.random-movie {
  min-height: 80%;
}

</style>
