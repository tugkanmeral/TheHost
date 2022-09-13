<script setup>
const emit = defineEmits(['clickedEvent'])
const props = defineProps({
    textColor: String,
    backgroundColor: String,
    width: String,
    icon: String,
    iconSize: String,
    text: String
})

function buttonClicked() {
    emit('clickedEvent')
}
</script>

<template>
    <div @click="buttonClicked" class="tm-button noselect" :style="{
        color: (props.textColor ? props.textColor : '#e7e7e7'),
        backgroundColor: (props.backgroundColor ? props.backgroundColor : '#242424'),
        width: (props.width ? props.width : 'fit-content')
    }">
        <div v-if="props.icon">
            <font-awesome-icon style="margin-right: 5px;" :icon="props.icon ? props.icon : ''"
                :size="props.iconSize ? props.iconSize : '1x'" />
        </div>
        <slot>
        </slot>
        {{ props.text }}
    </div>
</template>

<style scoped>
.tm-button {
    font-size: large;
    padding: 5px 15px;
    border-radius: var(--tm-border-radius-sm);
}

.tm-button:hover {
    cursor: pointer;
}

.tm-button:active {
    transform: translateY(1px);
}
</style>