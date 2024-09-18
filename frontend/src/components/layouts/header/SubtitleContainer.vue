<!-- 副标题容器组件 -->

<template>
  <div id="subtitle-container" class="subtitle-container">
    <p v-if="siteSubtitle">
      <span>{{ siteSubtitle }}</span>
    </p>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue';

  const env = import.meta.env;
  const siteSubtitle = ref(env.VITE_SITE_SUBTITLE_CN);
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .subtitle-container {
    grid-area: subtitle-container;
    @include useFlexBox;
    @include useTheme {
      color: getVar(text-color-secondary-opaque);
    }

    span {
      font-weight: 400;
      font-size: 14px;
      letter-spacing: 0.8em;
    }
  }
</style>
