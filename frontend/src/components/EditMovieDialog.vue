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
              <v-combobox
                v-model="selectedDirectors"
                :items="displayDirectors"
                chips
                deletable-chips
                item-text="name"
                multiple
                open-on-clear
                outlined
                ref="combobox"
                return-object
                @change="updateCombo">
              </v-combobox>
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
  methods: {
    ...mapActions('admin', ['setDefaultDirectors']),
    updateCombo () {
      if (this.selectedDirectors.length <= 0) {
        this.directorSearch = ''
      }
    },
    setInputValue (event) {
      this.directorSearch = event.target.value
    },
    addInputEventListener () {
      const combobox = this.$refs.combobox.$el
      this.inputElm = combobox.querySelector('input[type="text"]')
      this.inputElm.addEventListener('input', this.setInputValue)
    },
    removeInputEventListener () {
      this.inputElm.removeEventListener('input', this.setInputValue)
    }
  },
  watch: {
    directorSearch () {
      if (this.directorSearchTimeout) {
        clearTimeout(this.directorSearchTimeout)
        this.directorSearchTimeout = null
      }
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
    this.selectedDirectors = this.movie.directors
    this.setDefaultDirectors()

    this.addInputEventListener()
  },
  beforeDestroy () {
    this.removeInputEventListener()
  },
  computed: {
    ...mapGetters('admin', ['defaultDirectors']),
    displayDirectors () {
      const directors = this.directorSearch.trim().length > 0
        ? this.directors
        : this.defaultDirectors

      const existingDirectorIds = this.selectedDirectors.map(d => d.id)

      return directors.map(d => {
        const { firstName, lastName } = d
        if (firstName !== lastName) {
          d.name = `${lastName}, ${firstName}`
        }
        return d
      }).filter(d => !existingDirectorIds.includes(d.id))
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
