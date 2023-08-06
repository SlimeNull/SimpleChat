<template>
  <v-app-bar flat color="primary">
    <v-app-bar-title>
      <v-btn icon @click="utils.router.goToMain()">
        <v-icon icon="mdi-circle-slice-4" />
      </v-btn>
      SimpleChat
    </v-app-bar-title>

    <v-spacer></v-spacer>

    <template v-if="values.state.userProfile != null">
      <v-btn icon @click="utils.router.goToSearch()"> <v-icon>mdi-magnify</v-icon></v-btn>
      <v-btn icon>
        <v-avatar>
          <v-img :src="utils.info.getAvatar(values.state.userProfile.avatar)" alt="User"> </v-img>
        </v-avatar>

        <v-menu activator="parent">
          <v-list>
            <v-list-item @click="utils.router.goToMyProfile()" title="个人主页"/>
            <v-list-item @click="utils.router.goToAuth()" title="回到登陆页"></v-list-item>
          </v-list>
        </v-menu>
      </v-btn>
    </template>
  </v-app-bar>
</template>

<script setup>
import utils from "@/utils";
import values from "@/values";

</script>
