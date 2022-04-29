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
              <directors-combobox
                v-model="selectedDirectors"
                :movie="movie">
              </directors-combobox>
            </v-row>
            <v-row dense>
              <genres-combobox
                v-model="selectedGenres"
                :movie="movie">
              </genres-combobox>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import DirectorsCombobox from '@/components/EditMovieDialog/DirectorsCombobox.vue'
import GenresCombobox from '@/components/EditMovieDialog/GenresCombobox.vue'

export default {
  name: 'EditMovieDialog',
  components: {
    DirectorsCombobox,
    GenresCombobox
  },
  data () {
    return {
      movieLocal: {},
      selectedDirectors: [],
      selectedGenres: [],
      directorSearchTimeout: null,
      inputElm: null
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
  mounted () {
    this.movieLocal = { ...this.movie }
    this.selectedDirectors = this.movie.directors
  },
  computed: {
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

</style>
