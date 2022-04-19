<template>
  <v-container>
    <v-row>
      <v-col>
        <v-card>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="moviesFormatted"
              :footer-props="{
                'items-per-page-options': [10, 20, 30, 40, 50]
              }"
              :items-per-page="20"
              :options.sync="options"
              :loading="loading"
              :server-items-length="total">
              <template v-slot:item.action="{ item }">
                <v-btn
                  @click="edit(item.id)">Edit</v-btn>
              </template>
            </v-data-table>
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
      loading: true,
      movies: [],
      options: {},
      total: 0
    }
  },
  mounted () {
    // this.getMovies()
  },
  computed: {
    headers () {
      const headers = [
        'id',
        'title',
        'country',
        'directors',
        'genres',
        'year'
      ].map(headerName => {
        return {
          text: headerName,
          align: 'start',
          sortable: true,
          value: headerName
        }
      })
      headers.push({
        text: '',
        align: 'start',
        sortable: false,
        value: 'action'
      })
      return headers
    },
    moviesFormatted () {
      return this.movies.map(movie => {
        return {
          id: movie.id,
          title: this.decodeHtml(movie.title),
          country: movie.country.name,
          directors: movie.directors.map(d => d.name).join(' / '),
          genres: movie.genres.map(g => g.name).join(' / '),
          year: movie.year,
          action: true
        }
      })
    }
  },
  watch: {
    options () {
      this.getMovies()
    }
  },
  methods: {
    edit (id) {
      const index = this.movies.findIndex(m => m.id === id)
      const movie = this.movies[index]
      console.log(movie)
    },
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
    },
    decodeHtml (value) {
      const textArea = document.createElement('textarea')
      textArea.innerHTML = value
      return textArea.value
    }
  }
}
</script>

<style scoped>

</style>
