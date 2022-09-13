<script setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ButtonTM from './ButtonTM.vue';
import { useSideBarStore } from '../stores/sideBar';

const emit = defineEmits(['sideBarButtonClicked'])
const sideBarStore = useSideBarStore();

function sideBarButtonClicked() {
    emit('sideBarButtonClicked')
}
</script>

<template>
    <nav>
        <ButtonTM @clicked-event="sideBarButtonClicked">
            <font-awesome-icon icon="fa-solid fa-bars" size="2x" v-if="sideBarStore.isCollapsed" />
            <font-awesome-icon icon="fa-solid fa-arrow-left" size="2x" v-else />
        </ButtonTM>
    </nav>
</template>

<style>
nav {
    height: 50px;
    background-color: rgb(36 36 36);
    /* background: rgb(174, 238, 200);
    background: -moz-linear-gradient(90deg, rgba(174, 238, 200, 1) 0%, rgba(167, 222, 226, 1) 68%, rgba(148, 170, 233, 1) 100%);
    background: -webkit-linear-gradient(90deg, rgba(174, 238, 200, 1) 0%, rgba(167, 222, 226, 1) 68%, rgba(148, 170, 233, 1) 100%);
    background: linear-gradient(90deg, rgba(174, 238, 200, 1) 0%, rgba(167, 222, 226, 1) 68%, rgba(148, 170, 233, 1) 100%);
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#aeeec8", endColorstr="#94aae9", GradientType=1); */
    display: flex;
    align-items: center;
    padding-left: 5px;
}
</style>