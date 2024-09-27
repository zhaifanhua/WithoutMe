<!-- 音乐播放器组件 -->

<template>
  <div class="music-player">
    <div class="song-info">
      <div class="cover" :class="{ rotating: isPlaying }">
        <img :src="currentSong.coverUrl" alt="Album cover" />
      </div>
      <div class="title-artist">
        <div class="title">{{ currentSong.title }}</div>
        <div class="artist">{{ currentSong.artist }}</div>
      </div>
    </div>
    <div class="player-controls">
      <button @click="prevSong">上一曲</button>
      <button @click="togglePlay" :class="{ playing: isPlaying }">
        {{ isPlaying ? "暂停" : "播放" }}
      </button>
      <button @click="nextSong">下一曲</button>
      <button @click="toggleMute" :class="{ muted: isMuted }">静音</button>
    </div>
    <div class="volume-control">
      <input type="range" min="0" max="1" step="0.01" v-model="volume" @input="setVolume" orient="vertical" />
    </div>
    <audio ref="audioElement" preload="preload" @play="onPlay" @pause="onPause" @canplay="onCanPlay">
      <source :src="currentSong.musicUrl" type="audio/mp3" />
    </audio>
    <div class="lyrics-container">
      <div class="lyrics" :style="{ transform: `translateY(${-currentLyricIndex * 20}px)` }">
        <div v-for="(line, index) in parsedLyrics" :key="index" :class="{ active: index === currentLyricIndex }">
          {{ line.text }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed, watch, onMounted, nextTick } from "vue";
  import axios from "axios";

  interface Song {
    title: string;
    artist: string;
    musicUrl: string;
    lyricsUrl: string;
    coverUrl: string;
  }

  interface LyricLine {
    time: number;
    text: string;
  }

  const songs: Song[] = [
    // 示例歌曲数据
    {
      title: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
    },
    // ...更多歌曲
  ];

  const parsedLyrics = ref<LyricLine[]>([]);
  const audioElement = ref<HTMLAudioElement>(null);
  const currentSongIndex = ref(0);
  const isPlaying = ref(false);
  const isMuted = ref(false);
  const currentLyricIndex = ref(0);
  const volume = ref(1);

  const currentSong = computed(() => songs[currentSongIndex.value]);

  function setVolume() {
    if (audioElement.value) {
      audioElement.value.volume = volume.value;
    }
  }

  function toggleMute() {
    if (!audioElement.value) return;
    audioElement.value.muted = !audioElement.value.muted;
    isMuted.value = audioElement.value.muted;
    if (!isMuted.value) {
      volume.value = audioElement.value.volume;
    }
  }
  async function loadSong(index: number) {
    currentSongIndex.value = index;
    await fetchLyrics(currentSong.value.lyricsUrl);
    nextTick(() => {
      if (audioElement.value) {
        audioElement.value.src = currentSong.value.musicUrl;
        audioElement.value.load();
      }
    });
  }

  async function fetchLyrics(url: string) {
    try {
      const response = await axios.get(url);
      parsedLyrics.value = parseLyrics(response.data);
    } catch (error) {
      console.error("Failed to fetch lyrics:", error);
      parsedLyrics.value = [];
    }
  }

  function parseLyrics(lyricsText: string): LyricLine[] {
    const lines = lyricsText.split("\n");
    return lines
      .map(line => {
        const match = line.match(/^\[(\d{2}):(\d{2})\.(\d{2})\](.*)/);
        if (match) {
          const [, minutes, seconds, centiseconds, text] = match;
          const time = parseInt(minutes) * 60 + parseInt(seconds) + parseInt(centiseconds) / 100;
          return { time, text: text.trim() };
        }
        return null;
      })
      .filter((line): line is LyricLine => line !== null);
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

  function onPlay() {
    isPlaying.value = true;
    syncLyrics();
  }

  function onPause() {
    isPlaying.value = false;
  }

  function onCanPlay() {
    console.log("Audio can play");
    // 可以在这里自动开始播放，如果需要的话
    audioElement.value?.play().catch(e => console.error("Auto-play failed:", e));
  }

  function syncLyrics() {
    if (!audioElement.value || parsedLyrics.value.length === 0) return;
    const currentTime = audioElement.value.currentTime;
    const index = parsedLyrics.value.findIndex((line, index) => {
      const nextLine = parsedLyrics.value[index + 1];
      return currentTime >= line.time && (!nextLine || currentTime < nextLine.time);
    });
    if (index !== -1 && index !== currentLyricIndex.value) {
      currentLyricIndex.value = index;
    }
  }

  onMounted(() => {
    loadSong(0);
    if (audioElement.value) {
      audioElement.value.addEventListener("timeupdate", syncLyrics);
      audioElement.value.volume = volume.value;
    }
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
  $button-hover-color: #f1f1f1;

  * {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
  }

  .music-player {
    position: fixed; // 固定定位
    bottom: 150px; // 距离底部150px
    left: 20px; // 距离右侧20px
    width: 300px; // 设置一个固定宽度
    max-width: 100%; // 确保在小屏幕上不会超出视口
    background: #fff;
    padding: 15px;
    border-radius: 10px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.15);
    display: flex;
    flex-direction: column;
    align-items: center;
    z-index: 1000; // 确保播放器在其他元素之上

    .song-info {
      display: flex;
      align-items: center;
      margin-bottom: 10px;
      width: 100%;

      .cover {
        width: 60px;
        height: 60px;
        border-radius: 50%;
        overflow: hidden;
        margin-right: 10px;
        animation: rotate 10s linear infinite paused; // 默认暂停状态

        img {
          width: 100%;
          height: 100%;
          object-fit: cover;
        }

        &.rotating {
          animation-play-state: running; // 播放时运行动画
        }
      }

      .title-artist {
        flex-grow: 1;
        overflow: hidden;

        .title {
          font-weight: bold;
          font-size: 14px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }

        .artist {
          font-size: 12px;
          color: #666;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
      }
    }

    .volume-control {
      position: absolute;
      right: 10px;
      top: 50%;
      transform: translateY(-50%);
      height: 100px;
      display: flex;
      align-items: center;
      width: 30px; // 增加宽度以便于操作

      input[type="range"] {
        -webkit-appearance: none;
        width: 100px; // 增加宽度
        height: 4px; // 减小高度
        background: $background-color;
        outline: none;
        transform: rotate(-90deg) translateY(-50px); // 旋转并调整位置
        transform-origin: right center; // 设置旋转原点

        &::-webkit-slider-thumb {
          -webkit-appearance: none;
          appearance: none;
          width: 16px;
          height: 16px;
          border-radius: 50%;
          background: $primary-color;
          cursor: pointer;
          margin-top: -6px; // 调整滑块位置
        }

        &::-moz-range-thumb {
          width: 16px;
          height: 16px;
          border-radius: 50%;
          background: $primary-color;
          cursor: pointer;
        }

        &::-webkit-slider-runnable-track {
          width: 100%;
          height: 4px;
          cursor: pointer;
          background: $background-color;
          border-radius: 2px;
        }

        &::-moz-range-track {
          width: 100%;
          height: 4px;
          cursor: pointer;
          background: $background-color;
          border-radius: 2px;
        }
      }
    }

    .player-controls {
      display: flex;
      gap: 10px;
      margin-bottom: 10px;

      button {
        background: none;
        border: none;
        cursor: pointer;
        padding: 8px;
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

    .lyrics-container {
      width: 100%;
      height: 60px; // 设置一个固定高度
      overflow: hidden; // 隐藏溢出的内容
      position: relative;
    }

    .lyrics {
      width: 100%;
      text-align: center;
      transition: transform 0.3s ease;

      div {
        height: 20px; // 每行歌词的高度
        line-height: 20px;
        opacity: 0.5;
        transition:
          opacity 0.3s,
          transform 0.3s;

        &.active {
          opacity: 1;
          font-weight: bold;
          transform: scale(1.05);
        }
      }
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .music-player {
      width: 250px; // 在小屏幕上减小宽度
      bottom: 10px; // 调整底部距离
      right: 10px; // 调整右侧距离
      padding: 10px;
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
