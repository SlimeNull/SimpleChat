<template>
  <v-app class="bg-grey-lighten-5">
    <default-bar />

    <router-view />

    <div class="text-center">
      <v-dialog v-model="values.state.globalDialog" width="auto">
        <v-card>
          <v-card-text>
            {{ values.state.globalDialogText }}
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" block @click="values.state.globalDialog = false">
              OK
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
  </v-app>
</template>

<script setup>
import DefaultBar from "./AppBar.vue";
import values from "@/values";

import utils from "@/utils";
import router from "@/router";

async function loadAuthAndProfile() {
  let authOk = utils.auth.loadAuthInfo();
  let verifyOk = false;

  if (authOk)
    verifyOk = await utils.auth.verifyAuthInfo()

  if (!authOk || !verifyOk || values.state.userId == -1) {
    router.push("/auth");
    return;
  }

  // 更新一下用户信息
  utils.api.user.getUserProfile(values.state.userId).then(apiResult => {
    if (apiResult.isOk)
      values.state.userProfile = apiResult.value;

      if (values.state.userProfile.isBanned) {
        utils.tool.toast("您已被封禁, 请联系管理员");
        router.push("/auth");
      }
  })
}

loadAuthAndProfile();

</script>
