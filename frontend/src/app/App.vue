<template>
  <AppProvider>
    <DefultLayout>
      <RouterView></RouterView>
    </DefultLayout>
  </AppProvider>
</template>

<script setup lang="ts">
  import { onMounted } from 'vue';
  import { RouterView } from 'vue-router';
  import AppProvider from '@/components/AppProvider.vue';
  import DefultLayout from '@/layouts/DefultLayout.vue';
  import { initConsole } from '@/utils/app/infoTips';
  import '@/utils/app/autoUpdate';

  onMounted(() => {
    initConsole();
  });
</script>

<style lang="scss">
  @import '@/styles/app.scss';
</style>
