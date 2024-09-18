<template>
  <!-- header-start -->
  <header id="header" class="header">
    <ContentContainer class="header-container">
      <LogoContainer />
      <SubtitleContainer />
      <SwitchContainer />
    </ContentContainer>
  </header>
  <!-- header-end -->
</template>

<script setup lang="ts">
  import ContentContainer from '@/components/generic/ContentContainer.vue';
  import LogoContainer from '@/components/layouts/header/LogoContainer.vue';
  import SubtitleContainer from '@/components/layouts/header/SubtitleContainer.vue';
  import SwitchContainer from '@/components/layouts/header/SwitchContainer.vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .header {
    grid-area: header;
    width: 100%;
    height: $base-header-height;
    position: fixed;
    left: 0;
    top: 0;

    @include useFlexBox;
    @include useZindex(overlay);
    @include useBackdropFilter($base-bg-filter-blur);
    @include useBorderRadius(0, 0, $base-border-radius, $base-border-radius);
    @include useTheme {
      background-color: getVar(bg-color);
      border-bottom: $base-border-width $base-border-style getVar(bg-color-hover);
    }

    // 子元素布局
    .header-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto auto auto auto;
      grid-template-areas: 'logo-container subtitle-container switch-container';
    }
  }
</style>
