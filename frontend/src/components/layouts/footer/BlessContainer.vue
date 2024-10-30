<!-- 祝语容器组件 -->

<template>
  <div id="bless-container" class="bless-container">
    <p v-if="bless">
      <span>{{ bless }}</span>
    </p>
  </div>
</template>

<script setup lang="ts">
  import { ref } from "vue";
  import { siteBaseInfo } from "@/utils/app/siteInfo";

  const bless = ref(siteBaseInfo.bless);
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;

  .bless-container {
    grid-area: bless-container;
    @include mixins.useFlexBox;
    @include themes.useTheme {
      color: themes.getVar(text-color-secondary-opaque);
    }

    span {
      font-weight: 400;
      font-size: 20px;
      letter-spacing: 0.8em;
    }
  }
</style>
