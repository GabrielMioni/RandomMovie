<template>
  <div class="d-flex">
    <v-avatar size="80">
      <v-img
        v-if="person.profilePath"
        alt="test"
        :lazy-src="`https://image.tmdb.org/t/p/w45${person.profilePath}`"
        :src="`https://image.tmdb.org/t/p/w342${person.profilePath}`">
      </v-img>
      <v-icon
        size="100"
        v-else>
        mdi-account-circle
      </v-icon>
    </v-avatar>
    <div class="ma-3">
      <h4>
        {{ person.name }}
      </h4>
      <template v-if="person.knownFor === 'Acting' && person.character">
        as {{ person.character }}
      </template>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CastPerson',
  props: {
    person: {
      type: Object,
      required: true
    }
  }
}
</script>

<style scoped>

</style>
