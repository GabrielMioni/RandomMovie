<template>
  <v-app>
    <v-app-bar
      v-if="!currentRouteIsAdmin"
      elevation="4"
      max-height="80px">
    </v-app-bar>
    <admin-sidebar v-if="currentRouteIsAdmin"></admin-sidebar>
    <v-main>
      <v-container>
        <router-view>
        </router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapActions } from 'vuex'
import AdminSidebar from './components/AdminSidebar'

export default {
  name: 'App',
  components: {
    AdminSidebar
  },
  computed: {
    currentRouteIsAdmin () {
      const { path } = this.$route
      return path.split('/').filter(part => part === 'admin').length > 0
    }
  },
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
