<!-- 应用信息容器组件 -->

<template>
  <div id="app-info-container" class="app-info-container">
    <p v-if="appVersion">
      <span>The Current Version:</span>
      <span>&nbsp;</span>
      <a :href="appVersion.homepage" target="_blank"> v{{ appVersion.version }}_{{ appVersion.revisionDate }} </a>
    </p>
    <p v-if="appDevelopeer">
      <span>Designed and developed by:</span>
      <span>&nbsp;</span>
      <a :href="appDevelopeer.url" target="_blank">
        {{ appDevelopeer.name }}
      </a>
    </p>
  </div>
</template>

<script setup lang="ts">
  import { ref } from "vue";
  import { appInfo } from "@/utils/app/appInfo";

  const appVersion = ref({
    version: appInfo.version,
    revisionDate: appInfo.revisionDate,
    homepage: appInfo.homepage,
  });
  const appDevelopeer = ref(appInfo.author);
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;

  .app-info-container {
    grid-area: app-info-container;
    @include mixins.useFlexBox(column, center, center, center);
    @include themes.useTheme {
      color: themes.getVar(text-color-secondary-translucent);
    }

    p {
      font-size: 12px;
      justify-content: center;
      @include mixins.useFlexBox;
      a {
        color: inherit;
        text-decoration: none;

        &:hover {
          color: inherit;
          text-decoration: underline;
        }
      }
    }
  }
</style>
