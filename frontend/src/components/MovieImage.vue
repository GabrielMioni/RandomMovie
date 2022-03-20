<template>
  <v-img
    :src="imageUri"
    :width="width"
    :alt="alt">
  </v-img>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'MovieImage',
  props: {
    imagePath: {
      type: String,
      required: false,
      default: ''
    },
    width: {
      type: [Number, String],
      required: false,
      default: 500
    },
    alt: {
      type: String,
      required: false,
      default: 'Movie poster'
    },
    xs: {
      type: Boolean,
      required: false,
      default: false
    },
    sm: {
      type: Boolean,
      required: false,
      default: false
    },
    md: {
      type: Boolean,
      required: false,
      default: false
    },
    lg: {
      type: Boolean,
      required: false,
      default: false
    },
    xl: {
      type: Boolean,
      required: false,
      default: false
    },
    xxl: {
      type: Boolean,
      required: false,
      default: false
    },
    original: {
      type: Boolean,
      required: false,
      default: false
    }
  },
  computed: {
    ...mapGetters('meta', ['secureBaseUrl', 'posterSizes']),
    imageUri () {
      const { secureBaseUrl, selectedSize, imagePath } = this
      return `${secureBaseUrl}${selectedSize}${imagePath}`
    },
    selectedSize () {
      const sizesArray = [
        { name: 'sm', value: this.sm },
        { name: 'md', value: this.md },
        { name: 'lg', value: this.lg },
        { name: 'xl', value: this.xl },
        { name: 'xxl', value: this.xxl },
        { name: 'original', value: this.original }
      ]

      const index = sizesArray.findIndex(size => size.value)
      const sizeName = index > -1 ? sizesArray[index].name : 'md'
      return this.posterSizes[sizeName]
    }
  }
}
</script>

<style scoped>

</style>
