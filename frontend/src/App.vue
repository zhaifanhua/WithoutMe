<template>
  <AppProvider>
    <DefaultLayout>
      <RouterView></RouterView>
    </DefaultLayout>
  </AppProvider>
</template>

<script setup lang="ts">
  import { onMounted } from "vue";
  import { RouterView } from "vue-router";
  import AppProvider from "/@/components/AppProvider.vue";
  import DefaultLayout from "/@/layouts/DefaultLayout.vue";
  import { initConsole } from "/@/utils/app/appInfo";
  import { generateFingerprint, generateStableFingerprint } from "@xihan-ui/utils";

  onMounted(async () => {
    initConsole();

    // 生成指纹
    const fingerprint = await generateFingerprint();
    console.log("指纹:", fingerprint);
    // 生成稳定指纹
    const stableFingerprint = await generateStableFingerprint();
    console.log("稳定指纹:", stableFingerprint);
  });
</script>

<style lang="scss">
  @use "/@/styles/app" as app;
</style>
