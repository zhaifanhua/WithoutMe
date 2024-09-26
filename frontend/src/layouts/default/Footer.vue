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
  import ContentContainer from "@/components/generic/ContentContainer.vue";
  import BlessContainer from "@/components/layouts/footer/BlessContainer.vue";
  import SiteInfoContainer from "@/components/layouts/footer/SiteInfoContainer.vue";
  import AppInfoContainer from "@/components/layouts/footer/AppInfoContainer.vue";
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .footer {
    grid-area: footer;
    width: 100%;
    height: $base-footer-height;
    position: fixed;
    left: 0;
    bottom: 0;

    @include useFlexBox;
    @include useZindex(overlay);
    @include useBackdropFilter($base-bg-filter-blur);
    @include useBorderRadius($base-border-radius, $base-border-radius, 0, 0);
    @include useTheme {
      background-color: getVar(bg-color);
      border-top: $base-border-width $base-border-style getVar(bg-color-hover);
    }

    // 子元素布局
    .footer-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto auto auto;
      grid-template-areas: "site-info-container bless-container app-info-container";
    }
  }
</style>
