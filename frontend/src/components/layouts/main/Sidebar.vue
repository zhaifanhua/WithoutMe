<template>
  <!-- sidebar-start -->
  <aside id="sidebar" class="sidebar">
    <div> 广告位 </div>
  </aside>
  <!-- sidebar-end -->
</template>

<script setup lang="ts">
  import { Icon } from '@iconify/vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .sidebar {
    grid-area: sidebar;
    margin-left: $base-gap-sm;
    width: $base-aside-width;
    height: 500px;
    // todo 暂定，后续需要调整为下层组件
    // @include useBackdropFilter($base-module-bg-filter-blur);
    @include useZindex(overlay);
    @include useBorderRadius;
    @include useTheme {
      background-color: getVar(module-bg-color);
    }

    div {
      @include useFlexBox;
    }
  }
</style>
