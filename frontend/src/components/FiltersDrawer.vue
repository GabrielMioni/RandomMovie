<template>
  <v-theme-provider dark>
    <v-navigation-drawer
      v-model="filterIsOpenLocal"
      width="50%"
      absolute
      temporary
      right>
      <v-card
        height="100%"
        width="100%">
        <v-toolbar elevation="0">
          <v-spacer></v-spacer>
          <v-btn
            depressed
            fab
            color="transparent"
            @click="close">
            <v-icon icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text class="fill-height">
          <v-container class="fill-height align-content-start">
            <v-row class="fill-height">
              <v-col cols="4">
                <v-list>
                  <v-list-item
                    v-for="(filter, index) in filterTypes"
                    :key="`filter-${index}`">
                    <v-list-item-title>
                      {{ filter }}
                    </v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-col>
              <v-divider vertical></v-divider>
              <v-col
                cols="8"
                class="pa-1">
                <v-row>
                  <v-col cols="12">
                    <h3>Genres</h3>
                  </v-col>
                  <v-col
                    v-for="(genre, index) in genres"
                    :key="`genre-filter-${index}`"
                    class="py-0"
                    cols="6">
                    <v-checkbox
                      dense
                      hide-details
                      :label="genre.name"
                      @click="clickCheckbox('selectedGenres', genre.id)">>
                    </v-checkbox>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12">
                    <h3>Decades</h3>
                  </v-col>
                  <v-col
                    v-for="(decade, index) in decades"
                    :key="`decade-filter-${index}`"
                    class="py-0"
                    cols="6">
                    <v-checkbox
                      dense
                      hide-details
                      :label="decade.name"
                      @click="clickCheckbox('selectedDecades', decade.id)">
                    </v-checkbox>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12">
                    <h3>Countries</h3>
                  </v-col>
                  <v-col
                    v-for="(country, index) in countries"
                    :key="`decade-filter-${index}`"
                    class="py-0"
                    cols="6">
                    <v-checkbox
                      dense
                      hide-details
                      :label="country.name"
                      @click="clickCheckbox('selectedCountries', country.id)">
                    </v-checkbox>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12">
                    <h3>Directors</h3>
                  </v-col>
                  <v-col
                    v-for="(director, index) in directors"
                    :key="`decade-filter-${index}`"
                    class="py-0"
                    cols="6">
                    <v-checkbox
                      dense
                      hide-details
                      :label="director.name"
                      @click="clickCheckbox('selectedDirectors', director.id)">
                    </v-checkbox>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
      </v-card>
    </v-navigation-drawer>
  </v-theme-provider>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'FiltersDrawer',
  data () {
    return {
      filterTypes: [
        'Genres',
        'Decades',
        'Countries',
        'Directors'
      ],
      selectedGenres: [],
      selectedDecades: [],
      selectedCountries: [],
      selectedDirectors: []
    }
  },
  props: {
    filtersOpen: {
      type: Boolean,
      required: true
    }
  },
  mounted () {
    this.setFilters()
  },
  watch: {
    selectedGenres (value) {
      this.setSelectedFilters({ type: 'genres', ids: value })
    },
    selectedDecades (value) {
      this.setSelectedFilters({ type: 'decades', ids: value })
    },
    selectedCountries (value) {
      this.setSelectedFilters({ type: 'countries', ids: value })
    },
    selectedDirectors (value) {
      this.setSelectedFilters({ type: 'directors', ids: value })
    }
  },
  computed: {
    ...mapGetters('filters', ['genres', 'decades', 'countries', 'directors']),
    filterIsOpenLocal: {
      get () {
        return this.filtersOpen
      },
      set (value) {
        this.$emit('drawerIsOpen', value)
      }
    }
  },
  methods: {
    ...mapActions('filters', ['setFilters', 'setSelectedFilters']),
    clickCheckbox (filterType, value) {
      const index = this[filterType].findIndex(id => id === value)
      if (index < 0) {
        this[filterType].push(value)
        return
      }
      this[filterType].splice(index, 1)
    },
    close () {
      this.filterIsOpenLocal = false
    }
  }
}
</script>

<style scoped>

</style>
