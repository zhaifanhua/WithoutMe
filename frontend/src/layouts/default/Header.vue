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
  import ContentContainer from "@/components/generic/ContentContainer.vue";
  import LogoContainer from "@/components/layouts/header/LogoContainer.vue";
  import SubtitleContainer from "@/components/layouts/header/SubtitleContainer.vue";
  import SwitchContainer from "@/components/layouts/header/SwitchContainer.vue";
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables" as variables;

  .header {
    grid-area: header;
    width: 100%;
    height: variables.$base-header-height;
    position: fixed;
    left: 0;
    top: 0;

    @include mixins.useFlexBox;
    @include mixins.useZindex(overlay);
    @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
    @include mixins.useBorderRadius(0, 0, variables.$base-border-radius, variables.$base-border-radius);
    @include themes.useTheme {
      background-color: themes.getVar(bg-color);
      border-bottom: variables.$base-border-width variables.$base-border-style themes.getVar(bg-color-hover);
    }

    // 子元素布局
    .header-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto auto auto auto;
      grid-template-areas: "logo-container subtitle-container switch-container";
    }
  }
</style>
