<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
    placeholder: String,
    type: String
})

const emit = defineEmits(['valueChanged'])

const text = ref('')

watch(text, (val) => {
    emit('valueChanged', val)
})

</script>

<template>
    <input title="tminput" class="tm-input" v-model="text" :type="props.type ? props.type : 'text'"
        :placeholder="props.placeholder" />
</template>

<style scoped>
.tm-input {
    border: 2px solid var(--tm-color-dark);
    border-radius: var(--tm-border-radius-md);
    padding: 5px;
}
</style>