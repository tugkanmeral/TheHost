<script setup>
import { ref, watch } from 'vue';
import { RouterView } from 'vue-router'

import router from './router';
import { useUserStore } from './stores/user';
import { useSideBarStore } from './stores/sideBar';

import NavTM from './components/NavTM.vue';
import SideBarTM from './components/SideBarTM.vue';

const sideBarCollapsed = ref(true);

const userStore = useUserStore();
const sideBarStore = useSideBarStore();

function changeCollapseState() {
  sideBarStore.switch();
}

watch(router.currentRoute, (val) => {
  if (!userStore.isLoggedIn) {
    userStore.removeToken();
    router.replace({ path: '/login' })
  }
})
</script>

<template>
  <title>{{router.currentRoute.value.name}}</title>
  <NavTM @sideBarButtonClicked="() => changeCollapseState()" />
  <div id="mid-body">
    <SideBarTM :collapsed=sideBarStore.isCollapsed />
    <main>
      <RouterView />
    </main>
  </div>

</template>
