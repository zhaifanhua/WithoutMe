<template>
  <!-- sidebar-start -->
  <aside id="sidebar" class="sidebar">
    <div>广告位</div>
  </aside>
  <!-- sidebar-end -->
</template>

<script setup lang="ts">
  import { Icon } from "@iconify/vue";
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables";

  .sidebar {
    grid-area: sidebar;
    margin-left: variables.$base-gap-sm;
    width: variables.$base-aside-width;
    height: 500px;
    // todo 暂定，后续需要调整为下层组件
    // @include mixins.useBackdropFilter(variables.$base-module-bg-filter-blur);
    @include mixins.useZindex(overlay);
    @include mixins.useBorderRadius;
    @include themes.useTheme {
      background-color: themes.getVar(module-bg-color);
    }

    div {
      @include mixins.useFlexBox;
    }
  }
</style>
