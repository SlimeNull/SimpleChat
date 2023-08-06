import Vuex from 'vuex'

export default new Vuex.Store({
  state: {
    JWT: "",
    userId: -1,
    userName: "",
    userProfile: null,
    userIsAdmin: false,

    globalDialog: false,
    globalDialogText: "",
  }
});