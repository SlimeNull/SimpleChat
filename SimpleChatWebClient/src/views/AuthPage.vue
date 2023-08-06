<template >
  <v-main>
    <v-container class="fluid fill-height">
      <v-row align-center justify="center">
        <v-col :cols="6" lg="6" sm="8">
          <v-card justify-center class="mx-auto">
            <v-toolbar color="primary">
              <v-toolbar-title> Auth </v-toolbar-title>
            </v-toolbar>
            <v-card-text>
              <v-form>
                <v-text-field v-model="username" name="username" label="Username" type="text" placeholder="username"
                  required></v-text-field>

                <v-text-field v-model="password" name="password" label="Password" type="password" placeholder="password"
                  required></v-text-field>

                <div class="d-flex justify-end">
                  <v-btn @click="admin()">Admin</v-btn>
                  <v-btn @click="register()" class="ms-2"> Register </v-btn>
                  <v-btn @click="login()" color="primary" class="ms-2">
                    Login
                  </v-btn>
                </div>
              </v-form>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup>
import { ref } from "vue";
import utils from "@/utils";
import router from "@/router";
import values from "@/values";

const username = ref("");
const password = ref("");

async function admin() {
  const apiResult = await utils.api.auth.superAdminLogin(
    username.value,
    password.value
  );

  if (apiResult.isOk) {
    const apiValue = apiResult.value;
    values.state.JWT = apiValue.token;
    values.state.userId = -114514;
    values.state.userName = username;
    values.state.userProfile = {
      id: -114514,
      userName: username,
      nickname: "超级管理员",
      avatar: "",
      description: "",
      isAdmin: true,
    };

    router.push("/admin");
  } else {
    utils.tool.toast(apiResult.message);
  }
}

async function register() {
  const apiResult = await utils.api.auth.register(
    username.value,
    password.value
  );

  if (apiResult.isOk) {
    utils.tool.toast("注册成功, 待管理员审核后即可登录");
  } else {
    utils.tool.toast(apiResult.message);
  }
}

async function login() {
  const apiResult = await utils.api.auth.login(username.value, password.value);

  if (apiResult.isOk) {
    const apiValue = apiResult.value;
    utils.auth.saveAuthInfo(apiValue.userId, apiValue.userName, apiValue.userProfile, apiValue.token);

    router.push("/main");
  } else {
    utils.tool.toast(apiResult.message);
  }
}

</script>