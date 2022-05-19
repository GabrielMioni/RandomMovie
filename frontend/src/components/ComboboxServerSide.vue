<template>
  <v-combobox
    v-model="selectedItemsLocal"
    :items="items"
    :item-text="itemText"
    :label="label"
    :loading="loading"
    chips
    deletable-chips
    multiple
    open-on-clear
    outlined
    ref="combobox"
    @update:search-input="setSearchValue"
    return-object>
    <template v-slot:item="{ item, attrs }">
      <v-list-item
        v-bind="modifyAttrs(attrs, item.id)"
        @click="setSelectedItems(item)">
        <v-list-item-action>
          <v-simple-checkbox
            @click="setSelectedItems(item)"
            :value="checkIfItemIsSelected(item.id)"
            v-ripple>
          </v-simple-checkbox>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title
            v-text="item[itemText]">
          </v-list-item-title>
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
    loading: {
      type: Boolean,
      required: false,
      default: false
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
        this.$emit('update:selected-items', selectedItems)
      }
    }
  },
  watch: {
    search (value) {
      const setValue = value === null ? '' : value
      this.$emit('input', setValue)
    }
  },
  methods: {
    checkIfItemIsSelected (id) {
      return this.selectedItemsLocal.findIndex(i => i.id === id) > -1
    },
    formatSelectedItems (selectedItems) {
      return selectedItems.filter(item => item.id !== undefined)
    },
    modifyAttrs (attrs, id) {
      attrs.inputValue = this.checkIfItemIsSelected(id)
      return attrs
    },
    setSearchValue (value) {
      this.search = value
    },
    setSelectedItems (clickedItem) {
      const index = this.selectedItemsLocal.findIndex(i => i.id === clickedItem.id)
      if (index > -1) {
        this.selectedItemsLocal.splice(index, 1)
        return
      }
      const selected = [...this.selectedItems]
      selected.push(clickedItem)
      this.selectedItemsLocal = selected
    }
  }
}
</script>

<style scoped>

</style>
