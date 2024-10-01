<!-- 时间显示组件 -->

<template>
  <div class="time-display">
    <span class="current-time">{{ formatTime(currentTime) }}</span>
    <span class="total-time">{{ formatTime(duration) }}</span>
  </div>
</template>

<script setup lang="ts">
  import { defineProps } from "vue";

  const props = defineProps<{
    currentTime: number;
    duration: number;
  }>();

  function formatTime(time: number): string {
    const minutes = Math.floor(time / 60);
    const seconds = Math.floor(time % 60);
    return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
  }
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .time-display {
    display: flex;
    justify-content: space-between;
    font-size: 12px;
    color: $text-color-secondary;
  }
</style>
