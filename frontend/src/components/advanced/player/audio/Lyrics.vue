<!-- 歌词显示组件 -->

<template>
  <div class="lyrics-display" :class="{ active: show }">
    <ul class="lyrics" :style="{ transform: `translateY(${translateY}px)` }">
      <li v-for="(line, index) in lyrics" :key="index" :class="{ active: index === currentLineIndex }">
        {{ line.text }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import { computed } from "vue";

  interface LyricLine {
    time: number;
    text: string;
  }

  const props = defineProps<{
    show: boolean;
    lyrics: LyricLine[];
    currentTime: number;
  }>();

  const lineHeight = 20; // 每行歌词的高度
  const containerHeight = 60; // 歌词容器的高度

  const currentLineIndex = computed(() => {
    return props.lyrics.findIndex((line, index) => {
      const nextLine = props.lyrics[index + 1];
      return props.currentTime >= line.time && (!nextLine || props.currentTime < nextLine.time);
    });
  });

  const translateY = computed(() => {
    const centerPosition = containerHeight / 2;
    const activeLyricPosition = currentLineIndex.value * lineHeight;
    return centerPosition - activeLyricPosition - lineHeight / 2;
  });
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "./_variables.scss";

  .lyrics-display {
    position: absolute;
    top: 100%;
    left: 50%;
    transform: translateX(-50%);
    width: $module-width;
    height: 60px;
    max-height: 0;
    overflow: hidden;
    transition: $transition;
    opacity: 0;
    @include useBackdropFilter($base-bg-filter-blur);
    background-color: rgba($bg-color-secondary, 0.5);
    border-radius: 0 0 $border-radius-modula $border-radius-modula;
    margin-top: 1px;

    &.active {
      max-height: $lyrics-height;
      opacity: 1;
    }

    .lyrics {
      text-align: center;
      transition: transform 0.5s ease;

      li {
        height: 20px;
        line-height: 20px;
        opacity: 0.7;
        transition: all 0.3s ease;
        font-size: 14px;
        color: $text-color-secondary;

        &.active {
          opacity: 1;
          font-weight: bold;
          transform: scale(1.2);
          color: $text-color-active;
        }
      }
    }
  }
</style>
