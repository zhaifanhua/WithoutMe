<template>
  <!-- main-start -->
  <main id="main" class="main">
    <ContentContainer class="main-container">
      <Navbar />
      <MainContent>
        <slot></slot>
      </MainContent>
      <Sidebar />
    </ContentContainer>
  </main>
  <!-- main-end -->
</template>

<script setup lang="ts">
  import ContentContainer from "@/components/generic/ContentContainer.vue";
  import Navbar from "@/components/layouts/main/Navbar.vue";
  import MainContent from "@/components/layouts/main/MainContent.vue";
  import Sidebar from "@/components/layouts/main/Sidebar.vue";
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables" as variables;

  .main {
    grid-area: main;
    padding: variables.$base-gap-xs 0;
    width: 100%;

    @include mixins.useFlexBox(row, center, start, center);
    @include mixins.useZindex;
    @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
    @include mixins.useBorderRadius;
    @include themes.useTheme {
      background-color: themes.getVar(bg-color);
    }

    // 子元素布局
    .main-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto;
      grid-template-areas: "navbar main-content sidebar";
    }
  }
</style>
