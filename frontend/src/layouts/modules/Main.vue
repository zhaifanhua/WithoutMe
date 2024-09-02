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
  import ContentContainer from '@/components/generic/ContentContainer.vue';
  import Navbar from '@/components/layouts/main/Navbar.vue';
  import MainContent from '@/components/layouts/main/MainContent.vue';
  import Sidebar from '@/components/layouts/main/Sidebar.vue';
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .main {
    grid-area: main;
    padding: 10px 0;
    width: 100%;
    height: 100%;
    @include useBackdropFilter($base-bg-filter-blur);
    @include useFlexBox(row, center, start, center);
    @include useZindex;
    // @include useTransition;
    @include useBorderRadius;
    @include useTheme {
      background-color: getVar(bg-color);
    }

    // 子元素布局
    .main-container {
      display: grid;
      grid-template-rows: 1fr;
      grid-template-columns: auto 1fr auto;
      grid-template-areas: 'navbar main-content sidebar';
      place-items: center;
    }
  }
</style>
