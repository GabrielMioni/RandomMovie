<template>
  <v-container>
    <v-row>
      <v-col>
        <v-btn
          @click="clickGetMovie">
          Gimme
        </v-btn>
      </v-col>
    </v-row>
    <v-row v-if="movie.title.trim().length > 0">
      <v-col>
        <v-card>
          <v-card-text>
            <movie-image
              :image-path="movie.meta['posterPath']"
              xl>
            </movie-image>
            {{ movie.title }}
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
