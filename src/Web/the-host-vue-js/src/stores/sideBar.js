import { defineStore } from "pinia";

export const useSideBarStore = defineStore({
  id: "sideBar",
  state: () => ({
    collapsed: true,
  }),
  getters: {
    isCollapsed: (state) => state.collapsed,
  },
  actions: {
    collapse() {
      this.collapsed = true;
    },
    expand() {
      this.collapsed = false;
    },
    switch() {
      this.collapsed = !this.collapsed;
    },
  },
});
