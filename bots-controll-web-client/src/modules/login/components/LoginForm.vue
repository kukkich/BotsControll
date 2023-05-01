<template>
  <v-form v-model="valid">
    <v-container>
      <v-card class="elevation-12" color="primary">
        <v-toolbar color="secondary" elevation="4">
          <v-toolbar-title>
            <v-tabs align-tabs="end">
              <v-tab> Option </v-tab>
              <v-tab> Another Option </v-tab>
            </v-tabs>
          </v-toolbar-title>
        </v-toolbar>
        <v-card-text>
          <v-text-field name="login" label="Login" type="text"/>
          <v-text-field name="password" label="Password" type="password"/>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn to="/" color="secondary" size="large" variant="elevated" elevation="5">Login</v-btn>
        </v-card-actions>
      </v-card>
    </v-container>
  </v-form>

</template>

<script>
export default {
  name: "LoginForm",
  data: () => ({
    valid: false,
    firstname: '',
    lastname: '',
    nameRules: [
      v => !!v || 'Name is required',
      v => v.length <= 10 || 'Name must be less than 10 characters',
    ],
    email: '',
    emailRules: [
      v => !!v || 'E-mail is required',
      v => /.+@.+/.test(v) || 'E-mail must be valid',
    ],
  }),
}
</script>

<style scoped>

</style>