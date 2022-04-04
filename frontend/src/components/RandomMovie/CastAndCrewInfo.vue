<template>
  <div>
    <v-row class="d-flex">
      <v-col
        cols="12"
        class="d-flex flex-column">
        <h3>Directed by</h3>
        <span class="cast-divider yellow accent-4 mt-3"></span>
      </v-col>
      <v-col
        v-for="(director, index) in directedBy"
        :key="`directed-by-${index}`"
        cols="6"
        class="cast-member d-flex">
        <v-tooltip
          bottom
          max-width="500px"
          color="black"
          nudge-right="150px">
          <template v-slot:activator="{ on, attrs }">
            <v-avatar size="80">
              <v-img
                alt="test"
                :lazy-src="`https://image.tmdb.org/t/p/w45${director.profilePath}`"
                :src="`https://image.tmdb.org/t/p/w342${director.profilePath}`"
                v-bind="attrs"
                v-on="on">
              </v-img>
            </v-avatar>
          </template>
          <span style="max-width: 500px">
            <p
              v-html="director.biography">
            </p>
          </span>
        </v-tooltip>
        <div class="ma-3">
          <h4>
            {{ director.name }}
          </h4>
        </div>
      </v-col>
      <v-col
        cols="12"
        class="d-flex flex-column">
        <h3>Cast</h3>
        <span class="cast-divider yellow accent-4 mt-3"></span>
      </v-col>
      <v-col
        class="cast-member d-flex"
        cols="6"
        v-for="index in 6"
        :key="index">
        <v-avatar
          color="primary"
          size="80">
          <v-img
            alt="test"
            lazy-src="https://image.tmdb.org/t/p/w45/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg"
            src="https://image.tmdb.org/t/p/w342/aUFBsGCN6qpcjsF14LccMzE5ye7.jpg">
          </v-img>
        </v-avatar>
        <div class="ma-3">
          <h4>
            Max von sydow
          </h4>
          as Albert Emanuel Vogler
        </div>
      </v-col>
    </v-row>
  </div>
</template>

<script>
export default {
  name: 'CastAndCrewInfo',
  props: {
    movie: {
      type: Object,
      required: true
    }
  },
  computed: {
    directedBy () {
      return this.movie.directors.map(d => {
        const { name } = d
        const director = {
          name,
          biography: null,
          birthday: null,
          deathday: null,
          profilePath: null
        }

        const directorDetails = this.movie.credits.find(c => c.knownFor === 'Directing' && c.name === name)

        if (directorDetails) {
          director.biography = directorDetails.biography
          director.birthday = directorDetails.birthday
          director.deathday = directorDetails.deathday
          director.profilePath = directorDetails.profilePath
        }

        return director
      })
    }
  }
}
</script>

<style scoped>

</style>
