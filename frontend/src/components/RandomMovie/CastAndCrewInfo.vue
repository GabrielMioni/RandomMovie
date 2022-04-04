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
        <cast-person
          :person="director">
        </cast-person>
      </v-col>
      <template v-if="cast.length > 0">
        <v-col
          cols="12"
          class="d-flex flex-column">
          <h3>Cast</h3>
          <span class="cast-divider yellow accent-4 mt-3"></span>
        </v-col>
        <v-col
          v-for="(person, index) in cast"
          :key="`cast-${index}`"
          cols="6"
          class="cast-member d-flex">
          <cast-person
            :person="person">
          </cast-person>
        </v-col>
      </template>
    </v-row>
  </div>
</template>

<script>
import CastPerson from './CastPerson'

export default {
  name: 'CastAndCrewInfo',
  components: {
    CastPerson
  },
  props: {
    movie: {
      type: Object,
      required: true
    }
  },
  computed: {
    cast () {
      return this.movie.credits.filter(c => c.knownFor === 'Acting')
    },
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
