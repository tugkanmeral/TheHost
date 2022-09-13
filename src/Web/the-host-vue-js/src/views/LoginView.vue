<script setup>
import { ref } from 'vue';
import FreeTextTm from '../components/input/FreeTextTm.vue';
import ButtonTM from '../components/ButtonTM.vue';
import { useUserStore } from '../stores/user'
import CartTM from '../components/container/CartTM.vue'
import router from '../router';

const username = ref('')
const password = ref('')

const userStore = useUserStore();

if (userStore.isLoggedIn) {
    router.replace({ path: '/' })
}

function usernameChanged(_val) {
    username.value = _val;
}
function passwordChanged(_val) {
    password.value = _val;
}

function login() {
    fetch("http://localhost:5001/api/auth/getToken", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Username: username.value,
            Password: password.value
        })
    }).then(async (response) => {
        const data = await response.text();
        userStore.setToken(data)
        router.replace({ path: '/' })
    }).catch((err) => {
        console.error(err);
    })
}
</script>

<template>
    <CartTM>
        <FreeTextTm placeholder="username" @value-changed="(val) => usernameChanged(val)" class="mb-5" />
        <FreeTextTm placeholder="password" @value-changed="(val) => passwordChanged(val)" type="password"
            class="mb-5" />
        <ButtonTM text="Login" v-on:clicked-event="login" />
    </CartTM>
</template>