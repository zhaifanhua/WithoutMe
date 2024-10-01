<!-- 播放列表组件 -->

<template>
  <div class="playlist" :class="{ active: show }">
    <ul>
      <li v-for="(song, index) in playlist" :key="index" @dblclick="selectSong(index)" :class="{ active: index === currentIndex }">
        <span class="song-index">{{ index + 1 }}</span>
        <span class="song-cover">
          <img :src="song.coverUrl" :alt="song.artist" />
        </span>
        <span class="song-info">
          <span class="song-name">{{ song.name }}</span>
          <span class="song-artist">{{ song.artist }}</span>
        </span>
        <span class="song-duration">{{ formatTime(song.duration) }}</span>
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import { defineProps, defineEmits } from "vue";

  interface Song {
    name: string;
    artist: string;
    coverUrl: string;
    duration: number;
  }

  const props = defineProps<{
    show: boolean;
    playlist: Song[];
    currentIndex: number;
  }>();

  const emit = defineEmits<{
    (e: "selectSong", index: number): void;
  }>();

  function selectSong(index: number) {
    emit("selectSong", index);
  }

  function formatTime(time: number): string {
    const minutes = Math.floor(time / 60);
    const seconds = Math.floor(time % 60);
    return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
  }
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "./_variables.scss";

  .playlist {
    position: absolute;
    bottom: 100%;
    left: 50%;
    transform: translateX(-50%);
    width: $module-width;
    max-height: 0;
    overflow: hidden;
    transition: $transition;
    opacity: 0;
    @include useBackdropFilter($base-bg-filter-blur);
    background-color: rgba($bg-color-secondary, 0.5);
    border-radius: $border-radius-modula $border-radius-modula 0 0;
    overflow-y: auto;
    margin-bottom: 1px;

    &.active {
      max-height: $playlist-height;
      opacity: 1;
    }

    ul {
      list-style-type: none;
      padding: 0;
      overflow-y: auto;

      li {
        display: flex;
        align-items: center;
        padding: 10px 15px;
        cursor: pointer;
        box-shadow: inset 0 -1px 0 #e0e0e0;

        &:hover {
          background-color: rgba($bg-color-primary, 0.05);
        }

        &.active {
          background-color: rgba($bg-color-primary, 0.1);
          color: $text-color-active;
        }

        .song-index {
          margin-right: 10px;
          font-size: 12px;
          color: $text-color-secondary;
        }

        .song-cover {
          margin-right: 10px;
          width: 40px;
          height: 40px;
          border-radius: 5px;
          overflow: hidden;

          img {
            width: 100%;
            height: 100%;
            object-fit: cover;
          }
        }

        .song-info {
          flex: 1;
          overflow: hidden;
          display: flex;
          flex-direction: column;
          justify-content: center;

          .song-name {
            font-weight: bold;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            margin-bottom: 2px;
          }

          .song-artist {
            font-size: 12px;
            color: $text-color-secondary;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
          }
        }

        .song-duration {
          font-size: 12px;
          color: $text-color-secondary;
        }
      }
    }
  }
</style>
