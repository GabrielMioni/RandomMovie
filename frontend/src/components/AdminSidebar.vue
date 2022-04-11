<template>
  <v-navigation-drawer
    app
    permanent>
    <v-list-item>
      <v-list-item-content>
        <v-list-item-title class="text-h6">
          <router-link to="/movie">
            Random Movie
          </router-link>
        </v-list-item-title>
        <v-list-item-subtitle>
          Hello {{ userName }}
        </v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>
    <v-divider></v-divider>
    <v-list-item
      v-for="(route, index) in routes"
      :key="`sidebar-route-${index}`">
      <v-list-item-content>
        <v-list-item-title>
          <router-link :to="route.path">
            {{ route.title }}
          </router-link>
        </v-list-item-title>
      </v-list-item-content>
    </v-list-item>
  </v-navigation-drawer>
</template>

<script>
export default {
  name: 'AdminSidebar',
  data () {
    return {
      userName: '',
      routes: [
        {
          path: '/admin/movies',
          title: 'Movies'
        },
        {
          path: '/admin/filters',
          title: 'Filters'
        }
      ]
    }
  },
  mounted () {
    const userDataJson = localStorage.getItem('user')
    const userData = JSON.parse(userDataJson)
    this.userName = userData.userName
  }
}
</script>

<style scoped>

</style>
