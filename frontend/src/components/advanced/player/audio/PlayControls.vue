<!-- 播放控制组件 -->

<template>
  <div class="control">
    <button @click="emitTogglePlayMode" :title="modeTitle">
      <Icon :icon="modeIcon" />
    </button>
    <button @click="emitPrevSong" :title="'上一首'">
      <Icon :icon="'solar:skip-previous-bold'" />
    </button>
    <button @click="emitTogglePlay" :class="{ playing: isPlaying }" :title="playPauseTitle">
      <Icon :icon="playPauseIcon" />
    </button>
    <button @click="emitNextSong" :title="'下一首'">
      <Icon :icon="'solar:skip-next-bold'" />
    </button>
    <button @click="emitToggleMute" :class="{ muted: isMuted || volume === 0 }" :title="volumeTitle">
      <Icon :icon="volumeIcon" />
    </button>
  </div>
</template>

<script setup lang="ts">
  import { computed, defineProps, defineEmits } from "vue";
  import { Icon } from "@iconify/vue";

  enum PlayMode {
    Sequential = "sequential",
    Random = "random",
    SingleLoop = "singleLoop",
    ListLoop = "listLoop",
  }

  const props = defineProps<{
    isPlaying: boolean;
    playMode: PlayMode;
    isMuted: boolean;
    volume: number;
  }>();

  const emit = defineEmits<{
    (e: "toggle-play"): void;
    (e: "prev-song"): void;
    (e: "next-song"): void;
    (e: "toggle-play-mode"): void;
    (e: "toggle-mute"): void;
  }>();

  function emitTogglePlay() {
    emit("toggle-play");
  }

  function emitPrevSong() {
    emit("prev-song");
  }

  function emitNextSong() {
    emit("next-song");
  }

  function emitTogglePlayMode() {
    emit("toggle-play-mode");
  }

  function emitToggleMute() {
    emit("toggle-mute");
  }

  const modeIcon = computed(() => {
    switch (props.playMode) {
      case PlayMode.Sequential:
        return "solar:reorder-outline";
      case PlayMode.Random:
        return "solar:shuffle-linear";
      case PlayMode.SingleLoop:
        return "solar:repeat-one-linear";
      case PlayMode.ListLoop:
        return "solar:repeat-linear";
    }
  });

  const modeTitle = computed(() => {
    switch (props.playMode) {
      case PlayMode.Sequential:
        return "顺序播放";
      case PlayMode.Random:
        return "随机播放";
      case PlayMode.SingleLoop:
        return "单曲循环";
      case PlayMode.ListLoop:
        return "列表循环";
    }
  });

  const playPauseIcon = computed(() => (props.isPlaying ? "solar:pause-bold" : "solar:play-bold"));
  const playPauseTitle = computed(() => (props.isPlaying ? "暂停" : "播放"));

  const volumeIcon = computed(() => (props.isMuted || props.volume === 0 ? "solar:muted-linear" : "solar:volume-linear"));
  const volumeTitle = computed(() => (props.isMuted || props.volume === 0 ? "静音" : "音量"));
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .control {
    display: flex;
    justify-content: space-between;
    align-items: center;

    button {
      background: none;
      border: none;
      font-size: 16px;
      color: $text-color-secondary;
      cursor: pointer;

      &:hover {
        color: $text-color-primary;
      }
      &.playing {
        color: $text-color-active;
      }
    }
  }
</style>
