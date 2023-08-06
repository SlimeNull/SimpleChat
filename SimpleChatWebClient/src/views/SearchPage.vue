<template>
  <v-main>
    <v-container>
      <v-card>
        <v-card-title> 搜索 </v-card-title>
        <v-card-text>
          <v-list>
            <v-list-item>
              <v-text-field v-model="keyword" :loading="loading" :append-icon="keyword ? 'mdi-magnify' : null"
                label="Search templates" class="m-2" single-line 
                @click:append="search()"
                @keydown.enter="search()"></v-text-field>
            </v-list-item>

            <v-list-subheader> 用户 </v-list-subheader>

            <v-list-item v-for="user in users" :key="user.id"
              :title="utils.info.getUserFullDisplayName(user.userName, user.nickname)" :subtitle="user.id"
              :prepend-avatar="utils.info.getAvatar(user.avatar)" @click="utils.router.goToUserProfile(user.id)" />

            <v-list-item v-if="users.length == 0">
              <v-list-item-title>没有找到用户</v-list-item-title>
            </v-list-item>

            <v-list-subheader> 群组 </v-list-subheader>
            <v-list-item v-for="group in groups" :key="group.id"
              :title="group.name" :subtitle="group.id"
              :prepend-avatar="utils.info.getAvatar(group.avatar)" @click="utils.router.goToGroupProfile(group.id)"> </v-list-item>

            <v-list-item v-if="groups.length == 0">
              <v-list-item-title>没有找到群组</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-card-text>
      </v-card>
    </v-container>
  </v-main>
</template>

<script setup>
import { ref } from "vue";
import utils from "@/utils";

const keyword = ref("");
const loading = ref(false);
const users = ref([]);
const groups = ref([]);

async function search() {
  loading.value = true;

  const p1 = utils.api.user.searchUsers(keyword.value).then((apiResult) => {
    if (apiResult.isOk) users.value = apiResult.value;
    else utils.tool.toast(apiResult.message);
  });

  const p2 = utils.api.group.searchGroups(keyword.value).then((apiResult) => {
    if (apiResult.isOk) groups.value = apiResult.value;
    else utils.tool.toast(apiResult.message);
  });

  await Promise.all([p1, p2]);

  loading.value = false;
}

</script>