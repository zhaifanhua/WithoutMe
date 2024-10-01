<!-- 歌曲信息组件 -->

<template>
  <div class="song-info">
    <div class="cover-container">
      <div class="cover" :class="{ rotating: isPlaying }" :style="coverStyle">
        <img :src="song.coverUrl" :alt="song.artist" />
      </div>
    </div>
    <div class="info">
      <div class="name" :title="song.name">{{ song.name }}</div>
      <div class="artist" :title="song.artist">{{ song.artist }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, watch, onMounted } from "vue";

  interface Song {
    name: string;
    artist: string;
    coverUrl: string;
  }

  const props = defineProps<{
    song: Song;
    isPlaying: boolean;
  }>();

  const coverStyle = ref({
    animationPlayState: "paused",
  });

  watch(
    () => props.isPlaying,
    newValue => {
      coverStyle.value.animationPlayState = newValue ? "running" : "paused";
    }
  );

  onMounted(() => {
    coverStyle.value.animationPlayState = props.isPlaying ? "running" : "paused";
  });
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .song-info {
    display: flex;
    align-items: center;
    width: 100%;
    max-width: 200px;

    .cover-container {
      width: 80px; // 增加到 80px
      height: 80px; // 增加到 80px
      border-radius: 50%;
      overflow: hidden;
      margin-right: 15px; // 稍微增加右边距
      flex-shrink: 0;

      .cover {
        width: 100%;
        height: 100%;
        border-radius: 50%;
        overflow: hidden;
        animation: rotate 20s linear infinite;
        animation-play-state: paused;

        img {
          width: 100%;
          height: 100%;
          object-fit: cover;
        }
      }
    }

    .info {
      flex-grow: 1;
      overflow: hidden;

      .name,
      .artist {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .name {
        font-size: 16px; // 稍微增加字体大小
        font-weight: bold;
        color: $text-color-primary;
        margin-bottom: 6px; // 稍微增加底部边距
      }

      .artist {
        font-size: 14px; // 稍微增加字体大小
        color: $text-color-secondary;
      }
    }
  }

  @keyframes rotate {
    from {
      transform: rotate(0deg);
    }
    to {
      transform: rotate(360deg);
    }
  }
</style>
