<template>
  <v-combobox
    v-model="selectedItemsLocal"
    :items="items"
    :item-text="itemText"
    :label="label"
    chips
    deletable-chips
    multiple
    open-on-clear
    outlined
    ref="combobox"
    return-object>
    <template v-slot:item="{ item, attrs }">
      <v-list-item @click="setSelectedItems(item)" v-bind="attrs">
        <v-list-item-action>
          <v-simple-checkbox
            @click="setSelectedItems(item)"
            :color="checkIfItemIsSelected(item) ? 'primary' : ''"
            :value="checkIfItemIsSelected(item)"
            v-ripple>
          </v-simple-checkbox>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title v-text="item[itemText]"></v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </template>
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
    },
    checkIfItemIsSelected (item) {
      return this.selectedItemsLocal.findIndex(i => i.id === item.id) > -1
    },
    setSelectedItems (clickedItem) {
      const index = this.selectedItemsLocal.findIndex(i => i.id === clickedItem.id)
      if (index > -1) {
        this.selectedItemsLocal.splice(index, 1)
        return
      }
      this.selectedItemsLocal.push(clickedItem)
    }
  }
}
</script>

<style scoped>

</style>
