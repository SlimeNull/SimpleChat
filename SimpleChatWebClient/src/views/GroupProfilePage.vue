<template>
  <v-main>
    <v-container>
      <v-card>
        <v-card-text>
          <div class="d-flex align-start">
            <div>
              <v-avatar size="x-large">
                <v-img :src="utils.info.getAvatar(groupProfile.avatar)"></v-img>
              </v-avatar>
            </div>
            <div class="ms-4">
              <div class="text-h6">{{ groupProfile.name }}
              </div>
              <div>{{ groupProfile.id }}</div>
            </div>
          </div>
          <div class="mt-8">
            {{ groupProfile.description || "群主很懒，什么都没有留下" }}
          </div>
          <div class="mt-4">
            <v-btn icon @click="sendGroupRequest(groupProfile.id)">
              <v-icon color="primary">mdi-chat</v-icon>
              <v-tooltip activator="parent">聊天</v-tooltip>
            </v-btn>
          </div>
        </v-card-text>
      </v-card>

      <v-card class="mt-4">
        <v-card-title>群管理员</v-card-title>
        <v-card-text>
          <v-list>
            <v-list-item v-for="member in groupMembersIsAdmin" :key="member.id"
              :title="utils.info.getUserFullDisplayName(member.userName, member.nickname)" :subtitle="member.id">

              <template v-slot:prepend>
                <v-btn icon @click="utils.router.goToUserProfile(member.id)" class="me-3">
                  <v-avatar>
                    <v-img :src="utils.info.getAvatar(member.avatar)"></v-img>
                  </v-avatar>
                </v-btn>
              </template>

              <template v-slot:append v-if="selfIsGroupAdmin">
                <v-btn icon size="small" @click="deleteGroupMember(groupProfile.id, member.id)">
                  <v-icon icon="mdi-account-cancel" color="red" />
                  <v-tooltip activator="parent">踢出群成员</v-tooltip>
                </v-btn>

                <template v-if="values.state.userId == groupProfile.creatorId">
                  <v-btn icon size="small" class="ms-2" @click="utils.router.goToUserProfile(member.id)">
                    <v-icon icon="mdi-account-edit" color="orange"
                      @click="setGroupAdmin(groupProfile.id, member.id, false)" />
                    <v-tooltip activator="parent">取消管理员</v-tooltip>
                  </v-btn>
                </template>
              </template>
            </v-list-item>
          </v-list>
        </v-card-text>
      </v-card>

      <v-card class="mt-4">
        <v-card-title>群成员</v-card-title>
        <v-card-text>
          <v-list>
            <v-list-item v-for="member in groupMembers" :key="member.id"
              :title="utils.info.getUserFullDisplayName(member.userName, member.nickname)" :subtitle="member.id">

              <template v-slot:prepend>
                <v-btn icon @click="utils.router.goToUserProfile(member.id)" class="me-3">
                  <v-avatar>
                    <v-img :src="utils.info.getAvatar(member.avatar)"></v-img>
                  </v-avatar>
                </v-btn>
              </template>

              <template v-slot:append v-if="selfIsGroupAdmin">
                <v-btn icon size="small" @click="deleteGroupMember(groupProfile.id, member.id)">
                  <v-icon icon="mdi-account-cancel" color="red" />
                  <v-tooltip activator="parent">踢出群成员</v-tooltip>
                </v-btn>

                <template v-if="values.state.userId == groupProfile.creatorId">
                  <v-btn icon size="small" class="ms-2" @click="utils.router.goToUserProfile(member.id)">
                    <v-icon icon="mdi-account-edit" color="green"
                      @click="setGroupAdmin(groupProfile.id, member.id, true)" />
                    <v-tooltip activator="parent">设为管理员</v-tooltip>
                  </v-btn>
                </template>
              </template>
            </v-list-item>
          </v-list>
        </v-card-text>
      </v-card>

    </v-container>
  </v-main>
</template>

<script setup>

import { useRoute } from 'vue-router'
import { ref } from 'vue'

import utils from "@/utils";
import values from '@/values';

const selfIsGroupAdmin = ref(false);
const groupProfile = ref({});
const groupMembers = ref([]);
const groupMembersIsAdmin = ref([]);

const route = useRoute();
const id = Number.parseInt(route.params.id);

async function loadGroupProfile() {
  await utils.api.group.getGroupProfile(id).then((apiResult) => {
    if (apiResult.isOk) {
      groupProfile.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function loadGroupMembers() {
  await utils.api.group.getGroupMembers(id).then(apiResult => {
    if (apiResult.isOk) {
      groupMembers.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  })
}

async function loadGroupMembersIsAdmin() {
  await utils.api.group.getGroupMembersIsAdmin(id).then(apiResult => {
    if (apiResult.isOk) {
      groupMembersIsAdmin.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function checkSelfIsGroupAdmin() {
  utils.api.group.checkUserIsGroupAdmin(values.state.userProfile.id).then(apiResult => {
    if (apiResult.isOk) {
      selfIsGroupAdmin.value = apiResult.value;
    } else {
      utils.tool.toast(apiResult.message);
    }
  });
}

async function sendGroupRequest(groupId) {
  let isInGroupResult = await utils.api.group.checkUserInGroup(groupId);
  if (!isInGroupResult.isOk) {
    utils.tool.toast(isInGroupResult.message);
    return;
  }

  let isInGroup = isInGroupResult.value;
  if (!isInGroup) {
    let requestGroupResult = await utils.api.request.requestGroup(groupId);
    if (!requestGroupResult.isOk) {
      utils.tool.toast(requestGroupResult.message);
      return;
    } else {
      utils.tool.toast("已发送加群请求, 等待验证");
    }
  }
}

async function deleteGroupMember(groupId, userId) {
  let deleteGroupMemberResult = await utils.api.group.deleteGroupMember(groupId, userId);
  if (!deleteGroupMemberResult.isOk) {
    utils.tool.toast(deleteGroupMemberResult.message);
    return;
  }

  utils.tool.toast("已删除群成员");
  await loadGroupMembers();
}

async function setGroupAdmin(groupId, userId, enable) {
  let setGroupAdminResult = await utils.api.group.setGroupAdmin(groupId, userId, enable);
  if (!setGroupAdminResult.isOk) {
    utils.tool.toast(setGroupAdminResult.message);
    return;
  }

  utils.tool.toast("已设置为管理员");
  await loadGroupMembersIsAdmin();
}

async function loadPageValues() {
  await loadGroupProfile();
  await loadGroupMembers();
  await loadGroupMembersIsAdmin();

  await checkSelfIsGroupAdmin();
}

loadPageValues();

</script>