<!-- 音乐播放器组件 -->

<template>
  <div class="music-player" :class="{ eject: isEject }">
    <!-- 移除测试按钮 -->
    <!-- 音频元素 -->
    <audio ref="audioElement" preload="preload" @play="onPlay" @pause="onPause" @canplay="onCanPlay" @ended="onEnded">
      <source :src="currentSong.musicUrl" type="audio/mp3" />
    </audio>

    <!-- 播放器 -->
    <div class="player">
      <!-- 频谱炫彩背景 -->
      <Visualizer :show="showColorful" :analyser="analyser" />

      <!-- 左侧容器 -->
      <div class="left-container">
        <div class="left1">
          <!-- 功能按钮 -->
          <FunctionButtons
            :show-colorful="showColorful"
            :show-lyrics="showLyrics"
            :show-playlist="showPlaylist"
            @toggle-colorful="toggleColorful"
            @toggle-lyrics="toggleLyrics"
            @toggle-playlist="togglePlaylist"
          />
        </div>
        <div class="left2">
          <!-- 歌曲信息 -->
          <SongInfo :song="currentSong" :is-playing="isPlaying" />
        </div>
      </div>

      <!-- 右侧容器 -->
      <div class="right-container">
        <div class="right1">
          <!-- 控制 -->
          <PlayControls
            :is-playing="isPlaying"
            :play-mode="playMode"
            :is-muted="isMuted"
            :volume="volume"
            @toggle-play="togglePlay"
            @prev-song="prevSong"
            @next-song="nextSong"
            @toggle-play-mode="togglePlayMode"
            @toggle-mute="toggleMute"
          />
        </div>
        <div class="right2">
          <!-- 进度条 -->
          <div class="progress">
            <ProgressBar :min="0" :max="currentDuration" :value="currentTime" @update:value="seek" />
            <TimeDisplay :current-time="currentTime" :duration="currentDuration" />
          </div>
        </div>
      </div>
    </div>

    <!-- 列表 -->
    <Playlist :show="showPlaylist" :playlist="playList" :current-index="currentSongIndex" @select-song="playSelectedSong" />

    <!-- 歌词 -->
    <Lyrics :show="showLyrics" :lyrics="parsedLyrics" :current-time="currentTime" />

    <!-- 侧边 -->
    <SidePopupButton :is-eject="isEject" @toggle-eject="toggleEject" />
  </div>
</template>

<script setup lang="ts">
  import { ref, useTemplateRef, computed, watch, onMounted, onUnmounted } from "vue";
  import axios from "axios";
  import ProgressBar from "./ProgressBar.vue";
  import Visualizer from "./Visualizer.vue";
  import PlayControls from "./PlayControls.vue";
  import SongInfo from "./SongInfo.vue";
  import Playlist from "./Playlist.vue";
  import Lyrics from "./Lyrics.vue";
  import TimeDisplay from "./TimeDisplay.vue";
  import FunctionButtons from "./FunctionButtons.vue";
  import SidePopupButton from "./SidePopupButton.vue";

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
  const audioElement = useTemplateRef<HTMLAudioElement>("audioElement");
  const parsedLyrics = ref<LyricLine[]>([]);
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
  ];
  const isPlaying = ref(false);
  const isMuted = ref(false);
  const isEject = ref(false);
  const volume = ref(1);
  const playMode = ref<PlayMode>(PlayMode.ListLoop);
  const showPlaylist = ref(false);
  const showLyrics = ref(false);
  const showColorful = ref(false);
  const audioContext = ref<AudioContext | null>(null);
  const analyser = ref<AnalyserNode | null>(null);
  const source = ref<MediaElementAudioSourceNode | null>(null);
  const currentSongIndex = ref(0);
  const currentTime = ref(0);
  const currentDuration = ref(0);
  const currentSong = computed(() => playList[currentSongIndex.value]);
  const sourceBuffer = ref<SourceBuffer | null>(null);
  const pendingData: ArrayBuffer[] = [];
  //#endregion

  //#region 方法
  // 加载歌曲
  async function loadSong(index: number) {
    currentSongIndex.value = index;
    await fetchLyrics(currentSong.value.lyricsUrl);

    if (!audioContext) {
      setupAudioContext();
    }

    const response = await fetch(currentSong.value.musicUrl);
    const reader = response.body!.getReader();

    const mediaSource = new MediaSource();
    audioElement.value!.src = URL.createObjectURL(mediaSource);

    mediaSource.addEventListener("sourceopen", async () => {
      sourceBuffer.value = mediaSource.addSourceBuffer("audio/mpeg");
      sourceBuffer.value.addEventListener("updateend", appendNextChunk);

      let allChunksRead = false;

      async function readChunks() {
        while (true) {
          const { done, value } = await reader.read();
          if (done) {
            allChunksRead = true;
            break;
          }

          if (sourceBuffer.value!.updating || pendingData.length > 0) {
            pendingData.push(value.buffer);
          } else {
            appendChunk(value.buffer);
          }
        }
      }

      readChunks();

      function checkEndOfStream() {
        if (allChunksRead && !sourceBuffer.value!.updating && pendingData.length === 0) {
          mediaSource.endOfStream();
        } else {
          setTimeout(checkEndOfStream, 100);
        }
      }

      checkEndOfStream();
    });
  }

  function appendNextChunk() {
    if (pendingData.length > 0 && sourceBuffer.value && !sourceBuffer.value.updating) {
      const chunk = pendingData.shift();
      sourceBuffer.value.appendBuffer(chunk!);
    }
  }

  function appendChunk(chunk: ArrayBuffer) {
    if (sourceBuffer.value && !sourceBuffer.value.updating) {
      sourceBuffer.value.appendBuffer(chunk);
    } else {
      pendingData.push(chunk);
    }
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

  // 设置音频上下文
  function setupAudioContext() {
    if (!audioContext.value) {
      audioContext.value = new (window.AudioContext || (window as any).webkitAudioContext)();
    }
    if (audioContext.value.state === "suspended") {
      audioContext.value.resume();
    }
    if (!analyser.value) {
      analyser.value = audioContext.value.createAnalyser();
    }
    if (audioElement.value && !source) {
      source = audioContext.value.createMediaElementSource(audioElement.value);
      source.connect(analyser.value);
      analyser.value.connect(audioContext.value.destination);
    }
  }

  // /停
  async function togglePlay() {
    if (!audioElement.value) return;

    // 确保 AudioContext 已设置
    await setupAudioContext();

    if (isPlaying.value) {
      audioElement.value.pause();
    } else {
      try {
        await audioElement.value.play();
      } catch (error) {
        console.error("播放失败:", error);
      }
    }
    isPlaying.value = !isPlaying.value;
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

  // 切换播放模式
  function togglePlayMode() {
    const modes = Object.values(PlayMode);
    const currentIndex = modes.indexOf(playMode.value);
    const nextIndex = (currentIndex + 1) % modes.length;
    playMode.value = modes[nextIndex];
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

  // 切换炫彩
  function toggleColorful() {
    showColorful.value = !showColorful.value;
  }

  // 切换歌词
  function toggleLyrics() {
    showLyrics.value = !showLyrics.value;
  }

  // 切换播放列表
  function togglePlaylist() {
    showPlaylist.value = !showPlaylist.value;
  }

  // 切换弹出
  function toggleEject() {
    isEject.value = !isEject.value;
  }

  // 跳转
  function seek(newTime: number) {
    if (audioElement.value) {
      audioElement.value.currentTime = newTime;
    }
  }

  // 播放选中的歌曲
  function playSelectedSong(index: number) {
    loadSong(index);
    if (audioElement.value) {
      audioElement.value.play();
    }
  }

  // 播放
  function onPlay() {
    isPlaying.value = true;
  }

  // 暂停
  function onPause() {
    isPlaying.value = false;
  }

  // 可以播放
  function onCanPlay() {
    audioElement.value?.play().catch(e => console.error("音乐自动播放失败，请手动播放"));
  }

  // 播放结束
  function onEnded() {
    if (playMode.value === PlayMode.SingleLoop) {
      audioElement.value?.play();
    } else {
      nextSong();
    }
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

  function syncLyrics() {
    if (!audioElement.value) return;
    const currentTime = audioElement.value.currentTime;
    const currentLyricIndex = parsedLyrics.value.findIndex(
      (line, index) => currentTime >= line.time && (!parsedLyrics.value[index + 1] || currentTime < parsedLyrics.value[index + 1].time)
    );
  }

  watch(volume, newVolume => {
    if (audioElement.value) {
      audioElement.value.volume = newVolume;
      isMuted.value = newVolume === 0;
    }
  });

  onMounted(() => {
    if (audioElement.value) {
      loadSong(0);
      audioElement.value.addEventListener("timeupdate", updateProgress);
      audioElement.value.volume = volume.value;
    }
  });

  onUnmounted(() => {
    if (audioElement.value) {
      audioElement.value.removeEventListener("timeupdate", updateProgress);
    }
    if (audioContext) {
      audioContext.value?.close();
    }
  });

  // 更新进度的函数
  function updateProgress() {
    if (audioElement.value) {
      currentTime.value = audioElement.value.currentTime;
      currentDuration.value = audioElement.value.duration;
    }
  }
  //#endregion
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "./_variables.scss";

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

    &.eject {
      transform: translateX(-($x + $width));
    }

    .player {
      position: relative;
      display: flex;
      align-items: center;
      width: $player-width;
      height: $player-height;
      @include useBackdropFilter($base-bg-filter-blur);
      background-color: rgba($bg-color-primary, 0.5);
      border-radius: $border-radius;
      padding: 10px;

      .left-container,
      .right-container {
        position: relative;
        z-index: 1;
      }

      .left-container {
        flex: 0 auto;
        margin-right: 20px; // 增加右边距
        width: 220px; // 增加宽度以适应更大的封面
      }

      .right-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        justify-content: space-between;

        .right-top {
          display: flex;
          justify-content: space-between;
          align-items: center;
          padding-top: 10px;
        }

        .right-middle {
          // 样式将在 PlayControls 组件中定义
        }

        .right-bottom {
          // 样式将在 ProgressBar 和 TimeDisplay 组件中定义
        }
      }
    }
  }
</style>
