<template>
  <!-- header-start -->
  <header id="header" class="header">
    <ContentContainer class="header-container">
      <LogoContainer />
      <SubtitleContainer />
      <LangSwitch />
      <ThemeSwitch />
      <FullScreenSwitch />
    </ContentContainer>
  </header>
  <!-- header-end -->
</template>

<script setup lang="ts">
  import ContentContainer from '@/components/generic/ContentContainer.vue';
  import LogoContainer from '@/components/layouts/header/LogoContainer.vue';
  import SubtitleContainer from '@/components/layouts/header/SubtitleContainer.vue';
  import LangSwitch from '@/components/layouts/header/LangSwitch.vue';
  import ThemeSwitch from '@/components/layouts/header/ThemeSwitch.vue';
  import FullScreenSwitch from '@/components/layouts/header/FullScreenSwitch.vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .header {
    grid-area: header;
    padding: 5px 0;
    width: 100%;
    @include useBackdropFilter($base-module-bg-filter-blur);
    @include useFlexBox;
    @include useZindex(overlay);
    // @include useTransition;
    @include useBorderRadius(0, 0, $base-border-radius-lg, $base-border-radius-lg);
    @include useTheme {
      background-color: getVar(module-bg-color);
      border-bottom: $base-border-width $base-border-style getVar(module-bg-color-hover);
    }

    // 子元素布局
    .header-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto auto auto auto;
      grid-template-areas: 'logo-container subtitle-container lang-switch theme-switch fullscreen-switch';
      place-items: center;
    }
  }
</style>
