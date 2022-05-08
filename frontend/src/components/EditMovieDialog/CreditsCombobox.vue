<template>
  <combobox-server-side
    v-model="search"
    :items="displayItems"
    :loading="loading"
    :selected-items="selectedItems"
    item-text="name"
    label="Credits"
    @update:selected-items="updateSelectedItems">
  </combobox-server-side>
</template>

<script>
import { searchPeople } from '@/api/meta'
import { mapGetters } from 'vuex'
import ComboboxServerSide from '@/components/ComboboxServerSide.vue'

export default {
  name: 'CreditsCombobox',
  components: { ComboboxServerSide },
  data () {
    return {
      credits: [],
      search: '',
      searchTimeout: null,
      loading: false,
      selectedItems: []
    }
  },
  props: {
    value: {
      type: Array,
      required: true
    },
    movieCredits: {
      required: true,
      type: Array
    }
  },
  mounted () {
    this.selectedItems = [...this.movieCredits]
  },
  computed: {
    ...mapGetters('admin', ['defaultCredits']),
    displayItems () {
      const credits = this.search.trim().length > 0
        ? this.credits
        : this.defaultCredits

      return credits.map(d => {
        const { firstName, lastName } = d
        if (firstName !== lastName) {
          d.name = `${lastName}, ${firstName}`
        }
        return d
      })
    }
  },
  watch: {
    search () {
      this.loading = true
      if (this.searchTimeout) {
        clearTimeout(this.searchTimeout)
        this.searchTimeout = null
      }
      this.searchTimeout = setTimeout(() => {
        searchPeople(this.search)
          .then(response => {
            this.credits = response.data
          })
          .catch(error => console.error(error))
          .finally(() => {
            this.loading = false
          })
      })
    }
  },
  methods: {
    updateSelectedItems (value) {
      this.selectedItems = value
      this.$emit('input', value)
    }
  }
}
</script>

<style scoped>

</style>
