<template>
  <div class="fill-height d-flex align-center justify-center">
    <v-img
      v-if="imagePath"
      :alt="alt"
      :src="imageUri"
      height="100%"
      contain>
    </v-img>
    <div
      v-else
      class="d-flex flex-column">
      <span>No poster available</span>
      <v-icon
        x-large
        class="ma-3">
        mdi-emoticon-frown-outline
      </v-icon>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'MovieImage',
  props: {
    movieDetails: {
      type: Object,
      required: false,
      default: null
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
    imagePath () {
      return this.movieDetails === null ? null : this.movieDetails.posterPath
    },
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
