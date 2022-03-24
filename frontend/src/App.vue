<template>
  <v-app>
    <v-main>
      <router-view/>
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
