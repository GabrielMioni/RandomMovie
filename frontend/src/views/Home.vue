<template>
  <div>
    <v-btn @click="toggleFilters">
      Filters
    </v-btn>
    <random-movie></random-movie>
    <filters-drawer
      :filters-open="filtersOpen"
      @drawerIsOpen="toggleFilters">
    </filters-drawer>
  </div>
</template>

<script>
// import HelloWorld from '../components/HelloWorld'
import RandomMovie from '../components/RandomMovie.vue'
import FiltersDrawer from '@/components/FiltersDrawer'

export default {
  name: 'Home',
  data () {
    return {
      filtersOpen: false
    }
  },
  components: {
    RandomMovie,
    FiltersDrawer
  },
  methods: {
    toggleFilters (value) {
      console.log({ clicko: value })
      this.filtersOpen = value
    }
  }
}
</script>
