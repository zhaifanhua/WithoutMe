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
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;
  @use "@/styles/base/variables" as variables;

  .default-layout {
    display: grid;
    grid-template-rows: auto 1fr auto;
    grid-template-columns: 1fr;
    grid-template-areas:
      "header"
      "main"
      "footer";
    padding-top: variables.$base-header-height + variables.$base-gap-xs;
    padding-bottom: variables.$base-footer-height + variables.$base-gap-xs;
  }
</style>
