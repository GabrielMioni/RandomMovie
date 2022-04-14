<template>
  <v-container>
    <v-row>
      <v-col>
        <v-card>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="movies"
              :footer-props="{
                'items-per-page-options': [10, 20, 30, 40, 50]
              }"
              :items-per-page="20"
              :options.sync="options"
              :loading="loading"
              :server-items-length="total">
            </v-data-table>
            Welcome to Admin Movies
            <v-btn @click="getMovies">
              Do get movie stuff
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { getMovies } from '@/api/movies'

export default {
  name: 'AdminMovies',
  data () {
    return {
      headers: [
        {
          text: 'title',
          align: 'start',
          sortable: false,
          value: 'title'
        },
        {
          text: 'country',
          align: 'start',
          sortable: false,
          value: 'country'
        },
        {
          text: 'year',
          align: 'start',
          sortable: false,
          value: 'year'
        }
      ],
      loading: true,
      movies: [],
      options: {},
      total: 0
    }
  },
  mounted () {
    // this.getMovies()
  },
  watch: {
    options () {
      this.getMovies()
    }
  },
  methods: {
    getMovies () {
      console.log('GetMovies called')
      this.loading = true
      getMovies(this.options)
        .then(response => {
          if (response.status !== 200) {
            return
          }
          const { data: { movies, total } } = response
          this.movies = movies
          this.total = total
        })
        .catch(error => console.error(error))
        .finally(() => {
          this.loading = false
        })
    }
  }
}
</script>

<style scoped>

</style>
