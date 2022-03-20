<template>
  <v-container class="fill-height">
    <v-row>
      <v-col>
        <v-btn
          @click="clickGetMovie">
          Gimme
        </v-btn>
      </v-col>
    </v-row>
    <v-row
      v-if="movie.title.trim().length > 0">
      <v-col
        :cols="12"
        class="d-flex justify-center">
        <v-card width="80%">
          <v-card-text>
            <v-container fluid>
              <v-row>
                <v-col>
                  <movie-image
                    :image-path="movie.meta['posterPath']"
                    xl>
                  </movie-image>
                </v-col>
                <v-col>
                  <h1 class="pb-3">{{ movie.title }}</h1>
                  <p>
                    Directed by {{ directedBy }} • {{ movie.year }} • {{ movie.country.name }} <br>
                    {{ originalLanguage }}
                  </p>
                  <p>{{ movie.meta.overview }}</p>
                </v-col>
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
      }
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
          this.movie = response.data
        })
    }
  }
}
</script>

<style scoped>

</style>
