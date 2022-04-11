<template>
  <router-view></router-view>
</template>

<script>
import { values } from '@/api/values'

export default {
  name: 'AdminHome',
  mounted () {
    values()
      .then(response => console.log(response))
      .catch(error => console.log(error))
  }
}
</script>

<style scoped>

</style>
