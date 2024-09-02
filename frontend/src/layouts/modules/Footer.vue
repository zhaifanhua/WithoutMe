<template>
  <!-- footer-start -->
  <footer id="footer" class="footer">
    <ContentContainer class="footer-container">
      <BlessContainer />
      <SiteInfoContainer />
      <AppInfoContainer />
    </ContentContainer>
  </footer>
  <!-- footer-end -->
</template>

<script setup lang="ts">
  import ContentContainer from '@/components/generic/ContentContainer.vue';
  import BlessContainer from '@/components/layouts/footer/BlessContainer.vue';
  import SiteInfoContainer from '@/components/layouts/footer/SiteInfoContainer.vue';
  import AppInfoContainer from '@/components/layouts/footer/AppInfoContainer.vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .footer {
    grid-area: footer;
    padding: 5px 0;
    width: 100%;
    @include useBackdropFilter($base-module-bg-filter-blur);
    @include useFlexBox;
    @include useZindex(overlay);
    // @include useTransition;
    @include useBorderRadius($base-border-radius-lg, $base-border-radius-lg, 0, 0);
    @include useTheme {
      background-color: getVar(module-bg-color);
      border-top: $base-border-width $base-border-style getVar(module-bg-color-hover);
    }

    // 子元素布局
    .footer-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto;
      grid-template-areas: 'site-info-container bless-container app-info-container';
      place-items: center;
    }
  }
</style>
