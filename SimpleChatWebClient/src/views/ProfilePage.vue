<template>
  <v-main>
    <v-container>
      <v-card>
        <v-card-text>
          <div class="d-flex align-start">
            <div>
              <v-avatar size="x-large">
                <v-img :src="utils.info.getAvatar(userProfile.avatar)"></v-img>
              </v-avatar>
            </div>
            <div class="ms-4">
              <div class="text-h6">{{ utils.info.getUserFullDisplayName(userProfile.userName, userProfile.nickname) }}
              </div>
              <div>{{ userProfile.id }}</div>
            </div>
          </div>
          <div class="mt-8">
            {{ userProfile.description || "这个人很懒，什么都没有留下" }}
          </div>
          <div class="mt-4">
            <v-btn icon @click="startFriendChat(userProfile.id)">
              <v-icon color="primary">mdi-chat</v-icon>
              <v-tooltip activator="parent" location="bottom">聊天</v-tooltip>
            </v-btn>

            <template v-if="myProfile.isAdmin">
              <v-btn icon class="ms-2" v-if="userProfile.isBanned" @click="setUserBan(userProfile, false)">
                <v-icon color="green">mdi-account-reactivate</v-icon>
                <v-tooltip activator="parent" location="bottom">解除用户封禁</v-tooltip>
              </v-btn>
              <v-btn icon class="ms-2" v-else @click="setUserBan(userProfile, true)">
                <v-icon color="red">mdi-account-cancel</v-icon>
                <v-tooltip activator="parent" location="bottom">封禁用户</v-tooltip>
              </v-btn>
            </template>
          </div>
        </v-card-text>
      </v-card>
      <v-card title="说说" class="mt-3">
        <v-list>
          <v-list-item v-for="post in posts" :key="post.id" class="mx-3 my-5">
            <v-list-item-subtitle>{{ post.time }}</v-list-item-subtitle>
            <div>
              {{ post.content }}
            </div>
          </v-list-item>
          <v-list-item title="你来到了没有人的荒地..." v-if="posts.length == 0"></v-list-item>
        </v-list>
      </v-card>
    </v-container>
  </v-main>
</template>

<script setup>

import { useRoute } from 'vue-router'
import { ref } from 'vue'

import utils from "@/utils";
import values from '@/values';

const myProfile = ref(values.state.userProfile);
const userProfile = ref({});

const route = useRoute();
const id = Number.parseInt(route.params.id);

const posts = ref([]);

async function loadMyProfile() {
  utils.api.user.getUserProfile(values.state.userProfile.id).then((apiResult) => {
    if (apiResult.isOk) {
      myProfile.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  })
}

async function loadUserProfile() {
  utils.api.user.getUserProfile(id).then((apiResult) => {
    if (apiResult.isOk) {
      userProfile.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadPosts() {
  let timeStart = null;
  if (posts.value.length != 0) {
    timeStart = posts.value[0].time;
  }

  return utils.api.post.getLatestPosts(id, 20, timeStart, null).then(apiResult => {
    if (apiResult.isOk) {
      const latestPosts = apiResult.value;
      for (const post of latestPosts) {
        posts.value.unshift(post);
      }
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function startFriendChat(userId) {
  let isFriendResult = await utils.api.user.checkUserIsFriend(userId);
  if (!isFriendResult.isOk) {
    utils.tool.toast(isFriendResult.message);
    return;
  }

  let isFriend = isFriendResult.value;
  if (!isFriend) {
    let requestFriendResult = await utils.api.request.requestFriend(userId);
    if (!requestFriendResult.isOk) {
      utils.tool.toast(requestFriendResult.message);
      return;
    } else {
      utils.tool.toast("已发送好友请求, 等待验证");
    }
  }
}

async function setUserBan(user, enable) {
  return utils.api.admin.setUserBan(user.id, enable).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast(`设置完成! 用户 ${user.userName} 当前封禁状态: ${enable}`);
      user.isBanned = enable;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

loadMyProfile();
loadUserProfile();
loadPosts();

</script>