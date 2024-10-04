<!-- 功能按钮组件 -->

<template>
  <div class="function-buttons">
    <button @click="emitTogglePlaylist" :class="{ active: props.showPlaylist }" :title="'播放列表'">
      <Icon icon="solar:document-text-bold" />
    </button>
    <button @click="emitToggleColorful" :class="{ active: props.showColorful }" :title="'开关炫彩'">
      <Icon icon="ion:color-palette" />
    </button>
    <button @click="emitToggleLyrics" :class="{ active: props.showLyrics }" :title="'开关歌词'">
      <Icon icon="solar:text-square-bold" />
    </button>
  </div>
</template>

<script setup lang="ts">
  import { Icon } from "@iconify/vue";

  const props = defineProps<{
    showColorful: boolean;
    showLyrics: boolean;
    showPlaylist: boolean;
  }>();

  const emit = defineEmits<{
    (e: "toggle-colorful"): void;
    (e: "toggle-lyrics"): void;
    (e: "toggle-playlist"): void;
  }>();

  function emitToggleColorful() {
    emit("toggle-colorful");
  }

  function emitToggleLyrics() {
    emit("toggle-lyrics");
  }

  function emitTogglePlaylist() {
    emit("toggle-playlist");
  }
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .function-buttons {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 20px;
    gap: 10px;

    button {
      background: none;
      border: none;
      font-size: 18px;
      color: $text-color-secondary;
      cursor: pointer;

      &:hover,
      &.active {
        color: $text-color-primary;
      }
    }
  }
</style>
