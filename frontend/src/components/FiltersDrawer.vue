<template>
  <v-navigation-drawer
    v-model="filterIsOpenLocal"
    width="50%"
    absolute
    temporary
    right>
    <v-container>
      <v-row>
        <v-btn @click="close">
          <v-icon icon>mdi-close</v-icon>
        </v-btn>
      </v-row>
    </v-container>
    <v-list-item>
      <v-list-item-title>
        Hi.
      </v-list-item-title>
    </v-list-item>
  </v-navigation-drawer>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'FiltersDrawer',
  props: {
    filtersOpen: {
      type: Boolean,
      required: true
    }
  },
  mounted () {
    console.log('look at me I am the filters guys')
    this.storeFilters()
  },
  computed: {
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
    ...mapActions('filters', ['storeFilters']),
    close () {
      this.filterIsOpenLocal = false
    }
  }
}
</script>

<style scoped>

</style>
