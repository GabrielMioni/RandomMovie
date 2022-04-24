<template>
  <div>
    <random-movie
      :movieId="movieId">
    </random-movie>
    <filters-drawer
      :filters-open="filtersOpen"
      @drawerIsOpen="toggleFilters">
    </filters-drawer>
  </div>
</template>

<script>
// import HelloWorld from '../components/HelloWorld'
import RandomMovie from '@/components/RandomMovie/RandomMovie.vue'
import FiltersDrawer from '@/components/FiltersDrawer'

export default {
  name: 'Home',
  props: {
    movieId: {
      required: false,
      default: null
    }
  },
  data () {
    return {
      filtersOpen: false
    }
  },
  components: {
    RandomMovie,
    FiltersDrawer
  },
  beforeMount () {
    this.$vuetify.theme.dark = true
  },
  beforeDestroy () {
    this.$vuetify.theme.dark = false
  },
  methods: {
    toggleFilters (value) {
      this.filtersOpen = value
    }
  }
}
</script>
