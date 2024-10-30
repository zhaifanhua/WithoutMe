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
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables" as variables;

  .footer {
    grid-area: footer;
    width: 100%;
    height: variables.$base-footer-height;
    position: fixed;
    left: 0;
    bottom: 0;

    @include mixins.useFlexBox;
    @include mixins.useZindex(overlay);
    @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
    @include mixins.useBorderRadius(variables.$base-border-radius, variables.$base-border-radius, 0, 0);
    @include themes.useTheme {
      background-color: themes.getVar(bg-color);
      border-top: variables.$base-border-width variables.$base-border-style themes.getVar(bg-color-hover);
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
