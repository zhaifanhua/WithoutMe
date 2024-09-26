<template>
  <div id="default-layout" class="default-layout">
    <Header></Header>
    <Main>
      <slot></slot>
    </Main>
    <Footer></Footer>
  </div>
</template>

<script setup lang="ts">
  import Header from "@/layouts/default/Header.vue";
  import Main from "@/layouts/default/Main.vue";
  import Footer from "@/layouts/default/Footer.vue";
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .default-layout {
    display: grid;
    grid-template-rows: auto 1fr auto;
    grid-template-columns: 1fr;
    grid-template-areas:
      "header"
      "main"
      "footer";
    padding-top: $base-header-height + $base-gap-xs;
    padding-bottom: $base-footer-height + $base-gap-xs;
  }
</style>
