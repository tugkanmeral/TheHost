<script setup>
import WelcomeItem from './WelcomeItem.vue'
import DocumentationIcon from './icons/IconDocumentation.vue'
import { UiButton } from 'balm-ui'
</script>

<template>
  <ui-button icon="favorite">Icon</ui-button>
  <UiButton unelevated>unelevated</UiButton>
  <WelcomeItem>
    <template #icon>
      <DocumentationIcon />
    </template>
    <template #heading>Documentation</template>

    Vue’s
    <a target="_blank" href="https://vuejs.org/">official documentation</a>
    provides you with all information you need to get started.
  </WelcomeItem>
</template>
