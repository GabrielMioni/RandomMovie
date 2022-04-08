<template>
  <v-container class="random-movie">
    <v-row justify="center">
      <v-col
        md="10"
        sm="12">
        <v-card
          class="overflow-y-auto"
          height="800px">
          <v-card-text class="fill-height">
            <v-container fill-height fluid>
              <v-btn
                tile
                outlined
                class="filters-button"
                @click="openFilters">
                <v-icon>
                  mdi-plus
                </v-icon>
                Filters
              </v-btn>
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
                  v-else-if="initialized && movie.id > 0">
                  <v-col
                    md="6"
                    sm="12"
                    class="fill-height">
                    <movie-image
                      :movie-details="movie.meta"
                      xxl>
                    </movie-image>
                  </v-col>
                  <v-col
                    md="6"
                    sm="12"
                    class="mt-8">
                    <v-row>
                      <v-tabs
                        v-model="tab"
                        color="white">
                        <v-tabs-slider color="yellow accent-4"></v-tabs-slider>
                        <v-tab>Film</v-tab>
                        <v-tab>Cast</v-tab>
                      </v-tabs>
                    </v-row>
                    <v-row class="movie-information mt-8">
                      <v-col
                        class="pl-0 pt-4"
                        cols="11">
                        <keep-alive>
                          <v-tabs-items
                            v-model="tab">
                            <v-tab-item
                              transition="none">
                              <div>
                                <h1 class="pb-3">{{ movie.title }}</h1>
                                <p>
                                  Directed by {{ directedBy }} • {{ movie.year }} • {{ movie.country.name }} <br>
                                  {{ originalLanguage }} <br>
                                  {{ genresDisplay }}
                                </p>
                                <p>{{ movie.meta ? movie.meta.overview : '' }}</p>
                              </div>
                            </v-tab-item>
                            <v-tab-item transition="none">
                              <cast-and-crew-info
                                :movie="movie">
                              </cast-and-crew-info>
                            </v-tab-item>
                          </v-tabs-items>
                        </keep-alive>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-btn
                        @click="getMovie()"
                        width="100%">
                        Find another movie
                      </v-btn>
                    </v-row>
                  </v-col>
                </template>
                <template v-else>
                  <v-col
                    cols="12"
                    v-ripple
                    style="cursor: pointer"
                    @click="getMovie()">
                    <div class="d-flex flex-column align-center justify-center fill-height">
                      <v-icon
                        x-large
                        class="mb-3">
                        mdi-filmstrip
                      </v-icon>
                      <span v-if="initialized">
                        No movies found. Try removing some filters.
                      </span>
                      <span v-else>
                        Click to get a random movie.
                      </span>
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
import CastAndCrewInfo from '@/components/RandomMovie/CastAndCrewInfo'
import { getRandomMovie, getMovieById } from '@/api/movies'
import { mapGetters } from 'vuex'

export default {
  name: 'RandomMovie',
  components: {
    MovieImage,
    CastAndCrewInfo
  },
  props: {
    movieId: {
      required: false,
      default: null
    }
  },
  data () {
    return {
      movie: {
        id: 0,
        country: null,
        decade: null,
        directors: [],
        genres: [],
        meta: null,
        title: '',
        year: 0
      },
      movieLoading: false,
      initialized: false,
      tab: 0
    }
  },
  mounted () {
    if (this.movieId) {
      this.getMovie(true)
    }
    this.$root.$on('applyFilters', () => {
      this.getMovie()
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
    genresDisplay () {
      return this.movie.genres.map(g => g.name).join(' • ')
    },
    originalLanguage () {
      if (this.movie.meta !== null) {
        const language = new Intl.DisplayNames(['en'], { type: 'language' })
        const languageOf = language.of(this.movie.meta.originalLanguage)
        if (languageOf === 'xx') {
          return 'Silent'
        }
        return languageOf
      }
      return ''
    }
  },
  methods: {
    openFilters () {
      this.$root.$emit('openFilters')
    },
    getMovieByFilters () {
      const params = {
        genreIds: this.selectedGenres,
        decadeIds: this.selectedDecades,
        countryIds: this.selectedCountries,
        directorIds: this.selectedDirectors
      }

      return getRandomMovie(params)
    },
    getMovieById () {
      return getMovieById(this.movieId)
    },
    getMovie (byId = false) {
      this.tab = 0
      this.movieLoading = true

      const getMovieApiMethod = byId
        ? this.getMovieById
        : this.getMovieByFilters

      getMovieApiMethod()
        .then(response => {
          this.initialized = true
          if (response.status === 204) {
            this.resetMovie()
            return
          }
          this.movie = response.data
        })
        .catch(error => console.error(error))
        .finally(() => {
          setTimeout(() => {
            this.movieLoading = false
          }, 1000)
        })
    },
    resetMovie () {
      this.movie = {
        id: 0,
        country: null,
        decade: null,
        directors: [],
        genres: [],
        meta: null,
        title: '',
        year: 0
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.filters-button {
  position: absolute;
  top: 1rem;
  right: 1rem;
}
.movie-information {
  height: 600px;
}
.cast-member {
  height: 106px;
}
.cast-divider {
  height: 2px;
}
</style>
