<template>
  <v-theme-provider light>
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
                      v-model="selectedGenres"
                      dense
                      hide-details
                      :label="genre.name"
                      :value="genre.id">
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
      selectedGenres: []
    }
  },
  props: {
    filtersOpen: {
      type: Boolean,
      required: true
    }
  },
  mounted () {
    console.log('look at me I am the filters guys')
    this.setFilters()
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
    ...mapActions('filters', ['setFilters']),
    close () {
      this.filterIsOpenLocal = false
    }
  }
}
</script>

<style scoped>

</style>
