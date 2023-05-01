<template>
  <v-theme-provider>
    <v-app>
      <v-app-bar :elevation="1" border color="primary">
        <v-btn retain-focus-on-click to="/" variant="plain" size="large">Home</v-btn>
        <v-btn  to="/about" variant="plain" size="large">About</v-btn>
        <v-spacer></v-spacer>

        <v-btn to="/login" variant="plain" size="x-large">Login</v-btn>
      </v-app-bar>

      <v-main>
        <router-view/>
      </v-main>
    </v-app>
  </v-theme-provider>

</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
