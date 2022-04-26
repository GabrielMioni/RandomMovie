<template>
  <v-dialog
    max-width="1200"
    :fullscreen="$vuetify.breakpoint.smAndDown"
    v-model="open">
    <v-card>
      <v-toolbar
        color="primary"
        flat>
        <v-spacer></v-spacer>
        <v-btn
          color="white"
          icon
          @click="open = false">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-text>
        <v-form>
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
              <v-col>
                <v-text-field
                  v-model="directorSearch"
                  label="Search Directors"
                  hide-details
                  outlined>
                </v-text-field>
              </v-col>
            </v-row>
            <v-row
              class="directors"
              dense>
              <v-col
                v-for="director in displayDirectors"
                :key="`director-checkbox-${director.id}`"
                :cols="4">
                <v-checkbox
                  v-model="selectedDirectors"
                  dense
                  hide-details
                  multiple
                  :value="director.id"
                  :label="director.name">
                </v-checkbox>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import { searchDirectors } from '@/api/filters'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'EditMovieDialog',
  data () {
    return {
      movieLocal: {},
      directorSearch: '',
      directors: [],
      selectedDirectors: [],
      directorSearchTimeout: null
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
  methods: {
    ...mapActions('admin', ['setDefaultDirectors'])
  },
  watch: {
    directorSearch () {
      this.directorSearchTimeout = setTimeout(() => {
        searchDirectors(this.directorSearch)
          .then(response => {
            this.directors = response.data
          })
          .catch(error => console.error(error))
      }, 500)
    }
  },
  mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedDirectors = this.movie.directors.map(movie => movie.id)
    this.setDefaultDirectors()
  },
  computed: {
    ...mapGetters('admin', ['defaultDirectors']),
    displayDirectors () {
      const directors = this.directorSearch.trim().length > 0
        ? this.directors
        : this.defaultDirectors

      const movieDirectors = this.movie.directors
      const existingDirectorIds = movieDirectors.map(d => d.id)

      return movieDirectors.concat(directors.filter(d => !existingDirectorIds.includes(d.id)))
    },
    open: {
      get () {
        return this.value
      },
      set (value) {
        this.$emit('input', value)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.directors {
  height: 200px;
  overflow: auto;
}
</style>
