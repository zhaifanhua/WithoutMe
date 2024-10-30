<!-- 全屏切换组件 -->

<template>
  <div id="fullscreen-switch" class="fullscreen-switch">
    <button @click="toggleFullScreen" title="Switch fullScreen">
      <Icon :icon="fullScreenIcon" />
    </button>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed } from "vue";
  import { Icon } from "@iconify/vue";

  const isFullScreen = ref(false);

  enum FullScreenKey {
    True,
    False,
  }

  const fullScreenIcon = computed(() => {
    const fullScreenIconMap = {
      [FullScreenKey.True]: "tdesign:fullscreen-1",
      [FullScreenKey.False]: "tdesign:fullscreen-2",
    };
    return fullScreenIconMap[isFullScreen.value ? FullScreenKey.True : FullScreenKey.False];
  });

  const toggleFullScreen = () => {
    if (!isFullScreen.value) {
      if (document.documentElement.requestFullscreen) {
        document.documentElement.requestFullscreen();
      }
    } else {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      }
    }
  };

  document.addEventListener("fullscreenchange", () => {
    isFullScreen.value = document.fullscreenElement !== null;
  });
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;

  .fullscreen-switch {
    grid-area: fullscreen-switch;
    width: 30px;
    height: 30px;
    @include mixins.useFlexBox;
    @include mixins.useTransition;

    button {
      border: none;
      border-radius: 30%;
      background: transparent;
      cursor: pointer;
      width: 26px;
      height: 26px;
      @include mixins.useFlexBox;
      @include themes.useTheme {
        color: themes.getVar(text-color);
      }

      &:hover {
        @include themes.useTheme {
          background-color: themes.getVar(btn-color-hover);
        }
      }
    }
  }
</style>
