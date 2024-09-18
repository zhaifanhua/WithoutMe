<template>
  <AppProvider>
    <DefaultLayout>
      <RouterView></RouterView>
    </DefaultLayout>
  </AppProvider>
</template>

<script setup lang="ts">
  import { onMounted } from 'vue';
  import { RouterView } from 'vue-router';
  import AppProvider from '@/components/AppProvider.vue';
  import DefaultLayout from '@/layouts/DefaultLayout.vue';
  import { initConsole } from '@/utils/app/appInfo';

  onMounted(() => {
    initConsole();
  });
</script>

<style lang="scss">
  @import '@/styles/app.scss';
</style>
