import { defineStore } from "pinia";

const tokenInLocalStorage = localStorage.getItem("token");

export const useUserStore = defineStore({
  id: "user",
  state: () => ({
    token: tokenInLocalStorage,
  }),
  getters: {
    tokenGetter: (state) => state.token,
    isLoggedIn: (state) => (state.token ? true : false),
  },
  actions: {
    setToken(_token) {
      localStorage.setItem("token", _token);
      this.$state.token = _token;
    },
    removeToken() {
      localStorage.removeItem("token");
      this.$state.token = null;
    },
  },
});
