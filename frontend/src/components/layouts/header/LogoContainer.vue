<!-- 图标容器组件 -->

<template>
  <div id="logo-container" class="logo-container">
    <a href="/">
      <img class="avatar" :src="siteLogo" alt="logo" />
    </a>
  </div>
</template>

<script setup lang="ts">
  import { ref } from "vue";

  const env = import.meta.env;
  const siteLogo = ref(env.VITE_SITE_LOGO);
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";

  .logo-container {
    grid-area: logo-container;
    @include useFlexBox;

    .avatar {
      height: 68px;
      width: 68px;
      border-radius: 50%;
      object-fit: cover;
    }
  }
</style>
