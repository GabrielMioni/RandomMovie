<template>
  <div>Hey bud I'm the CreditsCombobox</div>
</template>

<script>
export default {
  name: 'CreditsCombobox',
  props: {
    value: {
      type: Array,
      required: true
    },
    movieCredits: {
      required: true,
      type: Array
    }
  }
}
</script>

<style scoped>

</style>
