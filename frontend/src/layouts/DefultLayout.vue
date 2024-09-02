<template>
  <div id="defult-layout" class="defult-layout">
    <Header></Header>
    <Main>
      <slot></slot>
    </Main>
    <Footer></Footer>
  </div>
</template>

<script setup lang="ts">
  import Header from '@/layouts/modules/Header.vue';
  import Main from '@/layouts/modules/Main.vue';
  import Footer from '@/layouts/modules/Footer.vue';
  import { useEnhancer } from '@/app/enhancer';

  const { isDarkTheme } = useEnhancer();
</script>

<style scoped lang="scss">
  @import '@/styles/base/mixins.scss';
  @import '@/styles/base/themes.scss';

  .defult-layout {
    display: grid;
    grid-template-rows: auto 1fr auto;
    grid-template-columns: 1fr;
    grid-template-areas:
      'header'
      'main'
      'footer';
    place-items: center;
    height: 100%;
    width: 100%;
    @include useTheme {
      background-color: getVar(bg-color);
    }
  }
</style>
