<template>
  <!-- main-content-start -->
  <div id="main-content" class="main-content">
    <slot></slot>
  </div>
  <!-- main-content-end -->
</template>

<script setup lang="ts">
  import { Icon } from '@iconify/vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .main-content {
    grid-area: main-content;
    width: 100%;
    height: 100%;
    @include useZindex;
    @include useBorderRadius;
    // todo 暂定，后续需要调整为下层组件
    // @include useBackdropFilter($base-module-bg-filter-blur);
    @include useTheme {
      background-color: getVar(module-bg-color);
    }
  }
</style>
