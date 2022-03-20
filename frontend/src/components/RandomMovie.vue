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
              <v-row class="fill-height">
                <template v-if="movieLoaded">
                  <v-col>
                    <movie-image
                      v-if="movie.meta.posterPath"
                      :image-path="movie.meta['posterPath']"
                      original>
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
                          <p>{{ movie.meta.overview }}</p>
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
                  <v-spacer></v-spacer>
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

export default {
  name: 'RandomMovie',
  components: {
    MovieImage
  },
  data () {
    return {
      movie: {
        country: {},
        decade: {},
        directors: [],
        genres: [],
        meta: {},
        title: '',
        year: 0
      },
      movieLoaded: false
    }
  },
  computed: {
    directedBy () {
      const directedByString = this.movie.directors.map(director => director.name).join(', ')
      const lastIndexComma = directedByString.lastIndexOf(',')
      if (lastIndexComma < 0) {
        return directedByString
      }
      return `${directedByString.substring(0, lastIndexComma)} and ${directedByString.substring(lastIndexComma + 1)}`
    },
    originalLanguage () {
      if (Object.keys(this.movie.meta).length <= 0) {
        return ''
      }
      const language = new Intl.DisplayNames(['en'], { type: 'language' })
      return language.of(this.movie.meta.originalLanguage)
    }
  },
  methods: {
    clickGetMovie () {
      getRandomMovie()
        .then(response => {
          this.movieLoaded = true
          this.movie = response.data
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
