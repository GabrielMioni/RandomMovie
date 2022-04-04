<template>
  <div>
    <random-movie></random-movie>
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
      this.filtersOpen = value
    }
  }
}
</script>
