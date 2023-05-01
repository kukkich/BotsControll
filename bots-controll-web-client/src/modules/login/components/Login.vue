<template>
  <v-container style="margin-top: 10px;">
    <v-row>
      <v-col />
      <v-col cols="5">
        <LoginForm/>
      </v-col>
      <v-col />
    </v-row>

  </v-container>
</template>

<script>

import {Options, Vue} from "vue-class-component";
import LoginForm from "@/modules/login/components/LoginForm";

@Options({
  components: {
    LoginForm,
  },
})

export default class Login extends Vue {}

</script>

<style scoped>

</style>