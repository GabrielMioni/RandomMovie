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
                  v-else-if="initialized">
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
                              <v-row class="d-flex">
                                <template v-for="index in 2">
                                  <v-col
                                    class="cast-member d-flex"
                                    cols="6"
                                    :key="index">
                                    <v-tooltip
                                      bottom
                                      max-width="500px"
                                      color="black"
                                      nudge-right="150px">
                                      <template v-slot:activator="{ on, attrs }">
                                        <v-avatar size="80">
                                          <v-img
                                            alt="test"
                                            lazy-src="https://image.tmdb.org/t/p/w45/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg"
                                            src="https://image.tmdb.org/t/p/w342/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg"
                                            v-bind="attrs"
                                            v-on="on">
                                          </v-img>
                                        </v-avatar>
                                      </template>
                                      <span style="max-width: 500px">
                                        <p v-html="`Max von Sydow (10 April 1929 – 8 March 2020) was a Swedish actor. He also held French citizenship since 2002. He starred in many films and had supporting roles in dozens more. He performed in films filmed in many languages, including Swedish, Norwegian, English, Italian, German, Danish, French and Spanish.<br><br>Some of his most memorable film roles include knight Antonius Block in Ingmar Bergman's The Seventh Seal (the first of his eleven films with Bergman and the film that includes the iconic shot of his career in the scene where he plays chess with Death), Jesus in George Stevens's The Greatest Story Ever Told, Father Merrin in Friedkin's The Exorcist, Joubert the assassin in Three Days of the Condor, and Ming the Merciless in the 1980 version of Flash Gordon.<br><br>He was twice nominated for the Academy Award - Best Leading Actor for Pelle the Conqueror (1988) and Best Supporting Actor for Extremely Loud and Incredibly Close (2011).`"></p>
                                      </span>
                                    </v-tooltip>
                                    <div class="ma-3">
                                      <h4>
                                        Max von sydow
                                      </h4>
                                      as Albert Emanuel Vogler
                                    </div>
                                  </v-col>
                                </template>
                                <v-col
                                  cols="12"
                                  class="d-flex flex-column">
                                  <h3>Cast</h3>
                                  <span class="cast-divider yellow accent-4 mt-3"></span>
                                </v-col>
                                <v-col
                                  class="cast-member d-flex"
                                  cols="6"
                                  v-for="index in 6"
                                  :key="index">
                                  <v-avatar
                                    color="primary"
                                    size="80">
                                    <v-img
                                      alt="test"
                                      lazy-src="https://image.tmdb.org/t/p/w45/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg"
                                      src="https://image.tmdb.org/t/p/w342/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg">
                                    </v-img>
                                  </v-avatar>
                                  <div class="ma-3">
                                    <h4>
                                      Max von sydow
                                    </h4>
                                    as Albert Emanuel Vogler
                                  </div>
                                </v-col>
                              </v-row>
                            </v-tab-item>
                          </v-tabs-items>
                        </keep-alive>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-btn
                        @click="clickGetMovie"
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
      initialized: false,
      tab: 0
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
