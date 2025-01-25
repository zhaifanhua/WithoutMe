<!-- 音乐播放器组件 -->

<template>
  <div class="music-player" :class="{ eject: isEject }">
    <!-- 音频元素 -->
    <audio ref="audioElement" preload="preload" @play="onPlay" @pause="onPause" @canplay="onCanPlay" @ended="onEnded">
      <source :src="currentSong.musicUrl" type="audio/mp3" />
    </audio>

    <!-- 频谱炫彩背景 -->
    <canvas ref="visualizer" class="visualizer" :style="{ opacity: isVisualizerActive ? 0.8 : 0 }"></canvas>

    <!-- 列表包装器 -->
    <div class="playlist-wrapper" :class="{ active: isPlaylistVisible }">
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
        <!-- 特殊按钮 -->
        <div class="specific">
          <button
            @click="toggleColorful"
            v-bind:class="{ opacity: isVisualizerActive ? 0.8 : 0 }"
            :class="{ active: isVisualizerActive }"
            :title="playerTitles.colorful"
          >
            <Icon :icon="playerIcons.colorful" class="colorful" />
          </button>
          <button @click="toggleLyrics" :class="{ active: isLyricsVisible }" :title="playerTitles.lyrics">
            <Icon :icon="playerIcons.lyrics" />
          </button>
          <button @click="togglePlaylist" :class="{ active: isPlaylistVisible }" :title="playerTitles.list">
            <Icon :icon="playerIcons.list" />
          </button>
        </div>
      </div>

      <!-- 中间容器 -->
      <div class="middle-container">
        <div class="middle-top">
          <!-- 歌曲信息 -->
          <div class="current-song-info">
            <div class="current-song-name">{{ currentSong.name }}</div>
            <div class="current-song-artist">{{ currentSong.artist }}</div>
          </div>
        </div>
        <div class="middle-middle">
          <!-- 控制按钮 -->
          <div class="control">
            <button @click="togglePlayMode" :title="playerTitles.mode">
              <Icon :icon="playerIcons.mode" />
            </button>
            <button @click="prevSong" :title="playerTitles.prev">
              <Icon :icon="playerIcons.prev" />
            </button>
            <button @click="togglePlay" :class="{ playing: isPlaying }" :title="playerTitles.playOrPause">
              <Icon :icon="playerIcons.playOrPause" />
            </button>
            <button @click="nextSong" :title="playerTitles.next">
              <Icon :icon="playerIcons.next" />
            </button>
            <button @click="toggleMute" :class="{ muted: isMuted || volumeLevel === 0 }" :title="playerTitles.volume">
              <Icon :icon="playerIcons.volume" />
            </button>
          </div>
        </div>
        <div class="middle-bottom">
          <!-- 进度条 -->
          <div class="player_progress" @click="seek" @mousedown="startDrag">
            <div class="player_progress_inner">
              <div class="player_progress_load" :style="loadStyle"></div>
              <div class="player_progress_play" :style="playStyle">
                <i class="player_progress_dot"></i>
              </div>
            </div>
          </div>
          <div class="time-display">
            <span>{{ formatTime(currentTime) }}</span>
            <span>{{ formatTime(songDuration) }}</span>
          </div>
        </div>
      </div>

      <!-- 右侧容器 -->
      <div class="right-container">
        <div class="volume-control">
          <div class="volume-bar" @click="setVolume" @mousedown="startVolumeDrag" :style="volumeBarStyle"></div>
        </div>
      </div>
    </div>

    <!-- 歌词包装器 -->
    <div class="lyrics-wrapper" :class="{ active: isLyricsVisible }">
      <ul class="lyrics" :style="{ transform: `translateY(${lyricsTranslateY}px)` }">
        <li v-for="(line, index) in lyrics" :key="index" :class="{ active: index === currentLyricIndex }">
          {{ line.text }}
        </li>
      </ul>
    </div>

    <!-- 侧边包装器 -->
    <div class="side-popup-wrapper">
      <button @click="toggleEject" :class="{ eject: isEject }">
        <Icon :icon="playerIcons.side" />
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, useTemplateRef, computed, watch, onMounted, onUnmounted, nextTick, reactive } from "vue";
  import axios from "axios";
  import { Icon } from "@iconify/vue";
  import { playList, Song } from "./playList";

  //#region 数据类型定义
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
  const audioRef = useTemplateRef<HTMLAudioElement>("audioElement"); // 音频元素
  const lyrics = ref<LyricLine[]>([]); // 解析后的歌词
  //#endregion

  //#region 播放器状态
  const isPlaying = ref(false); // 是否播放
  const isMuted = ref(false); // 是否静音
  const isEject = ref(false); // 是否弹出
  const volumeLevel = ref(1); // 音量
  const lyricsContainerHeight = ref(60); // 歌词容器高度
  const lineHeight = 20; // 行高
  const playMode = ref<PlayMode>(PlayMode.ListLoop); // 播放模式
  const isPlaylistVisible = ref(false); // 是否显示播放列表
  const isLyricsVisible = ref(false); // 是否显示歌词
  const isVisualizerActive = ref(false); // 是否显示炫彩
  const visualizerRef = useTemplateRef<HTMLCanvasElement | null>("visualizer");
  let audioContext: AudioContext | null = null;
  let analyser: AnalyserNode | null = null;
  let source: MediaElementAudioSourceNode | null = null;
  let animationFrameId: number | null = null;
  //#endregion

  //#region 当前歌曲信息
  const currentSongIndex = ref(0); // 当前歌曲索引
  const currentLyricIndex = ref(0); // 当前歌词索引
  const currentSong = computed(() => playList[currentSongIndex.value]); // 当前歌曲
  const currentTime = ref(0); // 当前歌曲时间
  const songDuration = ref(0); // 当前歌曲时长
  const loadProgress = ref(0); // 当前歌曲加载进度
  //#endregion

  //#region 控制信息
  // 图标
  const playerIcons = reactive({
    mode: computed(() => {
      switch (playMode.value) {
        case PlayMode.Sequential:
          return "solar:reorder-outline";
        case PlayMode.Random:
          return "solar:shuffle-linear";
        case PlayMode.SingleLoop:
          return "solar:repeat-one-linear";
        case PlayMode.ListLoop:
          return "solar:repeat-linear";
      }
    }),
    prev: "solar:skip-previous-bold",
    playOrPause: computed(() => (isPlaying.value ? "solar:pause-bold" : "solar:play-bold")),
    next: "solar:skip-next-bold",
    volume: computed(() => (isMuted.value || volumeLevel.value === 0 ? "solar:muted-linear" : "solar:volume-linear")),
    list: "solar:document-text-bold",
    lyrics: "solar:text-square-bold",
    colorful: "ion:color-palette",
    side: computed(() => (isEject.value ? "solar:alt-arrow-left-linear" : "solar:alt-arrow-right-linear")),
  });

  // 标题
  const playerTitles = reactive({
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
    prev: "上一首",
    playOrPause: computed(() => (isPlaying.value ? "暂停" : "播放")),
    next: "下一首",
    volume: computed(() => (isMuted.value || volumeLevel.value === 0 ? "静音" : "音量")),
    list: "播放列表",
    lyrics: "开关歌词",
    colorful: "开关炫彩",
  });
  //#endregion

  //#region 方法
  //#region 获取数据
  // 加载歌曲
  async function loadSong(index: number) {
    currentSongIndex.value = index;
    loadProgress.value = 0; // 重置加载进度
    await fetchLyrics(currentSong.value.lyricsUrl);
    nextTick(() => {
      if (audioRef.value) {
        audioRef.value.src = currentSong.value.musicUrl;
        audioRef.value.load();
      }
    });
  }

  // 获取歌词
  async function fetchLyrics(url: string) {
    try {
      const response = await axios.get(url);
      lyrics.value = parseLyrics(response.data);
    } catch (error) {
      console.error("无法获取歌词：", error);
      lyrics.value = [];
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
    if (!audioRef.value) return;

    // 初始化音频上下文
    if (!audioContext) {
      setupAudioContext();
    }

    if (isPlaying.value) {
      audioRef.value.pause();
    } else {
      isMuted.value = false; // 更新静音状态
      audioRef.value.play().catch(e => console.error("播放失败，请手动播放"));
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
      audioRef.value?.play();
    } else {
      nextSong();
    }
  }

  // 可以播放
  function onCanPlay() {
    audioRef.value?.play().catch(e => console.error("音乐自动播放失败，请手动播放"));
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
    if (!audioRef.value || lyrics.value.length === 0) return;
    const currentTime = audioRef.value.currentTime;
    const index = lyrics.value.findIndex((line, index) => {
      const nextLine = lyrics.value[index + 1];
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
    isLyricsVisible.value = !isLyricsVisible.value;
  }
  //#endregion

  //#region 更新炫彩
  // 切换炫彩
  function toggleColorful() {
    isVisualizerActive.value = !isVisualizerActive.value;

    // 初始化音频上下文
    if (!audioContext) {
      setupAudioContext();
    }

    if (isVisualizerActive.value) {
      drawVisualization(); // 重新启动可视化效果
    } else {
      if (animationFrameId) {
        cancelAnimationFrame(animationFrameId);
        animationFrameId = null;
      }
      // 清除canvas
      const canvas = visualizerRef.value;
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
    if (!audioRef.value) return;

    audioContext = new (window.AudioContext || (window as any).webkitAudioContext)();
    analyser = audioContext.createAnalyser();
    source = audioContext.createMediaElementSource(audioRef.value);
    source.connect(analyser);
    analyser.connect(audioContext.destination);
  }

  // 绘制可视化
  function drawVisualization() {
    if (!isVisualizerActive.value || !visualizerRef.value || !analyser) return;

    const canvas = visualizerRef.value;
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
      { r: 0, g: 0, b: 255 }, // 蓝
      { r: 0, g: 255, b: 255 }, // 青
      { r: 0, g: 255, b: 0 }, // 绿
      { r: 128, g: 0, b: 255 }, // 紫
      { r: 255, g: 255, b: 0 }, // 黄
      { r: 255, g: 128, b: 0 }, // 橙
      { r: 255, g: 0, b: 0 }, // 红
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

      // 增加间增量,使效果更快
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
    const percentage = volumeLevel.value * 100;
    return {
      height: "100%", // 占满父容器的高度
      background: `linear-gradient(to top, var(--bg-color-active) ${percentage}%, var(--bg-color-inactive) ${percentage}%)`,
    };
  });
  // 切换静音
  function toggleMute() {
    if (!audioRef.value) return;
    if (isMuted.value) {
      // 取消静音
      isMuted.value = false;
      volumeLevel.value = volumeLevel.value === 0 ? 0.5 : volumeLevel.value;
    } else {
      // 静音
      isMuted.value = true;
      volumeLevel.value = 0;
    }
    audioRef.value.volume = isMuted.value ? 0 : volumeLevel.value;
  }

  // 设置音量
  function setVolume(event: MouseEvent) {
    if (!audioRef.value) return;
    const volumeElement = event.currentTarget as HTMLElement;
    const rect = volumeElement.getBoundingClientRect();
    const offsetY = rect.bottom - event.clientY;
    const newVolume = offsetY / rect.height;
    volumeLevel.value = Math.min(Math.max(newVolume, 0), 1);
    audioRef.value.volume = volumeLevel.value;
    isMuted.value = volumeLevel.value === 0;
  }

  function startVolumeDrag(event: MouseEvent) {
    if (!audioRef.value) return;
    const volumeElement = event.currentTarget as HTMLElement;

    const onMouseMove = (moveEvent: MouseEvent) => {
      const rect = volumeElement.getBoundingClientRect();
      const offsetY = rect.bottom - moveEvent.clientY;
      const newVolume = offsetY / rect.height;
      volumeLevel.value = Math.min(Math.max(newVolume, 0), 1);
      audioRef.value.volume = volumeLevel.value;
      isMuted.value = volumeLevel.value === 0;
    };

    const onMouseUp = () => {
      document.removeEventListener("mousemove", onMouseMove);
      document.removeEventListener("mouseup", onMouseUp);
    };

    document.addEventListener("mousemove", onMouseMove);
    document.addEventListener("mouseup", onMouseUp);
  }
  //#endregion

  //#region 更新进度条
  // 跳转
  function seek(event: MouseEvent) {
    if (!audioRef.value || !songDuration.value) return;
    const progressElement = event.currentTarget as HTMLElement;
    const rect = progressElement.getBoundingClientRect();
    const offsetX = event.clientX - rect.left;
    const newTime = (offsetX / rect.width) * songDuration.value;
    audioRef.value.currentTime = newTime;
  }

  function startDrag(event: MouseEvent) {
    if (!audioRef.value) return;
    const progressElement = event.currentTarget as HTMLElement;

    const onMouseMove = (moveEvent: MouseEvent) => {
      const rect = progressElement.getBoundingClientRect();
      const offsetX = moveEvent.clientX - rect.left;
      const newTime = (offsetX / rect.width) * songDuration.value;
      audioRef.value.currentTime = newTime;
    };

    const onMouseUp = () => {
      document.removeEventListener("mousemove", onMouseMove);
      document.removeEventListener("mouseup", onMouseUp);
    };

    document.addEventListener("mousemove", onMouseMove);
    document.addEventListener("mouseup", onMouseUp);
  }

  // 更新进度
  function updateProgress() {
    if (audioRef.value) {
      currentTime.value = audioRef.value.currentTime;
      // 检查 duration 是否有效
      if (!isNaN(audioRef.value.duration)) {
        songDuration.value = audioRef.value.duration;
      }
      syncLyrics();
    }
  }
  //#endregion

  //#region 切换列表
  // 切换播放列表
  function togglePlaylist() {
    isPlaylistVisible.value = !isPlaylistVisible.value;
  }

  // 播放选中的歌曲
  function playSelectedSong(index: number) {
    loadSong(index);
    if (audioRef.value) {
      audioRef.value.play();
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

  watch(volumeLevel, newVolume => {
    if (audioRef.value) {
      audioRef.value.volume = newVolume;
      isMuted.value = newVolume === 0;
    }
    const lyricsContainer = document.querySelector(".lyrics-container");
    if (lyricsContainer) {
      lyricsContainerHeight.value = lyricsContainer.clientHeight;
    }
  });

  onMounted(() => {
    loadSong(0); // 初始化加载第一首歌曲
    if (audioRef.value) {
      audioRef.value.addEventListener("timeupdate", updateProgress);
      audioRef.value.addEventListener("canplay", onCanPlay);
      audioRef.value.addEventListener("play", onPlay);
      audioRef.value.addEventListener("pause", onPause);
      audioRef.value.addEventListener("ended", onEnded);
      audioRef.value.addEventListener("progress", updateLoadProgress); // 添加进度事件监听器
      audioRef.value.volume = volumeLevel.value;
      if (isVisualizerActive.value) {
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

  const loadStyle = computed(() => {
    const loadProgressValue = loadProgress.value || 0;
    return {
      width: `${loadProgressValue}%`,
      marginLeft: "0px",
    };
  });

  const playStyle = computed(() => {
    const playProgress = (currentTime.value / songDuration.value) * 100 || 0;
    return {
      width: `${playProgress}%`,
      marginLeft: "0px",
    };
  });

  function updateLoadProgress() {
    if (!audioRef.value) return;
    const buffered = audioRef.value.buffered;
    if (buffered.length > 0) {
      const loaded = buffered.end(buffered.length - 1);
      const total = audioRef.value.duration;
      loadProgress.value = (loaded / total) * 100;
    }
  }
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/variables" as variables;

  $x: 0px;
  $y: 20%;
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
  $text-color-active: #a12ceb;
  $border-radius: 20px;
  $border-radius-modula: 10px;
  $border-radius-side: 12px;
  $transition: all 0.3s ease;
  $overflow: hidden;

  .music-player {
    --bg-color-active: #000000;
    --bg-color-semiactive: #808080;
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
    transform: translateX(-($x + $width));
    @include mixins.useZindex(player);

    // 弹出
    &.eject {
      transform: translateX(0); // 根据播放器宽度调整
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
      @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
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
            margin-right: 14px;
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
      @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
      background-color: rgba($bg-color-primary, 0.5);
      border-radius: $border-radius;
      padding: 10px;

      // 左侧容器
      .left-container {
        flex: 0.6; // 不伸缩,保持原有大小
        max-width: 110px;

        .cover {
          width: 100px;
          height: 100px;
          margin-top: -30px;
          border-radius: 16px;
          overflow: hidden;

          img {
            width: 100%;
            height: 100%;
            object-fit: cover;
          }
        }

        .specific {
          display: flex;

          button {
            margin-left: 10px;
            background: none;
            margin-top: 10px;
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

      // 中间容器
      .middle-container {
        flex: 1; // 占据剩余空间
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        max-width: 220px;

        .middle-top {
          max-width: 200px;

          .current-song-info {
            max-width: 180px;

            .current-song-name {
              font-size: 16px;
              text-align: center;
              font-weight: bold;
              white-space: nowrap;
              overflow: hidden;
              text-overflow: ellipsis;
            }

            .current-song-artist {
              font-size: 14px;
              text-align: center;
              color: $text-color-secondary;
              white-space: nowrap;
              overflow: hidden;
              text-overflow: ellipsis;
            }
          }
        }

        .middle-middle {
          max-width: 200px;

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

        .middle-bottom {
          max-width: 200px;

          .player_progress {
            position: relative;
            width: 100%;
            height: 6px;
            background-color: var(--bg-color-inactive);
            cursor: pointer;
            border-radius: 3px;

            &_inner {
              position: relative;
              width: 100%;
              height: 100%;
            }

            &_load {
              position: absolute;
              top: 0;
              left: 0;
              height: 100%;
              background-color: var(--bg-color-semiactive);
              border-radius: 3px;
            }

            &_play {
              position: absolute;
              top: 0;
              left: 0;
              height: 100%;
              background-color: var(--bg-color-active);
              border-radius: 3px;

              .player_progress_dot {
                position: absolute;
                top: 50%;
                right: -5px;
                width: 10px;
                height: 10px;
                background-color: var(--bg-color-active);
                border-radius: 50%;
                transform: translateY(-50%);
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

      // 右侧容器
      .right-container {
        flex: 0.2;
        max-width: 20px;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;

        .volume-control {
          position: relative;
          display: flex;
          align-items: center;
          justify-content: center;
          height: 80%; // 高度占播放器高度的80%

          .volume-bar {
            position: relative;
            width: 6px;
            height: 100%; // 占满父容器的高度
            background-color: var(--bg-color-inactive);
            border-radius: 3px;
            cursor: pointer;
            overflow: hidden;
          }

          button {
            background: none;
            border: none;
            font-size: 18px;
            color: $text-color-secondary;
            cursor: pointer;
            transition: color 0.3s ease;

            &:hover {
              color: $text-color-primary;
            }
          }
        }
      }

      .control button,
      .specific button {
        transition: transform 0.3s ease; // 添加过渡效果
      }

      .control button:hover,
      .specific button:hover {
        transform: scale(1.4); // 鼠标移上去时放大
      }
    }

    // 歌词包装器
    .lyrics-wrapper {
      top: 100%;
      width: $module-width;
      height: 60px;
      @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
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

    // 侧边包装器
    .side-popup-wrapper {
      position: absolute;
      top: 50%;
      right: -$side-width;
      margin-right: -1px;
      transform: translateY(-50%);

      button {
        border: none;
        @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
        background-color: rgba($bg-color-secondary, 0.5);
        color: $text-color-primary;
        width: $side-width;
        height: $side-height;
        border-radius: 0 $border-radius-side $border-radius-side 0;
        cursor: pointer;
        transition: $transition;

        &.eject {
          @include mixins.useBackdropFilter(variables.$base-bg-filter-blur);
          background-color: rgba($bg-color-primary, 0.5);
        }
      }
    }
  }
</style>
