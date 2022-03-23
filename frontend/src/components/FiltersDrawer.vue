<template>
  <v-theme-provider dark>
    <v-navigation-drawer
      v-model="filterIsOpenLocal"
      width="50%"
      absolute
      temporary
      right>
      <v-card
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
              <v-col
                class="filters-col-left"
                cols="4">
                <v-list>
                  <v-list-item
                    v-for="(filter, index) in filterTypes"
                    :key="`filter-${index}`">
                    <v-list-item-title
                      @click="scrollToFilter(filter)">
                      <div class="filters-col-left__type">{{ filter }}</div>
                    </v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-col>
              <v-divider vertical></v-divider>
              <v-col
                cols="8"
                class="pa-1 overflow-y-auto overflow-x-hidden filters-col-right">
                <v-row>
                  <v-col cols="12">
                    <h3
                      ref="genres">
                      Genres
                    </h3>
                  </v-col>
                  <v-col
                    v-for="genre in genres"
                    :key="`genre-filter-${genre.id}`"
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
                    <h3
                      ref="decades">
                      Decades
                    </h3>
                  </v-col>
                  <v-col
                    v-for="decade in decades"
                    :key="`decade-filter-${decade.id}`"
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
                    <h3
                      ref="countries">
                      Countries
                    </h3>
                  </v-col>
                  <v-col
                    v-for="country in countries"
                    :key="`country-filter-${country.id}`"
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
                    <h3
                      ref="directors">
                      Directors
                    </h3>
                    <a id="#directors"></a>
                  </v-col>
                  <template
                    v-for="(sortedDirectors, key) in directorsSorted">
                    <v-col
                      :key="`director-${key}`"
                      cols="12">
                      <h3>{{ key.toUpperCase()}}</h3>
                    </v-col>
                    <v-col
                      v-for="director in sortedDirectors"
                      :key="`director-filter-${director.id}`"
                      class="py-0"
                      cols="6">
                      <v-checkbox
                        dense
                        hide-details
                        :label="displayDirectorName(director)"
                        @click="clickCheckbox('selectedDirectors', director.id)">
                      </v-checkbox>
                    </v-col>
                  </template>
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
    directorsSorted () {
      const directorsSorted = {}
      'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => {
        directorsSorted[letter] = this.directors.filter(d => d.lastName.substr(0, 1).toLowerCase() === letter)
          .sort((a, b) => {
            const aLastName = a.lastName
            const bLastName = b.lastName
            return (aLastName < bLastName) ? -1 : (aLastName > bLastName) ? 1 : 0
          })
      })
      return directorsSorted
    },
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
    scrollToFilter (type) {
      const targetFilter = this.$refs[type.toLowerCase()]
      targetFilter.scrollIntoView({ behavior: 'smooth' })
    },
    displayDirectorName (director) {
      const { firstName, lastName } = director

      if (firstName.trim().length > 0 && lastName.trim().length > 0) {
        return `${lastName}, ${firstName}`
      }
      return director.name
    },
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

<style lang="scss" scoped>
.filters-col-right {
  height: 750px;
}
.filters-col-left {
  &__type {
    cursor: pointer;
  }
}
</style>
