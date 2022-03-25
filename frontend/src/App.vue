<template>
  <v-app>
    <v-main>
      <v-app-bar elevation="4">
      </v-app-bar>
      <v-content
        fluid
        class="fill-height">
        <router-view
          class="main">
        </router-view>
      </v-content>
    </v-main>
  </v-app>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  name: 'App',

  data: () => ({
    //
  }),
  mounted () {
    this.initUserData()
    this.setConfigurationDetails()
  },
  methods: {
    ...mapActions('user', ['initUserData']),
    ...mapActions('meta', ['setConfigurationDetails'])
  }
}
</script>

<style lang="scss" scoped>
.main {
  height: 100%;
}
</style>
