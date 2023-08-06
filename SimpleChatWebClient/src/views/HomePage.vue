<template>
  <v-main>
    
  <v-container class="fill-height">
    <v-responsive class="text-center fill-height">
      <v-img height="300" src="@/assets/logo.png" />

      <div class="text-body-2 font-weight-light mb-n1">Welcome to</div>

      <h1 class="text-h2 font-weight-bold">Simple chat</h1>

      <div class="py-14"></div>

      <v-row class="d-flex align-center justify-center">
        <v-col cols="auto">
          <v-btn
            min-width="164"
            rel="noopener noreferrer"
            target="_blank"
            variant="text"
            @click="utils.router.goToAuth()"
          >
            <v-icon icon="mdi-view-dashboard" size="large" start />

            登陆 / 注册
          </v-btn>
        </v-col>
      </v-row>
    </v-responsive>
  </v-container>
  </v-main>
</template>

<script setup>

import utils from "@/utils";
import router from "@/router";

const authOk = utils.auth.loadAuthInfo();

if (authOk) {
  utils.auth.verifyAuthInfo().then((isOk) => {
    if (isOk) {
      router.push('/main');
    }
  });
}

</script>
