<!-- 音乐播放器组件 -->

<template>
  <div class="music-player" :class="{ eject: isEject }">
    <!-- 音频元素 -->
    <audio ref="audioElement" preload="preload" @play="onPlay" @pause="onPause" @canplay="onCanPlay" @ended="onEnded">
      <source :src="currentSong.musicUrl" type="audio/mp3" />
    </audio>

    <!-- 频谱炫彩背景 -->
    <canvas ref="visualizer" class="visualizer" :style="{ opacity: showColorful ? 0.8 : 0 }"></canvas>

    <!-- 列表包装器 -->
    <div class="playlist-wrapper" :class="{ active: showPlaylist }">
      <ul>
        <li v-for="(song, index) in playList" :key="index" @dblclick="playSelectedSong(index)" :class="{ active: index === currentSongIndex }">
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

    <!-- 播放器包装器 -->
    <div class="player-wrapper">
      <!-- 左侧容器 -->
      <div class="left-container">
        <div class="cover" :class="{ rotating: isPlaying }">
          <img :src="currentSong.coverUrl" :alt="currentSong.artist" />
        </div>
      </div>

      <!-- 右侧容器 -->
      <div class="right-container">
        <div class="right-top">
          <!-- 信息 -->
          <div class="info">
            <div class="name">{{ currentSong.name }}</div>
            <div class="artist">{{ currentSong.artist }}</div>
          </div>
          <!-- 特殊 -->
          <div class="specific">
            <button
              @click="toggleColorful"
              v-bind:class="{ opacity: showColorful ? 0.8 : 0 }"
              :class="{ active: showColorful }"
              :title="playerTitle.colorful"
            >
              <Icon :icon="playerIcon.colorful" class="colorful" />
            </button>
            <button @click="toggleLyrics" :class="{ active: showLyrics }" :title="playerTitle.lyrics">
              <Icon :icon="playerIcon.lyrics" />
            </button>
            <button @click="togglePlaylist" :class="{ active: showPlaylist }" :title="playerTitle.list">
              <Icon :icon="playerIcon.list" />
            </button>
          </div>
        </div>
        <div class="right-middle">
          <!-- 控制 -->
          <div class="control">
            <button @click="togglePlayMode" :title="playerTitle.mode">
              <Icon :icon="playerIcon.mode" />
            </button>
            <button @click="prevSong" :title="playerTitle.prev">
              <Icon :icon="playerIcon.prev" />
            </button>
            <button @click="togglePlay" :class="{ playing: isPlaying }" :title="playerTitle.playOrPause">
              <Icon :icon="playerIcon.playOrPause" />
            </button>
            <button @click="nextSong" :title="playerTitle.next">
              <Icon :icon="playerIcon.next" />
            </button>
            <button @click="toggleMute" :class="{ muted: isMuted || volume === 0 }" :title="playerTitle.volume">
              <Icon :icon="playerIcon.volume" />
            </button>
          </div>
        </div>
        <div class="right-bottom">
          <!-- 进度条 -->
          <div class="progress">
            <div class="slider">
              <input type="range" min="0" :value="currentTime" :max="currentDuration" @input="seek" :style="progressBarStyle" />
            </div>
            <div class="time-display">
              <span>{{ formatTime(currentTime) }}</span>
              <span>{{ formatTime(currentDuration) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 歌词包装器 -->
    <div class="lyrics-wrapper" :class="{ active: showLyrics }">
      <ul class="lyrics" :style="{ transform: `translateY(${lyricsTranslateY}px)` }">
        <li v-for="(line, index) in parsedLyrics" :key="index" :class="{ active: index === currentLyricIndex }">
          {{ line.text }}
        </li>
      </ul>
    </div>

    <!-- 侧边包装器 -->
    <div class="side-popup-wrapper">
      <button @click="toggleEject" :class="{ eject: isEject }">
        <Icon :icon="playerIcon.side" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, useTemplateRef, computed, watch, onMounted, onUnmounted, nextTick, reactive } from "vue";
  import axios from "axios";
  import { Icon } from "@iconify/vue";

  //#region 数据类型定义
  interface Song {
    name: string;
    artist: string;
    musicUrl: string;
    lyricsUrl: string;
    coverUrl: string;
    duration: number;
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
  //#endregion

  //#region 数据
  const audioElement = useTemplateRef<HTMLAudioElement>("audioElement"); // 音频元素
  const parsedLyrics = ref<LyricLine[]>([]); // 解析后的歌词
  const playList: Song[] = [
    // 示例歌曲数据
    {
      name: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "1111",
      artist: "1111",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "1111",
      artist: "1111",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "1111",
      artist: "1111",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "1111",
      artist: "1111",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "我们都被忘了",
      artist: "谢安琪",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
    {
      name: "1111",
      artist: "1111",
      musicUrl: "/audios/musics/1.mp3",
      lyricsUrl: "/audios/lyrics/1.lrc",
      coverUrl: "/audios/covers/1.jpg",
      duration: 2546,
    },
  ]; // 歌曲列表
  //#endregion

  //#region 播放器信息
  const isPlaying = ref(false); // 是否播放
  const isMuted = ref(false); // 是否静音
  const isEject = ref(false); // 是否弹出
  const volume = ref(1); // 音量
  const lyricsContainerHeight = ref(60); // 歌词容器高度
  const lineHeight = 20; // 行高
  const playMode = ref<PlayMode>(PlayMode.ListLoop); // 播放模式
  const showPlaylist = ref(false); // 是否显示播放列表
  const showLyrics = ref(false); // 是否显示歌词
  const showColorful = ref(false); // 是否显示炫彩
  const visualizer = useTemplateRef<HTMLCanvasElement | null>("visualizer");
  let audioContext: AudioContext | null = null;
  let analyser: AnalyserNode | null = null;
  let source: MediaElementAudioSourceNode | null = null;
  let animationFrameId: number | null = null;
  //#endregion

  //#region 歌曲信息
  const currentSongIndex = ref(0); // 当前歌曲索引
  const currentLyricIndex = ref(0); // 当前歌词索引
  const currentSong = computed(() => playList[currentSongIndex.value]); // 当前歌曲
  const currentTime = ref(0); // 当前时间
  const currentDuration = ref(0); // 当前歌曲时长
  //#endregion

  //#region 控制信息
  // 图标
  const playerIcon = reactive({
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
    list: "solar:document-text-bold",
    // 歌词
    lyrics: "solar:text-square-bold",
    // 炫彩
    colorful: "ion:color-palette",
    // 侧边弹出
    side: computed(() => {
      return isEject.value ? "solar:alt-arrow-right-linear" : "solar:alt-arrow-left-linear";
    }),
  });

  // 标题
  const playerTitle = reactive({
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
    // 开关歌词
    lyrics: "开关歌词",
    // 开关炫彩
    colorful: "开关炫彩",
  });
  //#endregion

  //#region 方法
  //#region 获取数据
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
  //#endregion

  //#region 切换歌曲
  // 上一首
  function prevSong() {
    if (currentSongIndex.value > 0) {
      loadSong(currentSongIndex.value - 1);
      scrollToCurrentSong();
    }
  }

  // 下一首
  function nextSong() {
    const nextIndex = getNextSongIndex();
    loadSong(nextIndex);
    scrollToCurrentSong();
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

  // 播放
  function onPlay() {
    isPlaying.value = true;
    syncLyrics();
  }

  // 暂停
  function onPause() {
    isPlaying.value = false;
  }

  // 播放结束
  function onEnded() {
    if (playMode.value === PlayMode.SingleLoop) {
      audioElement.value?.play();
    } else {
      nextSong();
    }
  }

  // 可以播放
  function onCanPlay() {
    audioElement.value?.play().catch(e => console.error("音乐自动播放失败，请手动播放"));
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
        return (currentSongIndex.value + 1) % playList.length;
      case PlayMode.Random:
        return Math.floor(Math.random() * playList.length);
      case PlayMode.SingleLoop:
        return currentSongIndex.value;
    }
  }

  //#endregion

  //#region 更新歌词
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

  // 歌词平移
  const lyricsTranslateY = computed(() => {
    const centerPosition = lyricsContainerHeight.value / 2;
    const activeLyricPosition = currentLyricIndex.value * lineHeight;
    return centerPosition - activeLyricPosition - lineHeight / 2;
  });

  // 切换歌词
  function toggleLyrics() {
    showLyrics.value = !showLyrics.value;
  }
  //#endregion

  //#region 更新炫彩
  // 切换炫彩
  function toggleColorful() {
    showColorful.value = !showColorful.value;
    if (showColorful.value) {
      drawVisualization(); // 重新启动可视化效果
    } else {
      if (animationFrameId) {
        cancelAnimationFrame(animationFrameId);
        animationFrameId = null;
      }
      // 清除canvas
      const canvas = visualizer.value;
      if (canvas) {
        const ctx = canvas.getContext("2d");
        if (ctx) {
          ctx.clearRect(0, 0, canvas.width, canvas.height);
        }
      }
    }
  }

  // 设置音频上下文
  function setupAudioContext() {
    if (!audioElement.value) return;

    audioContext = new (window.AudioContext || (window as any).webkitAudioContext)();
    analyser = audioContext.createAnalyser();
    source = audioContext.createMediaElementSource(audioElement.value);
    source.connect(analyser);
    analyser.connect(audioContext.destination);
  }

  // 绘制可视化
  function drawVisualization() {
    if (!showColorful.value || !visualizer.value || !analyser) return;

    const canvas = visualizer.value;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const width = canvas.width;
    const height = canvas.height;
    const bufferLength = analyser.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    // 创建离屏canvas用于缓存渐变
    const offscreenCanvas = document.createElement("canvas");
    offscreenCanvas.width = width;
    offscreenCanvas.height = height;
    const offscreenCtx = offscreenCanvas.getContext("2d");

    if (!offscreenCtx) return;

    // 定义颜色
    const colors = [
      { r: 255, g: 0, b: 128 }, // 粉红
      { r: 255, g: 255, b: 0 }, // 黄色
      { r: 0, g: 255, b: 255 }, // 青色
      { r: 128, g: 0, b: 255 }, // 紫色
      { r: 0, g: 255, b: 0 }, // 绿色
      { r: 255, g: 0, b: 0 }, // 红色
      { r: 255, g: 128, b: 0 }, // 橙色
    ];

    // 改进的噪声函数,增加频率
    function noise(x: number, y: number, z: number) {
      return (Math.sin(x / 5 + y / 4 + z * 2) + Math.sin(x / 4 - y / 3 + z * 3) + Math.sin(-x / 5 + y / 4 + z * 2.5)) / 3;
    }

    // 颜色插值函数
    function interpolateColor(color1: any, color2: any, factor: number) {
      const result = { r: 0, g: 0, b: 0 };
      result.r = Math.round(color1.r + factor * (color2.r - color1.r));
      result.g = Math.round(color1.g + factor * (color2.g - color1.g));
      result.b = Math.round(color1.b + factor * (color2.b - color1.b));
      return result;
    }

    let time = 0;

    // 绘制
    function draw() {
      animationFrameId = requestAnimationFrame(draw);

      analyser.getByteFrequencyData(dataArray);
      const average = dataArray.reduce((a, b) => a + b) / bufferLength;
      const intensity = average / 255;

      // 增加时间增量,使效果更快
      time += 0.05 * (1 + intensity * 2);

      // 使用音频数据来影响颜色选择
      const baseColorIndex = Math.floor(intensity * (colors.length - 1));

      for (let x = 0; x < width; x += 2) {
        for (let y = 0; y < height; y += 2) {
          const value = (noise(x / 20, y / 20, time) + 1) / 2;

          // 使用音频数据来影响颜色选择
          const colorIndex = (baseColorIndex + Math.floor(value * 2)) % colors.length;
          const nextColorIndex = (colorIndex + 1) % colors.length;

          const color1 = colors[colorIndex];
          const color2 = colors[nextColorIndex];
          const interpolationFactor = value * (colors.length - 1) - Math.floor(value * (colors.length - 1));
          const color = interpolateColor(color1, color2, interpolationFactor);

          // 使用音频强度来调整颜色的透明度
          offscreenCtx.fillStyle = `rgba(${color.r}, ${color.g}, ${color.b}, ${0.1 + intensity * 0.2})`;
          offscreenCtx.fillRect(x, y, 2, 2);
        }
      }

      // 将离屏canvas的内容绘制到主canvas上
      ctx.globalCompositeOperation = "source-over";
      ctx.drawImage(offscreenCanvas, 0, 0);

      // 轻微模糊效果
      ctx.globalCompositeOperation = "lighter";
      ctx.filter = "blur(100px)"; // 增加模糊半径
      ctx.drawImage(canvas, 0, 0);
      ctx.filter = "none";
    }

    draw();
  }
  //#endregion

  //#region 更新音量
  // 音量条样式
  const volumeBarStyle = computed(() => {
    const percentage = volume.value * 100;
    return {
      background: `linear-gradient(to right, var(--primary-color) ${percentage}%, var(--background-color) ${percentage}%)`,
    };
  });
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

  // 设置音量
  function setVolume() {
    if (audioElement.value) {
      audioElement.value.volume = volume.value;
      isMuted.value = volume.value === 0;
    }
  }
  //#endregion

  //#region 更新进度条
  // 进度条样式
  const progressBarStyle = computed(() => {
    let percentage = (currentTime.value / currentDuration.value) * 100 || 0;
    // 解决小圆点在刚开始和快结束时，与进度条出现缝隙的问题
    percentage = percentage < 10 ? percentage + 1 : percentage;
    percentage = percentage > 90 ? percentage - 1 : percentage;
    return {
      background: `linear-gradient(to right, var(--bg-color-active) ${percentage}%, var(--bg-color-inactive) ${percentage}%)`,
    };
  });

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
      // 检查 duration 是否有效
      if (!isNaN(audioElement.value.duration)) {
        currentDuration.value = audioElement.value.duration;
      }
    }
  }
  //#endregion

  //#region 切换列表
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

  // 滚动到当前歌曲
  function scrollToCurrentSong() {
    nextTick(() => {
      const activeSongElement = document.querySelector(`.playlist-wrapper li.active`);
      if (activeSongElement) {
        activeSongElement.scrollIntoView({ behavior: "smooth", block: "nearest" });
      }
    });
  }
  //#endregion

  // 切换弹出
  function toggleEject() {
    isEject.value = !isEject.value;
  }

  // 格式化时间函数
  function formatTime(time: number): string {
    const minutes = Math.floor(time / 60);
    const seconds = Math.floor(time % 60);
    return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
  }
  //#endregion

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

  onMounted(() => {
    loadSong(0);
    if (audioElement.value) {
      audioElement.value.addEventListener("timeupdate", updateProgress);
      audioElement.value.addEventListener("timeupdate", syncLyrics);
      audioElement.value.volume = volume.value;
      setupAudioContext();
      if (showColorful.value) {
        drawVisualization();
      }
    }
  });

  onUnmounted(() => {
    if (animationFrameId) {
      cancelAnimationFrame(animationFrameId);
    }
    if (audioContext) {
      audioContext.close();
    }
  });
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";

  $x: 0px;
  $y: 30%;
  $width: 360px;
  $player-width: 100%;
  $module-width: 90%;
  $side-width: 20px;
  $height: 120px;
  $player-height: $height;
  $playlist-height: 300px;
  $lyrics-height: $height;
  $side-height: $height * 0.6;
  $bg-color-primary: #ffffff;
  $bg-color-secondary: #c9c9c9;
  $text-color-primary: #000000;
  $text-color-secondary: #535353;
  $text-color-active: #00ff2f;
  $border-radius: 20px;
  $border-radius-modula: 10px;
  $border-radius-side: 12px;
  $transition: all 0.3s ease;
  $overflow: hidden;

  .music-player {
    --bg-color-active: #000000;
    --bg-color-inactive: #c0c0c0;

    position: fixed;
    left: $x;
    bottom: $y;
    width: $width;
    height: $height;
    transition: $transition;
    display: flex;
    flex-direction: column;
    align-items: center;
    overflow: visible;
    @include useZindex(player);

    // 弹出
    &.eject {
      transform: translateX(-($x + $width)); // 根据播放器宽度调整
    }

    // 频谱炫彩背景
    .visualizer {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      opacity: 0.8;
      border-radius: $border-radius;
      transition: opacity 0.3s ease;
    }

    .playlist-wrapper,
    .lyrics-wrapper,
    .player-wrapper {
      width: 100%; // 占满父容器宽度
      max-width: $player-width; // 最大宽度与播放器主体相同
      margin: 0 auto; // 水平居中
    }

    // 播放列表和歌词
    .playlist-wrapper,
    .lyrics-wrapper {
      position: absolute;
      left: 50%;
      transform: translateX(-50%);
      width: 90%;
      max-height: 0;
      overflow: hidden;
      transition: $transition;
      opacity: 0;
    }

    // 播放列表包装器
    .playlist-wrapper {
      bottom: 100%;
      width: $module-width;
      @include useBackdropFilter($base-bg-filter-blur);
      background-color: rgba($bg-color-secondary, 0.5);
      border-radius: $border-radius-modula $border-radius-modula 0 0;
      margin-bottom: 1px;
      overflow-y: auto;
      // 隐藏滚动条
      scrollbar-width: none;
      &::-webkit-scrollbar {
        display: none;
      }

      &.active {
        max-height: $playlist-height;
        opacity: 1;
      }

      ul {
        list-style-type: none;
        padding: 0;

        li {
          display: flex;
          align-items: center;
          padding: 10px 15px;
          cursor: pointer;

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

    // 播放器包装器
    .player-wrapper {
      position: relative;
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: $player-width;
      height: $player-height;
      @include useBackdropFilter($base-bg-filter-blur);
      background-color: rgba($bg-color-primary, 0.5);
      border-radius: $border-radius;
      padding: 10px;

      // 左侧容器
      .left-container {
        flex: 0 0 auto; // 不伸缩,保持原有大小
        margin-right: 15px;

        .cover {
          width: 100px;
          height: 100px;
          border-radius: 16px;
          overflow: hidden;

          img {
            width: 100%;
            height: 100%;
            object-fit: cover;
          }
        }
      }

      // 右侧容器
      .right-container {
        flex: 1; // 占据剩余空间
        display: flex;
        flex-direction: column;
        justify-content: space-between;

        .right-top {
          display: flex;
          justify-content: space-between;
          align-items: center;

          .info {
            flex: 1;
            overflow: hidden;

            .name {
              font-size: 14px;
              font-weight: bold;
              white-space: nowrap;
              overflow: hidden;
              text-overflow: ellipsis;
            }

            .artist {
              font-size: 12px;
              color: $text-color-secondary;
            }
          }

          .specific {
            display: flex;
            margin-bottom: 10px;

            button {
              margin-left: 10px;
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
        }

        .right-middle {
          .control {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 10px;

            button {
              background: none;
              border: none;
              font-size: 18px;
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
        }

        .right-bottom {
          .progress {
            .slider {
              // 进度条
              input[type="range"] {
                -webkit-appearance: none; // 移除默认样式
                appearance: none;
                width: 100%;
                height: 6px;
                border-radius: 3px;
                outline: none; // 移除轮廓

                // 圆点
                &::-webkit-slider-thumb {
                  -webkit-appearance: none; // 移除默认样式
                  appearance: none;
                  height: 10px;
                  width: 10px;
                  border-radius: 50%;
                  background-color: var(--bg-color-active);
                  cursor: pointer;
                }
                &::-moz-range-thumb {
                  height: 10px;
                  width: 10px;
                  border-radius: 50%;
                  background-color: var(--bg-color-active);
                  cursor: pointer;
                }
              }
            }

            .time-display {
              display: flex;
              justify-content: space-between;
              font-size: 10px;
              color: $text-color-secondary;
            }
          }
        }
      }
    }

    // 歌词包装器
    .lyrics-wrapper {
      top: 100%;
      width: $module-width;
      height: 60px;
      @include useBackdropFilter($base-bg-filter-blur);
      background-color: rgba($bg-color-secondary, 0.5);
      border-radius: 0 0 $border-radius-modula $border-radius-modula;
      overflow: $overflow;
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

    // 侧边弹出包装器
    .side-popup-wrapper {
      position: absolute;
      top: 50%;
      right: -$side-width;
      margin-right: -1px;
      transform: translateY(-50%);

      button {
        border: none;
        @include useBackdropFilter($base-bg-filter-blur);
        background-color: rgba($bg-color-secondary, 0.5);
        color: $text-color-primary;
        width: $side-width;
        height: $side-height;
        border-radius: 0 $border-radius-side $border-radius-side 0;
        cursor: pointer;
        transition: $transition;

        &.eject {
          @include useBackdropFilter($base-bg-filter-blur);
          background-color: rgba($bg-color-primary, 0.5);
        }
      }
    }
  }
</style>
