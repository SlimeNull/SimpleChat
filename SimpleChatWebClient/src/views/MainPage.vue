<template>
  <v-navigation-drawer permanent>
    <v-list nav>
      <v-list-subheader>Friends</v-list-subheader>
      <v-list-item v-for="friend in friends" :key="friend.id" :prepend-avatar="utils.info.getAvatar(friend.avatar)"
        @click="selectFriend(friend.id)">

        <v-list-item-title>
          {{ utils.info.getUserDisplayName(friend.userName, friend.nickname) }}
        </v-list-item-title>
      </v-list-item>

      <v-divider class="mt-5" />
      <v-list-subheader>Groups</v-list-subheader>
      <v-list-item v-for="group in groups" :key="group.id" :title="group.name" :subtitle="group.id"
        :prepend-avatar="utils.info.getAvatar(group.avatar)" @click="selectGroup(group.id)">
      </v-list-item>

      <v-list-item class="mt-2" prepend-icon="mdi-account-multiple-plus" @click="createGroupDialog = true">
        <v-list-item-title>创建群聊</v-list-item-title>
      </v-list-item>
      <v-dialog v-model="createGroupDialog">
        <v-row class="justify-center">
          <v-col cols="12" sm="8" md="6">
            <v-card>
              <v-card-title>
                创建群聊
              </v-card-title>
              <v-card-text>
                <div class="d-flex">
                  <div class="pt-3">
                    <v-avatar size="x-large" class="elevation-1">
                      <v-img :src="utils.info.getAvatar(createGroupDialogModel.avatarPreviewUrl)" />
                    </v-avatar>
                  </div>
                  <v-container class="grow-1">
                    <v-row>
                      <v-col cols="12" md="6">
                        <v-text-field v-model="createGroupDialogModel.name" label="群聊名称" required></v-text-field>
                      </v-col>
                      <v-col cols="12" md="6">
                        <v-text-field v-model="createGroupDialogModel.description" label="群聊描述" required></v-text-field>
                      </v-col>
                      <v-col cols="12">
                        <v-file-input v-model="createGroupDialogModel.avatar" accept="image/*" label="群聊头像" required
                          @update:model-value="createGroupFormChanged()"></v-file-input>
                      </v-col>
                    </v-row>
                  </v-container>
                </div>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="createGroupDialog = false">
                  取消
                </v-btn>
                <v-btn color="blue-darken-1" variant="text" @click="createGroup()">
                  创建
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-dialog>
    </v-list>
  </v-navigation-drawer>

  <v-app-bar flat location="bottom" class="px-2">
    <v-text-field v-model="message" append-icon="mdi-send" single-line class="m-2" @click:append="sendMessage()"
      @keydown.enter="sendMessage()" />

  </v-app-bar>

  <v-app-bar color="white" elevation="1">
    <v-toolbar-title>{{ currentMessageContext.name }}</v-toolbar-title>
  </v-app-bar>

  <v-main>
    <v-col column overflow-hidden>
      <v-row rows="10">
        <v-container fluid class="pa-10" overflow-auto>
          <div v-for="message in currentMessageContext.messages" :key="message.id"
            :class="message.senderId == userProfile.id ? 'mb-3 d-flex flex-row-reverse' : 'mb-3 d-flex flex-row'">
            <v-btn icon @click="utils.router.goToUserProfile(message.senderId)">
              <v-avatar>
                <v-img :src="utils.info.getAvatar(message.avatar)" />
              </v-avatar>
            </v-btn>
            <div class="mx-3 d-flex flex-column">
              <div :class="message.senderId == userProfile.id ? 'align-self-end' : 'align-self-start'">
                <span class="text-caption">{{ message.senderName }}</span>
              </div>
              <v-card :color="message.senderId == userProfile.id ? 'primary' : ''"
                :class="message.senderId == userProfile.id ? 'align-self-end' : 'align-self-start'">
                <v-card-text class="pa-3 px-4">{{ message.message }}</v-card-text>
              </v-card>
            </div>
            <div v-if="userProfile.isAdmin" class="pt-5">
              <v-icon size="x-small" icon="mdi-close" @click="deleteMessage(message.id)"></v-icon>
            </div>
          </div>
        </v-container>
      </v-row>
      <v-row rows="2">
      </v-row>
    </v-col>
  </v-main>
</template>

<script setup>
import { ref, nextTick } from "vue";
import { onBeforeRouteLeave } from "vue-router";
import values from "@/values";
import utils from "@/utils";

const message = ref('');
const friends = ref([]);
const groups = ref([]);
const createGroupDialog = ref(false);
const createGroupDialogModel = ref({
  name: '',
  description: '',
  avatarFiles: null,
  avatarPreviewUrl: null
});

const friendMessageContexts = ref({});
const groupMessageContexts = ref({});
const currentMessageContext = ref({
  id: -1,
  name: '',
  avatar: '',
  messageLoaded: false,
  messages: []
});
const currentMessageContestIsGroup = ref(false);

const userProfile = ref({});
let eventSource = null;

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
  return await utils.api.group.getGroups().then(apiResult => {
    if (apiResult.isOk) {
      groups.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadFriendContexts() {
  for (const friend of friends.value) {
    await utils.api.user.getUserProfile(friend.id).then(apiResult => {
      if (apiResult.isOk) {
        friendMessageContexts.value[friend.id] = {
          id: friend.id,
          name: utils.info.getUserDisplayName(apiResult.value.userName, apiResult.value.nickname),
          avatar: apiResult.value.avatar,
          messageLoaded: false,
          messages: []
        };
      } else {
        utils.tool.toast(apiResult.message);
      }
    });

    console.log(`friend context loaded: ${friend.id}, ${friendMessageContexts.value[friend.id]}`);
  }
}

async function loadGroupContexts() {
  for (const group of groups.value) {
    await utils.api.group.getGroupProfile(group.id).then(apiResult => {
      if (apiResult.isOk) {
        groupMessageContexts.value[group.id] = {
          id: group.id,
          name: apiResult.value.name,
          avatar: apiResult.value.avatar,
          messageLoaded: false,
          messages: []
        };
      } else {
        utils.tool.toast(apiResult.message);
      }
    });

    console.log(`group context loaded: ${group.id}, ${groupMessageContexts.value[group.id]}`);
  }
}

async function loadLatestFriendMessages(userId, count) {
  const context = friendMessageContexts.value[userId];
  let timeStart = null;

  if (context.messages.length > 0) {
    timeStart = context.messages[context.messages.length - 1].time;
  }

  const isBottom = document.scrollingElement.scrollHeight - document.scrollingElement.clientHeight <= document.scrollingElement.scrollTop + 1;

  return utils.api.message.getLatestPrivateMessages(userId, count, timeStart, null).then(async apiResult => {
    if (apiResult.isOk) {
      for (const msg of apiResult.value) {
        msg.senderName = '';
        await utils.api.user.getUserProfile(msg.senderId).then(apiResult => {
          if (apiResult.isOk) {
            msg.senderName = utils.info.getUserDisplayName(apiResult.value.userName, apiResult.value.nickname);
            msg.avatar = apiResult.value.avatar;
          }
        });

        context.messages.push(msg);
      }

      context.messageLoaded = true;

      if (isBottom) {
        nextTick(() => {
          document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight;
        });
      }
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadLatestGroupMessages(groupId, count) {
  const context = groupMessageContexts.value[groupId];
  let timeStart = null;

  if (context.messages.length > 0) {
    timeStart = context.messages[context.messages.length - 1].time;
  }

  const isBottom = document.scrollingElement.scrollHeight - document.scrollingElement.clientHeight <= document.scrollingElement.scrollTop + 1;

  return utils.api.message.getLatestGroupMessages(groupId, count, timeStart, null).then(async apiResult => {
    if (apiResult.isOk) {
      for (const msg of apiResult.value) {
        msg.senderName = '';
        await utils.api.user.getUserProfile(msg.senderId).then(apiResult => {
          if (apiResult.isOk) {
            msg.senderName = utils.info.getUserDisplayName(apiResult.value.userName, apiResult.value.nickname);
            msg.avatar = apiResult.value.avatar;
          }
        });

        context.messages.push(msg);
      }

      context.messageLoaded = true;

      if (isBottom) {
        nextTick(() => {
          document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight;
        });
      }
    } else {
      utils.tool.toast(apiResult.message);
    }
  })
}

async function selectFriend(userId) {
  currentMessageContestIsGroup.value = false;
  currentMessageContext.value = friendMessageContexts.value[userId];
  console.log(`select friend: ${userId}`);

  if (!currentMessageContext.value.messageLoaded) {
    await loadLatestFriendMessages(userId, 20);
  }

  nextTick(() => {
    document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight;
  });
}

async function selectGroup(groupId) {
  currentMessageContestIsGroup.value = true;
  currentMessageContext.value = groupMessageContexts.value[groupId];
  console.log(`select group: ${groupId}`);

  if (!currentMessageContext.value.messageLoaded) {
    await loadLatestGroupMessages(groupId, 20);
  }

  nextTick(() => {
    document.scrollingElement.scrollTop = document.scrollingElement.scrollHeight;
  });
}

async function sendMessage() {
  if (!currentMessageContestIsGroup.value) {
    console.log(`send private message: ${currentMessageContext.value.id}, ${message.value}`);
    utils.api.message.sendPrivateMessage(currentMessageContext.value.id, message.value).then(apiResult => {
      if (apiResult.isOk) {
        message.value = '';
      } else {
        utils.tool.toast(apiResult.message);
      }
    });
  } else {
    console.log(`send group message: ${currentMessageContext.value.id}, ${message.value}`);
    utils.api.message.sendGroupMessage(currentMessageContext.value.id, message.value).then(apiResult => {
      if (apiResult.isOk) {
        message.value = '';
      } else {
        utils.tool.toast(apiResult.message);
      }
    });
  }
}

async function deleteMessage(messageId) {
  if (!currentMessageContestIsGroup.value) {
    utils.api.admin.deletePrivateMessage(messageId).then(apiResult => {
      if (!apiResult.isOk) {
        utils.tool.toast(apiResult.message);
      }
    });
  } else {
    utils.api.admin.deleteGroupMessage(messageId).then(apiResult => {
      if (!apiResult.isOk) {
        utils.tool.toast(apiResult.message);
      }
    });
  }
}

async function createGroupFormChanged() {
  if (createGroupDialogModel.value.avatar.length > 0) {
    const file = createGroupDialogModel.value.avatar[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
      createGroupDialogModel.value.avatarPreviewUrl = reader.result;
    }
  }
}

async function createGroup() {
  if (createGroupDialogModel.value.name == '') {
    utils.tool.toast('群聊名称不能为空');
    return;
  }

  if (createGroupDialogModel.value.description == '') {
    createGroupDialogModel.value.description = '群主太懒了, 连个群聊描述都不写'
  }

  if (createGroupDialogModel.value.avatar.length == 0) {
    utils.tool.toast('群聊头像不能为空');
    return;
  }

  let uploadResult = await utils.api.file.upload(createGroupDialogModel.value.avatar[0]);

  if (!uploadResult.isOk) {
    utils.tool.toast(uploadResult.message);
    return;
  }

  let avatarUrl = uploadResult.value.fileUrl;

  utils.api.group.createGroup(
    createGroupDialogModel.value.name,
    createGroupDialogModel.value.description,
    avatarUrl).then(apiResult => {
      if (apiResult.isOk) {
        createGroupDialog.value = false;
        loadGroups();
      } else {
        utils.tool.toast(apiResult.message);
      }
    });
}

async function loadPageValues() {
  await loadUserProfile();
  await loadFriends();
  await loadGroups();
  await loadFriendContexts();
  await loadGroupContexts();

  if (friends.value.length > 0) {
    await selectFriend(friends.value[0].id);
  }

  console.log('page values loaded');
}

function listenEvents() {
  eventSource = utils.api.event.getEvents();

  eventSource.addEventListener('PrivateMessageSent', function (event) {
    const eventData = JSON.parse(event.data);
    const friendUserId = eventData['FriendUserId'];
    loadLatestFriendMessages(friendUserId, 0);
  });

  eventSource.addEventListener('PrivateMessageReceived', function (event) {
    const eventData = JSON.parse(event.data);
    const friendUserId = eventData['FriendUserId'];
    loadLatestFriendMessages(friendUserId, 0);
  });

  eventSource.addEventListener('GroupMessageSent', function (event) {
    const eventData = JSON.parse(event.data);
    const groupId = eventData['GroupId'];
    loadLatestGroupMessages(groupId, 0);
  });

  eventSource.addEventListener('GroupMessageReceived', function (event) {
    const eventData = JSON.parse(event.data);
    const groupId = eventData['GroupId'];
    loadLatestGroupMessages(groupId, 0);
  });

  eventSource.addEventListener('FriendListChanged', async function () {
    await loadFriends();
    await loadFriendContexts();
  });

  eventSource.addEventListener('GroupListChanged', async function () {
    await loadGroups();
    await loadGroupContexts();
  });

  eventSource.addEventListener('PrivateMessageDeleted', function (event) {
    const eventData = JSON.parse(event.data);
    const messageId = eventData['MessageId'];
    for (const friend of friends.value) {
      let context = friendMessageContexts.value[friend.id];
      context.messages = context.messages.filter(msg => msg.id != messageId);
    }
  });

  eventSource.addEventListener('GroupMessageDeleted', function (event) {
    const eventData = JSON.parse(event.data);
    const messageId = eventData['MessageId'];
    for (const group of groups.value) {
      let context = groupMessageContexts.value[group.id];
      context.messages = context.messages.filter(msg => msg.id != messageId);
    }
  });

  console.log('events listened');
}

onBeforeRouteLeave((to, from, next) => {
  eventSource.close();
  eventSource = null;
  next();
});

loadPageValues();
listenEvents();
</script>