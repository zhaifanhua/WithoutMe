<!-- 音乐播放器组件 -->

<template>
  <div class="music-player">
    <!-- 详情包装器 -->
    <div class="details-wrapper">
      <!-- 信息容器 -->
      <div class="info-container">
        <!-- 封面 -->
        <div class="cover-container" :class="{ rotating: isPlaying }">
          <img :src="currentSong.coverUrl" alt="cover" />
        </div>
        <!-- 歌名歌手 -->
        <div class="title-container">
          <div class="title">{{ currentSong.title }}</div>
          <div class="artist">{{ currentSong.artist }}</div>
        </div>
      </div>
      <!-- 歌词容器 -->
      <div class="lyrics-container">
        <ul class="lyrics" :style="{ transform: `translateY(${lyricsTranslateY}px)` }">
          <li v-for="(line, index) in parsedLyrics" :key="index" :class="{ active: index === currentLyricIndex }">
            {{ line.text }}
          </li>
        </ul>
      </div>
      <!-- 音频元素 -->
      <audio ref="audioElement" preload="preload" @play="onPlay" @pause="onPause" @canplay="onCanPlay" @ended="onEnded">
        <source :src="currentSong.musicUrl" type="audio/mp3" />
      </audio>
    </div>

    <!-- 控制包装器 -->
    <div class="control-wrapper">
      <!-- 进度条控制 -->
      <div class="progress-control">
        <div class="time-display">
          {{ formatTime(currentTime) }}
        </div>
        <div class="slider-container">
          <input type="range" min="0" :max="duration" :value="currentTime" @input="seek" :style="progressBarStyle" />
        </div>
        <div class="time-display">
          {{ formatTime(duration) }}
        </div>
      </div>
      <!-- 播放控制 -->
      <div class="player-control">
        <button @click="togglePlayMode" :title="playerButtonTitle.mode">
          <Icon :icon="playerButtonIcon.mode" />
        </button>
        <button @click="prevSong" :title="playerButtonTitle.prev">
          <Icon :icon="playerButtonIcon.prev" />
        </button>
        <button @click="togglePlay" :class="{ playing: isPlaying }" :title="playerButtonTitle.playOrPause">
          <Icon :icon="playerButtonIcon.playOrPause" />
        </button>
        <button @click="nextSong" :title="playerButtonTitle.next">
          <Icon :icon="playerButtonIcon.next" />
        </button>
        <button @click="togglePlaylist" :class="{ active: showPlaylist }" :title="playerButtonTitle.list">
          <Icon :icon="playerButtonIcon.list" />
        </button>
      </div>
    </div>

    <!-- 音量控制 -->
    <div class="volume-control">
      <button @click="toggleMute" class="volume-icon" :class="{ muted: isMuted || volume === 0 }" :title="playerButtonTitle.volume">
        <Icon :icon="playerButtonIcon.volume" />
      </button>
      <div class="percentage-display">{{ Math.round(volume * 100) }}%</div>
      <div class="slider-container">
        <input type="range" min="0" max="1" step="0.01" v-model="volume" @input="setVolume" :style="volumeBarStyle" />
      </div>
    </div>

    <!-- 播放列表包装器 -->
    <div class="playlist-wrapper" v-if="showPlaylist">
      <h3>播放列表</h3>
      <ul>
        <li v-for="(song, index) in songs" :key="index" @dblclick="playSelectedSong(index)" :class="{ active: index === currentSongIndex }">
          {{ song.title }} - {{ song.artist }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, useTemplateRef, computed, watch, onMounted, nextTick, reactive } from "vue";
  import axios from "axios";
  import { Icon } from "@iconify/vue";

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

  enum PlayMode {
    Sequential = "sequential",
    Random = "random",
    SingleLoop = "singleLoop",
    ListLoop = "listLoop",
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
    {
      title: "2222",
      artist: "2222",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
    },
    // ...更多歌曲
  ];

  const parsedLyrics = ref<LyricLine[]>([]);
  const audioElement = useTemplateRef<HTMLAudioElement>("audioElement");
  const currentSongIndex = ref(0);
  const isPlaying = ref(false);
  const isMuted = ref(false);
  const currentLyricIndex = ref(0);
  const volume = ref(1);
  const lyricsContainerHeight = ref(60);
  const lineHeight = 20;
  const currentTime = ref(0);
  const playMode = ref<PlayMode>(PlayMode.ListLoop);
  const duration = ref(0);
  const showPlaylist = ref(false);

  // 控制按钮图标
  const playerButtonIcon = reactive({
    // 播放模式
    mode: computed(() => {
      switch (playMode.value) {
        // 顺序播放
        case PlayMode.Sequential:
          return "solar:reorder-outline";
        // 随机播放
        case PlayMode.Random:
          return "solar:shuffle-linear";
        // 单曲循环
        case PlayMode.SingleLoop:
          return "solar:repeat-one-linear";
        // 列表循环
        case PlayMode.ListLoop:
          return "solar:repeat-linear";
      }
    }),
    // 上一首
    prev: "solar:skip-previous-bold",
    // 播放/暂停
    playOrPause: computed(() => {
      return isPlaying.value ? "solar:pause-bold" : "solar:play-bold";
    }),
    // 下一首
    next: "solar:skip-next-bold",
    // 音量
    volume: computed(() => {
      return isMuted.value || volume.value === 0 ? "solar:muted-linear" : "solar:volume-linear";
    }),
    // 播放列表
    list: "solar:playlist-bold",
  });

  // 控制按钮标题
  const playerButtonTitle = reactive({
    // 播放模式
    mode: computed(() => {
      switch (playMode.value) {
        case PlayMode.Sequential:
          return "顺序播放";
        case PlayMode.Random:
          return "随机播放";
        case PlayMode.SingleLoop:
          return "单曲循环";
        case PlayMode.ListLoop:
          return "列表循环";
      }
    }),
    // 上一首
    prev: "上一首",
    // 播放/暂停
    playOrPause: computed(() => {
      return isPlaying.value ? "暂停" : "播放";
    }),
    // 下一首
    next: "下一首",
    // 音量
    volume: computed(() => {
      return isMuted.value || volume.value === 0 ? "静音" : "音量";
    }),
    // 播放列表
    list: "播放列表",
  });

  // 当前歌曲
  const currentSong = computed(() => songs[currentSongIndex.value]);

  // 歌词平移
  const lyricsTranslateY = computed(() => {
    const centerPosition = lyricsContainerHeight.value / 2;
    const activeLyricPosition = currentLyricIndex.value * lineHeight;
    return centerPosition - activeLyricPosition - lineHeight / 2;
  });

  // 进度条样式
  const progressBarStyle = computed(() => {
    const percentage = (currentTime.value / duration.value) * 100 || 0;
    return {
      background: `linear-gradient(to right, var(--primary-color) ${percentage}%, var(--background-color) ${percentage}%)`,
    };
  });

  // 音量条样式
  const volumeBarStyle = computed(() => {
    const percentage = volume.value * 100;
    return {
      background: `linear-gradient(to right, var(--primary-color) ${percentage}%, var(--background-color) ${percentage}%)`,
    };
  });

  // 加载歌曲
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

  // 获取歌词
  async function fetchLyrics(url: string) {
    try {
      const response = await axios.get(url);
      parsedLyrics.value = parseLyrics(response.data);
    } catch (error) {
      console.error("Failed to fetch lyrics:", error);
      parsedLyrics.value = [];
    }
  }

  // 解析歌词
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

  // 同步歌词
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

  // 跳转
  function seek(event: Event) {
    const target = event.target as HTMLInputElement;
    if (audioElement.value) {
      audioElement.value.currentTime = Number(target.value);
    }
  }

  // 更新进度
  function updateProgress() {
    if (audioElement.value) {
      currentTime.value = audioElement.value.currentTime;
      duration.value = audioElement.value.duration;
    }
  }

  // 上一首
  function prevSong() {
    if (currentSongIndex.value > 0) {
      loadSong(currentSongIndex.value - 1);
    }
  }

  // 下一首
  function nextSong() {
    const nextIndex = getNextSongIndex();
    loadSong(nextIndex);
  }

  // 播放/暂停
  function togglePlay() {
    if (!audioElement.value) return;
    if (isPlaying.value) {
      audioElement.value.pause();
    } else {
      audioElement.value.play();
    }
    isPlaying.value = !isPlaying.value;
  }

  // 切换播放模式
  function togglePlayMode() {
    const modes = Object.values(PlayMode);
    const currentIndex = modes.indexOf(playMode.value);
    const nextIndex = (currentIndex + 1) % modes.length;
    playMode.value = modes[nextIndex];
  }

  // 获取下一首歌曲索引
  function getNextSongIndex(): number {
    switch (playMode.value) {
      case PlayMode.Sequential:
      case PlayMode.ListLoop:
        return (currentSongIndex.value + 1) % songs.length;
      case PlayMode.Random:
        return Math.floor(Math.random() * songs.length);
      case PlayMode.SingleLoop:
        return currentSongIndex.value;
    }
  }

  // 播放结束
  function onEnded() {
    if (playMode.value === PlayMode.SingleLoop) {
      audioElement.value?.play();
    } else {
      nextSong();
    }
  }

  // 设置音量
  function setVolume() {
    if (audioElement.value) {
      audioElement.value.volume = volume.value;
      isMuted.value = volume.value === 0;
    }
  }

  // 切换静音
  function toggleMute() {
    if (!audioElement.value) return;
    if (isMuted.value) {
      // 取消静音
      isMuted.value = false;
      volume.value = volume.value === 0 ? 0.5 : volume.value;
    } else {
      // 静音
      isMuted.value = true;
      volume.value = 0;
    }
    audioElement.value.volume = isMuted.value ? 0 : volume.value;
  }

  // 播放
  function onPlay() {
    isPlaying.value = true;
    syncLyrics();
  }

  // 暂停
  function onPause() {
    isPlaying.value = false;
  }

  // 切换播放列表
  function togglePlaylist() {
    showPlaylist.value = !showPlaylist.value;
  }

  // 播放选中的歌曲
  function playSelectedSong(index: number) {
    loadSong(index);
    if (audioElement.value) {
      audioElement.value.play();
    }
  }

  // 可以播放
  function onCanPlay() {
    console.log("Audio can play");
    // 可以在这里自动开始播放，如果需要的话
    audioElement.value?.play().catch(e => console.error("Auto-play failed:", e));
  }

  // 格式化时间函数
  function formatTime(time: number): string {
    const minutes = Math.floor(time / 60);
    const seconds = Math.floor(time % 60);
    return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
  }

  onMounted(() => {
    loadSong(0);
    if (audioElement.value) {
      audioElement.value.addEventListener("timeupdate", updateProgress);
      audioElement.value.addEventListener("timeupdate", syncLyrics);
      audioElement.value.volume = volume.value;
    }
  });

  watch(volume, newVolume => {
    if (audioElement.value) {
      audioElement.value.volume = newVolume;
      isMuted.value = newVolume === 0;
    }
    const lyricsContainer = document.querySelector(".lyrics-container");
    if (lyricsContainer) {
      lyricsContainerHeight.value = lyricsContainer.clientHeight;
    }
  });
</script>

<style scoped lang="scss">
  $primary-color: #38a6f0;
  $secondary-color: #00ff6a;
  $background-color: #ecf0f1;
  $text-color: #34495e;
  $button-background-color-hover: #45aac1;
  $button-background-color-active: #26e2f7;

  @keyframes rotate {
    from {
      transform: rotate(0deg);
    }
    to {
      transform: rotate(360deg);
    }
  }

  @mixin glassmorphism {
    background: rgba(255, 255, 255, 0.25);
    backdrop-filter: blur(10px);
    border: 1px solid rgba(255, 255, 255, 0.18);
    box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
  }

  @media (max-width: 768px) {
    .music-player {
      width: 280px;
      bottom: 10px;
      right: 10px;
      padding: 15px;
    }
  }

  // 音乐播放器
  .music-player {
    --primary-color: #{$primary-color};
    --background-color: #{$background-color};
    --text-color: #{$text-color};

    @include glassmorphism;
    position: fixed;
    bottom: 100px;
    left: 100px;
    width: 300px;
    max-width: 100%;
    padding: 20px;
    border-radius: 15px;
    z-index: 1000;
    transition: all 0.3s ease;

    &:hover {
      transform: translateY(-5px);
      box-shadow: 0 12px 40px 0 rgba(31, 38, 135, 0.45);
    }

    // 详情包装器
    .details-wrapper {
      display: flex;
      flex-direction: column;
      align-items: center;
      margin-bottom: 15px;

      // 信息容器
      .info-container {
        display: flex;
        align-items: center;
        width: 100%;
        margin-bottom: 10px;

        // 封面容器
        .cover-container {
          width: 60px;
          height: 60px;
          border-radius: 50%;
          overflow: hidden;
          margin-right: 15px;
          box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
          animation: rotate 10s linear infinite paused;

          img {
            width: 100%;
            height: 100%;
            object-fit: cover;
          }

          &.rotating {
            animation-play-state: running;
          }
        }

        // 歌名歌手
        .title-container {
          flex-grow: 1;
          display: flex;
          flex-direction: column;
          justify-content: center;
          text-align: center;

          .title {
            font-weight: bold;
            font-size: 18px;
            margin-bottom: 5px;
            color: $text-color;
            text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.1);
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
          }

          .artist {
            font-size: 14px;
            color: $text-color;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
          }
        }
      }

      // 歌词容器
      .lyrics-container {
        width: 100%;
        height: 60px;
        overflow: hidden;
        position: relative;
        margin-top: 10px;

        .lyrics {
          text-align: center;
          transition: transform 0.5s ease;

          li {
            height: 20px;
            line-height: 20px;
            opacity: 0.7;
            transition: all 0.3s ease;
            font-size: 14px;
            color: $text-color;

            &.active {
              opacity: 1;
              font-weight: bold;
              transform: scale(1.2);
              color: $secondary-color;
            }
          }
        }
      }
    }

    // 控制包装器
    .control-wrapper {
      display: flex;
      flex-direction: column;
      width: 100%;

      // 进度条控制
      .progress-control {
        width: 100%;
        margin-bottom: 10px;
        display: flex;
        align-items: center;
        justify-content: space-between;

        // 时间显示
        .time-display {
          font-size: 12px;
          color: $text-color;
          min-width: 40px; // 确保时间显示有足够的空间
        }
      }

      // 播放控制
      .player-control {
        display: flex;
        gap: 15px;

        button {
          background: none;
          border: none;
          cursor: pointer;
          padding: 10px;
          border-radius: 10px;
          transition: all 0.3s ease;
          color: $text-color;

          &:hover {
            background-color: $button-background-color-hover;
            transform: scale(1.1);
          }

          &.playing {
            color: $button-background-color-active;
          }

          &:last-child {
            font-size: 18px;

            &.active {
              color: $primary-color;
            }
          }
        }
      }

      // 音量控制
      .volume-control {
        position: relative;
        display: flex;
        align-items: center;

        // 音量图标
        .volume-icon {
          background: none;
          border: none;
          cursor: pointer;
          font-size: 20px;
          padding: 5px;
          margin-right: 10px;
          transition: all 0.3s ease;
          color: $text-color;

          &:hover {
            transform: scale(1.1);
          }

          &.muted {
            color: #000;
          }
        }

        // 音量百分比
        .percentage-display {
          font-size: 12px;
          color: $text-color;
          min-width: 40px;
        }
      }

      // 进度条和音量条
      .progress-control,
      .volume-control {
        // 音量条
        .slider-container {
          flex-grow: 1;
          margin: 0 10px;
        }

        // 界限
        input[type="range"] {
          -webkit-appearance: none;
          width: 100%;
          height: 4px;
          border-radius: 2px;
          outline: none;
          padding: 0;
          margin: 0;

          &::-webkit-slider-thumb {
            -webkit-appearance: none;
            appearance: none;
            width: 12px;
            height: 12px;
            border-radius: 50%;
            background: $primary-color;
            cursor: pointer;
            transition: all 0.3s ease;

            &:hover {
              transform: scale(1.2);
            }
          }

          &::-moz-range-thumb {
            width: 12px;
            height: 12px;
            border-radius: 50%;
            background: $primary-color;
            cursor: pointer;
            transition: all 0.3s ease;

            &:hover {
              transform: scale(1.2);
            }
          }
        }
      }
    }

    // 播放列表包装器
    .playlist-wrapper {
      position: fixed;
      top: 0;
      right: -300px; // 初始位置在屏幕右侧
      width: 300px;
      height: 100vh;
      background: rgba(255, 255, 255, 0.9);
      backdrop-filter: blur(10px);
      box-shadow: -2px 0 10px rgba(0, 0, 0, 0.1);
      transition: right 0.3s ease;
      z-index: 1001;
      overflow-y: auto;

      &.show {
        right: 0; // 显示时滑入屏幕
      }

      h3 {
        padding: 20px;
        margin: 0;
        font-size: 18px;
        color: $text-color;
        border-bottom: 1px solid rgba(0, 0, 0, 0.1);
      }

      ul {
        list-style: none;
        padding: 0;
        margin: 0;

        li {
          padding: 15px 20px;
          cursor: pointer;
          transition: background-color 0.3s ease;

          &:hover {
            background-color: rgba(0, 0, 0, 0.05);
          }

          &.active {
            background-color: rgba($primary-color, 0.2);
            font-weight: bold;
          }
        }
      }
    }
  }
</style>
