<template>
  <v-dialog
    max-width="1200"
    :fullscreen="$vuetify.breakpoint.smAndDown"
    v-model="open">
    <v-card>
      <v-card-text>
        <v-form>
          <v-container>
            <v-row>
              <v-col>
                <v-text-field
                  v-model="movie.title"
                  solo>
                </v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  v-model="movie.title"
                  solo>
                </v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'EditMovieDialog',
  data () {
    return {
      movieLocal: {}
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
  watch: {
    movie () {
      this.movieLocal = { ...this.movie }
    }
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

<style scoped>

</style>
