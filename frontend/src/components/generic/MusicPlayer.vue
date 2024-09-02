<!-- 音乐播放器组件 -->

<template>
  <div class="music-player">
    <div class="player-controls">
      <button @click="prevSong">上一曲</button>
      <button @click="togglePlay" :class="{ playing: isPlaying }">
        {{ isPlaying ? '暂停' : '播放' }}
      </button>
      <button @click="nextSong">下一曲</button>
      <button @click="toggleMute" :class="{ muted: isMuted }">静音</button>
    </div>
    <audio ref="audioElement" preload="preload" muted @play="onPlay" @pause="onPause">
      <source :src="currentSong.url" type="audio/mp3" />
    </audio>
    <div class="lyrics">
      <div v-for="(line, index) in lyrics" :key="index" :class="{ active: index === currentLyricIndex }">
        {{ line }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed, watch, onMounted, nextTick } from 'vue';

  interface Song {
    title: string;
    url: string;
    lyrics: string[];
  }

  const songs: Song[] = [
    // 示例歌曲数据
    {
      title: 'Song 1',
      url: 'music/1.mp3',
      lyrics: ['[00:00.00]Lyric line 1', '[00:05.00]Lyric line 2'],
    },
    // ...更多歌曲
  ];

  const audioElement = ref<HTMLAudioElement>(null);
  const currentSongIndex = ref(0);
  const isPlaying = ref(false);
  const isMuted = ref(false);
  const lyrics = ref<string[]>([]);
  const currentLyricIndex = ref(0);

  const currentSong = computed(() => songs[currentSongIndex.value]);

  function loadSong(index: number) {
    currentSongIndex.value = index;
    lyrics.value = currentSong.value.lyrics;
    nextTick(() => {
      if (audioElement.value) {
        audioElement.value.src = currentSong.value.url;
        audioElement.value.load();
      }
    });
  }

  function prevSong() {
    if (currentSongIndex.value > 0) {
      loadSong(currentSongIndex.value - 1);
    }
  }

  function nextSong() {
    if (currentSongIndex.value < songs.length - 1) {
      loadSong(currentSongIndex.value + 1);
    }
  }

  function togglePlay() {
    if (!audioElement.value) return;
    if (isPlaying.value) {
      audioElement.value.pause();
    } else {
      audioElement.value.play();
    }
    isPlaying.value = !isPlaying.value;
  }

  function toggleMute() {
    if (!audioElement.value) return;
    audioElement.value.muted = !audioElement.value.muted;
    isMuted.value = audioElement.value.muted;
  }

  function onPlay() {
    isPlaying.value = true;
    syncLyrics();
  }

  function onPause() {
    isPlaying.value = false;
  }

  function syncLyrics() {
    if (!audioElement.value || !lyrics.value.length) return;
    const currentTime = audioElement.value.currentTime;
    lyrics.value.forEach((line, index) => {
      const time = line.match(/\[\d+:\d+\.\d+\]/);
      if (time && currentTime >= parseFloat(time[0].slice(1, -1))) {
        currentLyricIndex.value = index;
      }
    });
  }

  onMounted(() => {
    loadSong(0);
    audioElement.value?.addEventListener('timeupdate', syncLyrics);
  });

  watch(isPlaying, newVal => {
    if (newVal) {
      audioElement.value?.play();
    } else {
      audioElement.value?.pause();
    }
  });
</script>

<style scoped lang="scss">
  $primary-color: #3498db;
  $text-color: #34495e;
  $background-color: #ecf0f1;
  $button-hover-color: darken($primary-color, 10%);

  * {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
  }

  body {
    font-family: 'Roboto', sans-serif;
    background-color: $background-color;
    color: $text-color;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    margin: 0;
  }

  .music-player {
    width: 100%;
    max-width: 600px;
    background: #fff;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.15);
    display: flex;
    flex-direction: column;
    align-items: center;

    .player-controls {
      display: flex;
      gap: 10px;
      margin-bottom: 20px;

      button {
        background: none;
        border: none;
        cursor: pointer;
        padding: 10px;
        border-radius: 5px;
        transition: background-color 0.3s;

        &.playing {
          color: $primary-color;
        }

        &.muted {
          color: #e74c3c;
        }

        &:hover {
          background-color: $button-hover-color;
        }
      }
    }

    .lyrics {
      width: 100%;
      text-align: center;
      margin-bottom: 20px;

      div {
        opacity: 0.5;
        transition: opacity 0.3s;

        &.active {
          opacity: 1;
          font-weight: bold;
        }
      }
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .music-player {
      padding: 10px;
    }
  }
</style>
