import values from '@/values';
import config from '@/config';
import router from '@/router';
import EventSourcePolyfill from 'eventsource';

function apiFetch(url, options) {
  return fetch(`${config.API_HOST}${url}`, {
    method: "POST",
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(options)
  }).then(res => {
    if (res.status == 200) {
      return res.json();
    } else {
      throw new Error('Fetch failed');
    }
  }).catch(err => {
    values.state.globalDialogText = err.message;
    values.state.globalDialog = true;
  });
}

function apiFetchWithAuth(url, options) {
  return fetch(`${config.API_HOST}${url}`, {
    method: "POST",
    headers: {
      'Content-Type': 'application/json',
      "Authorization": `Bearer ${values.state.JWT}`
    },
    body: JSON.stringify(options)
  }).then(res => {
    if (res.status == 200) {
      return res.json();
    } else {
      throw new Error(`Fetch failed, ${res.status} ${res.statusText}`);
    }
  }).catch(err => {
    values.state.globalDialogText = err.message;
    values.state.globalDialog = true;
  });
}

export default {

  info: {
    getSelfAvatar: function () {
      if (values.state.userProfile) {
        let avatar = values.state.userProfile.avatar;
        if (!avatar.startsWith('http') && !avatar.startsWith('data')) {
          avatar = `${config.API_HOST}${avatar}`;
        }

        return avatar;
      }

      return `${config.API_HOST}/static/img/avatar_default.jpeg`;
    },

    getAvatar(avatarAddress) {
      if (avatarAddress) {
        if (!avatarAddress.startsWith('http') && !avatarAddress.startsWith('data')) {
          avatarAddress = `${config.API_HOST}${avatarAddress}`;
        }
        return avatarAddress;
      }

      return `${config.API_HOST}/static/img/avatar_default.jpeg`;
    },

    getUserDisplayName(userName, userNickname) {
      if (userNickname) {
        return userNickname;
      }

      return userName;
    },

    getUserFullDisplayName(userName, userNickname) {
      if (userNickname) {
        return `${userNickname} (${userName})`;
      }

      return userName;
    }
  },

  tool: {
    toast(content) {
      values.state.globalDialogText = content;
      values.state.globalDialog = true;
    },

    getEmptyUserProfile() {
      return {
        id: -1,
        userName: '',
        nickname: '',
        description: '',
      }
    }
  },

  router: {
    goToUserProfile(userId) {
      router.push(`/profile/${userId}`);
    },

    goToGroupProfile(groupId) {
      router.push(`/groupProfile/${groupId}`);
    },

    goToMain() {
      router.push("/main");
    },

    goToSearch() {
      router.push("/search");
    },

    goToMyProfile() {
      router.push("/myprofile");
    },

    goToAuth() {
      router.push("/auth");
    },
  },

  api: {
    auth: {
      login(username, password) {
        return apiFetch('/api/auth/login', { username, password });
      },

      register(username, password) {
        return apiFetch('/api/auth/register', { username, password });
      },

      superAdminLogin(username, password) {
        return apiFetch('/api/auth/superAdminLogin', { username, password });
      }
    },

    manage: {
      activeUser(userId) {
        return apiFetchWithAuth('/api/manage/activeUser', { userId });
      },

      getUsersNeedActive() {
        return apiFetchWithAuth('/api/manage/getUsersNeedActive');
      },

      getUsersBanned() {
        return apiFetchWithAuth('/api/manage/getUsersBanned');
      },

      getUsersIsAdmin() {
        return apiFetchWithAuth('/api/manage/getUsersIsAdmin');
      },

      getAllUsers() {
        return apiFetchWithAuth('/api/manage/getAllUsers');
      },

      setUserBan(userId, enable) {
        return apiFetchWithAuth('/api/manage/setUserBan', { userId, enable });
      },

      setUserAdmin(userId, enable) {
        return apiFetchWithAuth('/api/manage/setUserAdmin', { userId, enable });
      }
    },

    message: {
      sendPrivateMessage(userId, message) {
        return apiFetchWithAuth('/api/message/sendPrivateMessage', { userId, message });
      },

      sendGroupMessage(groupId, message) {
        return apiFetchWithAuth('/api/message/sendGroupMessage', { groupId, message });
      },

      getPrivateMessages(friendUserId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/message/getPrivateMessages', { friendUserId, count, timeStart, timeEnd });
      },

      getGroupMessages(groupId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/message/getGroupMessages', { groupId, count, timeStart, timeEnd });
      },

      getLatestPrivateMessages(friendUserId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/message/getLatestPrivateMessages', { friendUserId, count, timeStart, timeEnd });
      },

      getLatestGroupMessages(groupId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/message/getLatestGroupMessages', { groupId, count, timeStart, timeEnd });
      }
    },

    admin: {
      setUserBan(userId, enable) {
        return apiFetchWithAuth('/api/admin/setUserBan', { userId, enable });
      },

      deletePrivateMessage(messageId) {
        return apiFetchWithAuth('/api/admin/deletePrivateMessage', { messageId });
      },

      deleteGroupMessage(messageId) {
        return apiFetchWithAuth('/api/admin/deleteGroupMessage', { messageId });
      }
    },

    request: {
      requestFriend(userId) {
        return apiFetchWithAuth('/api/request/requestFriend', { userId });
      },

      requestGroup(groupId) {
        return apiFetchWithAuth('/api/request/requestGroup', { groupId });
      },

      getSentFriendRequests() {
        return apiFetchWithAuth('/api/request/getSentFriendRequests');
      },

      getReceivedFriendRequests() {
        return apiFetchWithAuth('/api/request/getReceivedFriendRequests');
      },

      getSentGroupRequests() {
        return apiFetchWithAuth('/api/request/getSentGroupRequests');
      },

      getReceivedGroupRequests() {
        return apiFetchWithAuth('/api/request/getReceivedGroupRequests');
      },

      acceptFriendRequest(friendRequestId) {
        return apiFetchWithAuth('/api/request/acceptFriendRequest', { friendRequestId });
      },

      acceptGroupRequest(groupRequestId) {
        return apiFetchWithAuth('/api/request/acceptGroupRequest', { groupRequestId });
      }
    },

    user: {
      getUserProfile(userId) {
        return apiFetchWithAuth('/api/user/getUserProfile', { userId });
      },

      setSelfProfile( nickname, description, avatar) {
        return apiFetchWithAuth('/api/user/setSelfProfile', { avatar, nickname, description });
      },

      getFriends() {
        return apiFetchWithAuth('/api/user/getFriends');
      },

      checkUserIsFriend(userId) {
        return apiFetchWithAuth('/api/user/checkUserIsFriend', { userId });
      },

      searchUsers(name) {
        return apiFetchWithAuth('/api/user/searchUsers', { name });
      },

      deleteFriend(userId) {
        return apiFetchWithAuth('/api/user/deleteFriend', { userId });
      }
    },

    group: {

      getGroupProfile(groupId) {
        return apiFetchWithAuth('/api/group/getGroupProfile', { groupId });
      },

      setGroupProfile(groupId, name, description, avatar) {
        return apiFetchWithAuth('/api/group/setGroupProfile', { id: groupId, name, description, avatar });
      },

      getGroups() {
        return apiFetchWithAuth('/api/group/getGroups');
      },

      getGroupMembers(groupId) {
        return apiFetchWithAuth('/api/group/getGroupMembers', { groupId });
      },

      getGroupMembersIsAdmin(groupId) {
        return apiFetchWithAuth('/api/group/getGroupMembersIsAdmin', { groupId });
      },

      checkUserInGroup(groupId) {
        return apiFetchWithAuth('/api/group/checkUserInGroup', { groupId });
      },

      checkUserIsGroupAdmin(groupId) {
        return apiFetchWithAuth('/api/group/checkUserIsGroupAdmin', { groupId });
      },

      createGroup(name, description, avatar) {
        return apiFetchWithAuth('/api/group/createGroup', { name, description, avatar });
      },

      deleteGroup(groupId) {
        return apiFetchWithAuth('/api/group/deleteGroup', { groupId });
      },

      leaveGroup(groupId) {
        return apiFetchWithAuth('/api/group/leaveGroup', { groupId });
      },

      setGroupAdmin(groupId, userId, enable) {
        return apiFetchWithAuth('/api/group/setGroupAdmin', { groupId, userId, enable });
      },

      deleteGroupMember(groupId, userId) {
        return apiFetchWithAuth('/api/group/deleteGroupMember', { groupId, userId });
      },

      searchGroups(name) {
        return apiFetchWithAuth('/api/group/searchGroups', { name });
      },


    },

    post: {
      sendPost(content) {
        return apiFetchWithAuth('/api/post/sendPost', { content });
      },

      getPosts(userId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/post/getPosts', { userId, count, timeStart, timeEnd });
      },

      getLatestPosts(userId, count, timeStart, timeEnd) {
        return apiFetchWithAuth('/api/post/getLatestPosts', { userId, count, timeStart, timeEnd });
      },
    },

    event: {
      getEvents() {
        return new EventSourcePolyfill(`${config.API_HOST}/api/event`, {
          headers: {
            "Authorization": `Bearer ${values.state.JWT}`
          }
        });
      }
    },

    file: {
      upload(file) {
        const url = `${config.API_HOST}/api/file/upload`;
        const formData = new FormData();
        formData.append("file", file);
      
        return fetch(url, {
          method: "POST",
          headers: {
            "Authorization": `Bearer ${values.state.JWT}`
          },
          body: formData
        }).then(res => {
          if (res.status == 200) {
            return res.json();
          } else {
            throw new Error('Upload failed');
          }
        }).catch(err => {
          values.state.globalDialogText = err.message;
          values.state.globalDialog = true;
        });
      }
    }
  },

  auth: {
    saveAuthInfo(userId, userName, userProfile, JWT) {
      values.state.userId = userId;
      values.state.userName = userName;
      values.state.userProfile = userProfile;
      values.state.JWT = JWT;

      document.cookie = `userId=${userId}`;
      document.cookie = `userName=${userName}`;
      document.cookie = `userProfile=${JSON.stringify(userProfile)}`;
      document.cookie = `JWT=${JWT}`;
    },

    loadAuthInfo() {
      let userIdOk = false;
      let userNameOk = false;
      let userProfileOk = false;
      let JWTOk = false;

      let cookies = document.cookie.split(';');
      for (let cookie of cookies) {
        let [key, value] = cookie.split('=');

        if (key == undefined || value == undefined) {
          continue;
        }

        key = key.trim();
        value = value.trim();

        if (key == 'userId') {
          values.state.userId = parseInt(value);
          userIdOk = true;
        } else if (key == 'userName') {
          values.state.userName = value;
          userNameOk = true;
        } else if (key == 'userProfile') {
          values.state.userProfile = JSON.parse(value);
          userProfileOk = true;
        } else if (key == 'JWT') {
          values.state.JWT = value;
          JWTOk = true;
        }
      }

      return userIdOk && userNameOk && userProfileOk && JWTOk;
    },

    async verifyAuthInfo() {
      try {
        await apiFetchWithAuth('/api/auth/verify');
        return true;
      } catch {
        return false;
      }
    },
  }
}