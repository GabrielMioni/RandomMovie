<template>
  <v-container>
    <v-row>
      <v-col>
        <v-card>
          <v-card-text>
            Welcome to Admin
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
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
