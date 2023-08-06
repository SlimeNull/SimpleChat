<template>
  <v-main>
    <v-container>
      <v-expansion-panels v-model="panel">
        <v-expansion-panel title="等待激活的用户">
          <v-expansion-panel-text>
            <v-list>
              <v-list-item v-for="user in usersToActive" :key="user.id"
                :title="utils.info.getUserFullDisplayName(user.userName, user.nickname)">
                <template v-slot:append>
                  <v-btn icon="mdi-checkbox-marked-circle" @click="activeUser(user)" size="small">
                  </v-btn>
                </template>
              </v-list-item>

              <v-list-item v-if="usersToActive.length === 0">
                <v-list-item-title>没有需要激活的用户</v-list-item-title>
              </v-list-item>
            </v-list>

            <div class="d-flex justify-end mt-5">
              <v-btn @click="loadUsersToActive()">刷新</v-btn>
            </div>
          </v-expansion-panel-text>
        </v-expansion-panel>
        <v-expansion-panel title="被封禁的用户" v-if="usersBanned.length != 0">
          <v-expansion-panel-text>
            <v-list>
              <v-list-item v-for="user in usersBanned" :key="user.id"
                :title="utils.info.getUserFullDisplayName(user.userName, user.nickname)">
                <template v-slot:append>
                  <v-btn icon="mdi-cancel" @click="setUserBan(user, false)"  size="small"/>
                </template>
              </v-list-item>
            </v-list>

            <div class="d-flex justify-end mt-5">
              <v-btn @click="loadUsersBanned()">刷新</v-btn>
            </div>
          </v-expansion-panel-text>
        </v-expansion-panel>
        <v-expansion-panel title="管理员用户">
          <v-expansion-panel-text>
            <v-list>
              <v-list-item v-for="user in usersIsAdmin" :key="user.id"
                :title="utils.info.getUserFullDisplayName(user.userName, user.nickname)">
                <template v-slot:append>
                  <v-btn icon="mdi-cancel" @click="setUserAdmin(user, false)" size="small" />
                </template>
              </v-list-item>
            </v-list>

            <div class="d-flex justify-end mt-5">
              <v-btn @click="loadUsersIsAdmin()">刷新</v-btn>
            </div>
          </v-expansion-panel-text>
        </v-expansion-panel>
        <v-expansion-panel title="所有用户">
          <v-expansion-panel-text>
            <v-list>
              <v-list-item v-for="user in allUsers" :key="user.id"
                :title="utils.info.getUserFullDisplayName(user.userName, user.nickname)">
                <template v-slot:append>
                  <v-btn icon="mdi-account-key" color="green" @click="setUserAdmin(user, true)" size="small">
                    <v-tooltip activator="parent" location="bottom">设置为管理员</v-tooltip>
                  </v-btn>
                  <v-btn icon="mdi-account-cancel" color="red" @click="setUserBan(user, true)" size="small" class="ms-2">
                    <v-tooltip activator="parent" location="bottom">封禁用户</v-tooltip>
                  </v-btn>
                </template>
              </v-list-item>
            </v-list>
          </v-expansion-panel-text>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>
  </v-main>
</template>

<script setup>
import values from "@/values";
import utils from "@/utils";

import { ref } from "vue";

console.log(`admin page, token: ${values.state.JWT}`);

const panel = ref([]);

const usersToActive = ref([]);
const usersBanned = ref([]);
const usersIsAdmin = ref([]);
const allUsers = ref([]);

async function loadUsersToActive() {
  return await utils.api.manage.getUsersNeedActive().then(apiResult => {
    if (apiResult.isOk) {
      usersToActive.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadUsersBanned() {
  return await utils.api.manage.getUsersBanned().then(apiResult => {
    if (apiResult.isOk) {
      usersBanned.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadUsersIsAdmin() {
  return await utils.api.manage.getUsersIsAdmin().then(apiResult => {
    if (apiResult.isOk) {
      usersIsAdmin.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadAllUsers() {
  return await utils.api.manage.getAllUsers().then(apiResult => {
    if (apiResult.isOk) {
      allUsers.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function activeUser(user) {
  return await utils.api.manage.activeUser(user.id).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast(`用户 ${user.userName} 已激活`);
      usersToActive.value = usersToActive.value.filter(
        (u) => u.id !== user.id
      );
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function setUserBan(user, enable) {
  return utils.api.manage.setUserBan(user.id, enable).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast(`设置完成! 用户 ${user.userName} 当前封禁状态: ${enable}`);

      if (enable) {
        usersBanned.value.push(user);
      } else {
        usersBanned.value = usersBanned.value.filter(
          (u) => u.id !== user.id
        );
      }
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function setUserAdmin(user, enable) {
  return utils.api.manage.setUserAdmin(user.id, enable).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast(`设置完成! 用户 ${user.userName} 当前管理员状态: ${enable}`);

      if (enable) {
        usersIsAdmin.value.push(user);
      } else {
        usersIsAdmin.value = usersIsAdmin.value.filter(
          (u) => u.id !== user.id
        );
      }
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadPageValues() {
  await loadUsersToActive();
  await loadUsersBanned();
  await loadUsersIsAdmin();
  await loadAllUsers();

  if (usersToActive.value.length > 0) {
    panel.value.push(0);
  }
}

loadPageValues();
</script>