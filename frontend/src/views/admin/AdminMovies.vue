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
                  icon
                  @click="edit(item.id)">
                  <v-icon>
                    mdi-pencil
                  </v-icon>
                </v-btn>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <edit-movie-dialog
      v-if="selectedMovie"
      v-model="showEditDialog"
      :movie="selectedMovie">
    </edit-movie-dialog>
  </v-container>
</template>

<script>
import { getMovies } from '@/api/movies'
import EditMovieDialog from '@/components/EditMovieDialog.vue'

export default {
  name: 'AdminMovies',
  components: {
    EditMovieDialog
  },
  data () {
    return {
      loading: true,
      movies: [],
      options: {},
      total: 0,
      showEditDialog: false,
      selectedMovieId: null
    }
  },
  mounted () {
    this.$vuetify.theme.dark = false
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
    },
    selectedMovie () {
      const { selectedMovieId } = this
      if (!selectedMovieId) {
        return null
      }
      const index = this.movies.findIndex(m => m.id === selectedMovieId)
      return this.movies[index]
    }
  },
  watch: {
    options () {
      this.getMovies()
    },
    showEditDialog (value) {
      if (!value) {
        this.selectedMovieId = null
      }
    }
  },
  methods: {
    edit (id) {
      this.selectedMovieId = id
      this.showEditDialog = true
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
