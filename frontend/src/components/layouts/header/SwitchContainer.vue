<!-- 开关容器组件 -->

<template>
  <div id="switch-container" class="switch-container">
    <div class="switch-content">
      <LangSwitch />
      <!-- <ThemeSwitch /> -->
      <FullScreenSwitch />
    </div>
  </div>
</template>

<script setup lang="ts">
  import LangSwitch from "@/components/layouts/header/switch/LangSwitch.vue";
  // import ThemeSwitch from '@/components/layouts/header/switch/ThemeSwitch.vue';
  import FullScreenSwitch from "@/components/layouts/header/switch/FullScreenSwitch.vue";
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;

  .switch-container {
    grid-area: switch-container;
    @include mixins.useFlexBox;

    // 子元素布局
    .switch-content {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto auto auto;
      grid-template-areas: "lang-switch theme-switch fullscreen-switch";
    }
  }
</style>
