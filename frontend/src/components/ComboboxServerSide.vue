<template>
  <v-combobox
    v-model="selectedItemsLocal"
    :items="items"
    chips
    deletable-chips
    :item-text="itemText"
    :label="label"
    multiple
    open-on-clear
    outlined
    ref="combobox"
    return-object>
  </v-combobox>
</template>

<script>
export default {
  name: 'ComboboxServerSide',
  props: {
    items: {
      type: Array,
      required: true
    },
    itemText: {
      type: String,
      required: false,
      default: 'name'
    },
    label: {
      type: String,
      required: true
    },
    selectedItems: {
      type: Array,
      required: true
    },
    value: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      inputElm: null,
      search: ''
    }
  },
  beforeDestroy () {
    this.removeInputEventListener()
  },
  computed: {
    selectedItemsLocal: {
      get () {
        return this.selectedItems
      },
      set (value) {
        const selectedItems = this.formatSelectedItems(value)
        if (selectedItems.length <= 0) {
          this.search = ''
        }
        this.$emit('selectedItemsUpdate', selectedItems)
      }
    }
  },
  watch: {
    search (value) {
      this.$emit('input', value)
    }
  },
  mounted () {
    this.addInputEventListener()
  },
  methods: {
    setInputValue (event) {
      this.search = event.target.value
    },
    addInputEventListener () {
      const combobox = this.$refs.combobox.$el
      this.inputElm = combobox.querySelector('input[type="text"]')
      this.inputElm.addEventListener('input', this.setInputValue)
    },
    removeInputEventListener () {
      this.inputElm.removeEventListener('input', this.setInputValue)
    },
    formatSelectedItems (selectedItems) {
      return selectedItems.filter(item => item.id !== undefined)
    }
  }
}
</script>

<style scoped>

</style>
