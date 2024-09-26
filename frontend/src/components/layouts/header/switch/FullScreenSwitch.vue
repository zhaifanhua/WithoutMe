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
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .fullscreen-switch {
    grid-area: fullscreen-switch;
    width: 30px;
    height: 30px;
    @include useFlexBox;
    @include useTransition;

    button {
      border: none;
      border-radius: 30%;
      background-color: transparent;
      background: transparent;
      cursor: pointer;
      width: 26px;
      height: 26px;
      @include useFlexBox;
      @include useTheme {
        color: getVar(text-color);
      }

      &:hover {
        @include useTheme {
          background-color: getVar(btn-color-hover);
        }
      }
    }
  }
</style>
