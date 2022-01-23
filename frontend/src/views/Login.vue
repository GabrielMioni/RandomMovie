<template>
  <v-container>
    <v-row
      align="center"
      justify="center">
      <v-col :cols="$vuetify.breakpoint.smAndUp ? 6 : 12">
        <v-form
          :disabled="loading"
          v-model="isValid">
          <v-card width="640px">
            <v-card-text>
              <v-text-field
                v-model="userName"
                label="Username"
                filled
                :rules="rules.userName">
              </v-text-field>
              <v-text-field
                v-model="password"
                label="Password"
                filled
                :type="showPassword ? 'text' : 'password'"
                :rules="rules.userName">
              </v-text-field>
              <v-switch
                v-model="showPassword"
                label="Show Password">
              </v-switch>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                @click="login"
                color="primary"
                :loading="loading">
                Login
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-form>
      </v-col>
    </v-row>
    <v-snackbar
      v-model="snackError"
      timeout="2000">
      {{ errorMessage }}
    </v-snackbar>
  </v-container>
</template>

<script>
import { mapActions } from 'vuex'
import { isRequired } from '@/rules'
import { login } from '@/api/user'

export default {
  name: 'Login',
  data () {
    return {
      userName: '',
      password: '',
      isValid: false,
      loading: false,
      showPassword: false,
      rules: {
        userName: [isRequired('The Username field is required')],
        password: [isRequired('The Password field is required')]
      },
      snackError: false,
      errorMessage: ''
    }
  },
  methods: {
    ...mapActions('user', ['setUserData']),
    login () {
      const { userName, password } = this
      this.loading = true
      let errorMessage = null

      login(userName, password)
        .then((response) => {
          this.setUserData(response.data)
        })
        .catch(error => {
          const { response: { data: { message } } } = error
          errorMessage = message
          console.error(error)
        })
        .finally(() => {
          setTimeout(() => {
            this.loading = false
            if (errorMessage) {
              this.snackError = true
              this.errorMessage = errorMessage
            }
          }, 500)
        })
    }
  }
}
</script>

<style scoped>

</style>
