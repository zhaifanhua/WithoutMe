<template>
  <!-- main-content-start -->
  <div id="main-content" class="main-content">
    <slot></slot>
  </div>
  <!-- main-content-end -->
</template>

<script setup lang="ts">
  import { Icon } from "@iconify/vue";
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;

  .main-content {
    grid-area: main-content;
    width: 100%;
    height: 100%;
    @include mixins.useZindex;
    @include mixins.useBorderRadius;
    // todo 暂定，后续需要调整为下层组件
    // @include mixins.useBackdropFilter(variables.$base-module-bg-filter-blur);
    @include themes.useTheme {
      background-color: themes.getVar(module-bg-color);
    }
  }
</style>
