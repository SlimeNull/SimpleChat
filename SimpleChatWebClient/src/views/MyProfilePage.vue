<template>
  <v-main>
    <v-container>
      <v-card>
        <v-card-text>
          <div class="d-flex align-start">
            <div>
              <v-avatar class="elevation-1" size="x-large">
                <v-img :src="utils.info.getAvatar(userProfile.avatar)"></v-img>
              </v-avatar>
            </div>
            <div class="ms-4">
              <div class="text-h6">{{ utils.info.getUserFullDisplayName(userProfile.userName, userProfile.nickname) }}
              </div>
              <div>{{ userProfile.id }}</div>
            </div>
            <v-spacer></v-spacer>
            <div>
              <v-btn text @click="openChangeProfileDialog()">编辑</v-btn>
              <v-dialog v-model="changeProfileDialog">
                <v-row class="justify-center">
                  <v-col cols="12" sm="8" md="6">
                    <v-card>
                      <v-card-title>
                        更改个人资料
                      </v-card-title>
                      <v-card-text>
                        <div class="d-flex">
                          <div class="pt-3">
                            <v-avatar size="x-large" class="elevation-1">
                              <v-img :src="utils.info.getAvatar(changeProfileDialogModel.avatarPreviewUrl)" />
                            </v-avatar>
                          </div>
                          <v-container class="grow-1">
                            <v-row>
                              <v-col cols="12" md="6">
                                <v-text-field label="昵称" v-model="changeProfileDialogModel.nickname" />
                              </v-col>
                              <v-col cols="12" md="6">
                                <v-text-field label="描述" v-model="changeProfileDialogModel.description" />
                              </v-col>
                              <v-col cols="12">
                                <v-file-input label="头像" v-model="changeProfileDialogModel.avatarFiles" accept="image/*"
                                  @update:model-value="changeProfileDialogModelAvatarFilesChanged()" />
                              </v-col>
                            </v-row>
                          </v-container>
                        </div>
                      </v-card-text>
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue-darken-1" variant="text" @click="changeProfileDialog = false">
                          取消
                        </v-btn>
                        <v-btn color="blue-darken-1" variant="text" @click="changeProfile()">
                          确认
                        </v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-col>
                </v-row>
              </v-dialog>
            </div>
          </div>
          <div class="mt-8">
            {{ userProfile.description || "这个人很懒，什么都没有留下" }}
          </div>
        </v-card-text>
      </v-card>

      <v-card class="mt-5" v-if="sentFriendRequests.length != 0 || receivedFriendRequests.length != 0">
        <v-card-title>
          <div class="text-h6">好友请求</div>
          <div class="text-caption">共 {{ sentFriendRequests.length + receivedFriendRequests.length }} 个好友请求</div>
        </v-card-title>
        <v-list>
          <template v-if="sentFriendRequests.length != 0">
            <v-list-subheader>发送出的</v-list-subheader>
            <v-list-item v-for="req in sentFriendRequests" :key="req.id"
              :title="utils.info.getUserFullDisplayName(req.contextProfile.userName, req.contextProfile.nickname)"
              :subtitle="req.contextProfile.id" :prepend-avatar="utils.info.getAvatar(req.contextProfile.avatar)"
              @click="utils.router.goToUserProfile(req.contextProfile.id)" />
          </template>

          <template v-if="receivedFriendRequests.length != 0">
            <v-list-subheader>接收到的</v-list-subheader>
            <v-list-item v-for="req in receivedFriendRequests" :key="req.id"
              :title="utils.info.getUserFullDisplayName(req.contextProfile.userName, req.contextProfile.nickname)"
              :subtitle="req.contextProfile.id">
              <template v-slot:prepend>
                <v-avatar>
                  <v-img :src="utils.info.getAvatar(req.contextProfile.avatar)" />
                </v-avatar>
              </template>

              <template v-slot:append>
                <v-btn icon size="small" flat @click="acceptFriendRequest(req.id)">
                  <v-icon color="primary">mdi-checkbox-marked-circle</v-icon>
                </v-btn>
              </template>
            </v-list-item>
          </template>
        </v-list>
      </v-card>

      <v-card class="mt-5" v-if="sentGroupRequests.length != 0 || receivedGroupRequests.length != 0">
        <v-card-title>
          <div class="text-h6">加群请求</div>
          <div class="text-caption">共 {{ sentFriendRequests.length + receivedFriendRequests.length }} 个加群请求</div>
        </v-card-title>
        <v-list>
          <template v-if="sentGroupRequests.length != 0">
            <v-list-subheader>发送出的</v-list-subheader>
            <v-list-item v-for="req in sentGroupRequests" :key="req.id" :title="req.contextProfile.name"
              :subtitle="req.contextProfile.id" :prepend-avatar="utils.info.getAvatar(req.contextProfile.avatar)"
              @click="utils.router.goToGroupProfile(req.contextProfile.id)" />
          </template>

          <template v-if="receivedGroupRequests.length != 0">
            <v-list-subheader>接收到的</v-list-subheader>
            <v-list-item v-for="req in receivedGroupRequests" :key="req.id">
              <template v-slot:prepend>
                <v-btn icon @click="utils.router.goToUserProfile(req.contextUserProfile.id)" size="small">
                  <v-avatar>
                    <v-img :src="utils.info.getAvatar(req.contextUserProfile.avatar)" />
                  </v-avatar>
                </v-btn>
                <v-btn icon @click="utils.router.goToGroupProfile(req.contextGroupProfile.id)" size="small" class="mx-2">
                  <v-avatar>
                    <v-img :src="utils.info.getAvatar(req.contextGroupProfile.avatar)" />
                  </v-avatar>
                </v-btn>
              </template>

              <v-list-item-title>
                {{ utils.info.getUserDisplayName(req.contextUserProfile.userName, req.contextUserProfile.nickname) }}
                请求加入 {{ req.contextGroupProfile.name }}
              </v-list-item-title>

              <template v-slot:append>
                <v-btn icon size="small" flat @click="acceptGroupRequest(req.id)">
                  <v-icon color="primary">mdi-checkbox-marked-circle</v-icon>
                </v-btn>
              </template>
            </v-list-item>
          </template>
        </v-list>
      </v-card>
      <v-row class="mt-5">
        <v-col cols="3">
          <v-card>
            <v-card-title>
              <div class="text-h6">好友列表</div>
              <div class="text-caption">共 {{ friends.length }} 个好友</div>
            </v-card-title>
            <v-list>
              <v-list-item v-for="friend in friends" :key="friend.id"
                :title="utils.info.getUserFullDisplayName(friend.userName, friend.nickname)" :subtitle="friend.id"
                :prepend-avatar="utils.info.getAvatar(friend.avatar)" @click="utils.router.goToUserProfile(friend.id)">

                <template v-slot:append>
                  <v-btn icon size="x-small" flat>
                    <v-icon icon="mdi-dots-horizontal"></v-icon>
                    <v-menu activator="parent">
                      <v-list>
                        <v-list-item @click="deleteFriend(friend.id)">
                          <v-list-item-title>删除好友</v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </v-btn>
                </template>
              </v-list-item>
            </v-list>
          </v-card>

          <v-card class="mt-5">
            <v-card-title>
              <div class="text-h6">群组列表</div>
              <div class="text-caption">共 {{ groups.length }} 个群组</div>
            </v-card-title>
            <v-list>
              <v-list-item v-for="group in groups" :key="group.id" :title="group.name" :subtitle="group.id"
                :prepend-avatar="utils.info.getAvatar(group.avatar)" @click="utils.router.goToGroupProfile(group.id)">

                <template v-slot:append>
                  <v-btn icon size="x-small" flat>
                    <v-icon icon="mdi-dots-horizontal"></v-icon>
                    <v-menu activator="parent">
                      <v-list>
                        <v-list-item @click="leaveGroup(group.id)">
                          <v-list-item-title>退出群组</v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </v-btn>
                </template>
              </v-list-item>
            </v-list>
          </v-card>
        </v-col>
        <v-col cols="9">
          <v-card title="说说">
            <v-list>
              <v-list-item>
                <v-container>
                  <v-row>
                    <v-col cols="10">
                      <v-textarea variant="solo" v-model="postInput"></v-textarea>
                    </v-col>
                    <v-col cols="2">
                      <v-btn block @click="sendPost(postInput)">Post</v-btn>
                    </v-col>
                  </v-row>
                </v-container>
              </v-list-item>

              <v-divider />

              <v-list-item v-for="post in posts" :key="post.id" class="mx-3 my-5">
                <v-list-item-subtitle>{{ post.time }}</v-list-item-subtitle>
                <div>
                  {{ post.content }}
                </div>
              </v-list-item>
              <v-list-item title="你来到了没有人的荒地..." v-if="posts.length == 0"></v-list-item>
            </v-list>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup>

import utils from '@/utils';
import values from '@/values';

import { ref } from 'vue'


const userProfile = ref(utils.tool.getEmptyUserProfile());

const friends = ref([]);
const groups = ref([]);
const sentFriendRequests = ref([]);
const receivedFriendRequests = ref([]);
const sentGroupRequests = ref([]);
const receivedGroupRequests = ref([]);
const posts = ref([]);

const postInput = ref('');

const changeProfileDialog = ref(false);
const changeProfileDialogModel = ref({
  nickname: '',
  description: '',
  avatarFiles: null,
  avatarPreviewUrl: null
});

async function openChangeProfileDialog() {
  changeProfileDialogModel.value.nickname = userProfile.value.nickname;
  changeProfileDialogModel.value.description = userProfile.value.description;
  changeProfileDialogModel.value.avatarFiles = null;
  changeProfileDialogModel.value.avatarPreviewUrl = userProfile.value.avatar;

  changeProfileDialog.value = true;
}

async function changeProfileDialogModelAvatarFilesChanged() {
  if (changeProfileDialogModel.value.avatarFiles.length != 0) {
    const file = changeProfileDialogModel.value.avatarFiles[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
      changeProfileDialogModel.value.avatarPreviewUrl = reader.result;
    }
  }
}

async function changeProfile() {
  changeProfileDialog.value = true;

  let avatar = null;
  if (changeProfileDialogModel.value.avatarFiles != null) {
    const uploadResult = await utils.api.file.upload(changeProfileDialogModel.value.avatarFiles[0]);

    if (uploadResult.isOk) {
      avatar = uploadResult.value.fileUrl;
    } else {
      utils.tool.toast(uploadResult.message);
      return;
    }
  }

  utils.api.user.setSelfProfile(changeProfileDialogModel.value.nickname, changeProfileDialogModel.value.description, avatar).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast("已更改个人资料");
      changeProfileDialog.value = false;
      loadUserProfile();
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadUserProfile() {
  const apiResult = await utils.api.user.getUserProfile(values.state.userId);

  if (apiResult.isOk) {
    userProfile.value = apiResult.value;
    values.state.userProfile = apiResult.value;
  } else {
    utils.tool.toast(apiResult.message);
  }
}

async function loadFriends() {
  return await utils.api.user.getFriends().then(apiResult => {
    if (apiResult.isOk) {
      friends.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadGroups() {
  utils.api.group.getGroups().then(apiResult => {
    if (apiResult.isOk) {
      groups.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadSentFriendRequests() {
  return await utils.api.request.getSentFriendRequests().then(async apiResult => {
    if (!apiResult.isOk) {
      utils.tool.toast(apiResult.message);
      return;
    }

    const _sentFriendRequests = [];

    for (const req of apiResult.value) {
      await utils.api.user.getUserProfile(req.userToId).then(apiResult => {
        if (apiResult.isOk) {
          req.contextProfile = apiResult.value;
        } else {
          utils.tool.toast(apiResult.message);
        }
      });

      _sentFriendRequests.push({
        id: req.id,
        userFromId: req.userFromId,
        userToId: req.userToId,

        contextProfile: req.contextProfile,
      });
    }

    sentFriendRequests.value = _sentFriendRequests;
  });
}

async function loadReceivedFriendRequests() {
  return await utils.api.request.getReceivedFriendRequests().then(async apiResult => {
    if (!apiResult.isOk) {
      utils.tool.toast(apiResult.message);
      return;
    }

    const _receivedFriendRequests = [];

    for (const req of apiResult.value) {
      console.log(req);

      await utils.api.user.getUserProfile(req.userFromId).then(apiResult => {
        if (!apiResult.isOk) {
          utils.tool.toast(apiResult.message);
        }

        req.contextProfile = apiResult.value;

        _receivedFriendRequests.push({
          id: req.id,
          userFromId: req.userFromId,
          userToId: req.userToId,

          contextProfile: req.contextProfile,
        });
      });
    }

    receivedFriendRequests.value = _receivedFriendRequests;
  });
}

async function loadSentGroupRequests() {
  return await utils.api.request.getSentGroupRequests().then(async apiResult => {
    if (!apiResult.isOk) {
      utils.tool.toast(apiResult.message);
      return;
    }

    const _sentGroupRequests = [];

    for (const req of apiResult.value) {
      await utils.api.group.getGroupProfile(req.groupToId).then(apiResult => {
        if (apiResult.isOk) {
          req.contextProfile = apiResult.value;
        } else {
          utils.tool.toast(apiResult.message);
        }
      });

      _sentGroupRequests.push({
        id: req.id,
        userFromId: req.userFromId,
        userToId: req.userToId,

        contextProfile: req.contextProfile,
      });
    }

    sentGroupRequests.value = _sentGroupRequests;
  });
}

async function loadReceivedGroupRequests() {
  return await utils.api.request.getReceivedGroupRequests().then(async apiResult => {
    if (!apiResult.isOk) {
      utils.tool.toast(apiResult.message);
      return;
    }

    const _receivedGroupRequests = [];
    for (const req of apiResult.value) {
      const reqGroupProfileResult = await utils.api.group.getGroupProfile(req.groupToId);
      if (!reqGroupProfileResult.isOk)
        continue;

      const reqUserProfileResult = await utils.api.user.getUserProfile(req.userFromId);
      if (!reqUserProfileResult.isOk)
        continue;

      _receivedGroupRequests.push({
        id: req.id,
        userFromId: req.userFromId,
        userToId: req.userToId,

        contextUserProfile: reqUserProfileResult.value,
        contextGroupProfile: reqGroupProfileResult.value,
      });
    }

    receivedGroupRequests.value = _receivedGroupRequests;
  });
}

async function loadPosts() {
  let timeStart = null;
  if (posts.value.length != 0) {
    timeStart = posts.value[0].time;
  }

  return utils.api.post.getLatestPosts(values.state.userProfile.id, 20, timeStart, null).then(apiResult => {
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

async function sendPost(content) {
  return utils.api.post.sendPost(content).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast("已发送");
      postInput.value = '';
    } else {
      utils.tool.toast(apiResult.message);
    }
  }).then(() => {
    return loadPosts();
  });
}

async function loadPageValues() {
  await loadUserProfile();

  await loadFriends();
  await loadGroups();

  await loadSentFriendRequests();
  await loadReceivedFriendRequests();

  await loadSentGroupRequests();
  await loadReceivedGroupRequests();

  await loadPosts();
}

async function acceptFriendRequest(requestId) {
  const apiResult = await utils.api.request.acceptFriendRequest(requestId);

  if (apiResult.isOk) {
    utils.tool.toast("已接受好友请求");
    loadFriends();
    loadReceivedFriendRequests();
  } else {
    utils.tool.toast(apiResult.message);
  }
}

async function acceptGroupRequest(requestId) {
  const apiResult = await utils.api.request.acceptGroupRequest(requestId);

  if (apiResult.isOk) {
    utils.tool.toast("已接受群组请求");
    loadGroups();
    loadReceivedGroupRequests();
  } else {
    utils.tool.toast(apiResult.message);
  }
}

async function deleteFriend(friendUserId) {
  await utils.api.user.deleteFriend(friendUserId).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast("已删除好友");
      loadFriends();
    } else {
      utils.tool.toast(apiResult.message);
    }
  })
}

async function leaveGroup(groupId) {
  await utils.api.group.leaveGroup(groupId).then(apiResult => {
    if (apiResult.isOk) {
      utils.tool.toast("已离开群组");
      loadGroups();
    } else {
      utils.tool.toast(apiResult.message);
    }
  })
}

loadPageValues();

</script>