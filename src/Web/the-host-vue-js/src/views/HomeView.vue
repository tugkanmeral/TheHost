<script setup>
import ButtonTM from '../components/ButtonTM.vue';

function test() {
  console.log('Hi there!')
}
</script>

<template>
  <ButtonTM @clickedEvent="() => test()" :textColor="'#e7e7e7'" :width="'fit-content'">Test</ButtonTM>
</template>
