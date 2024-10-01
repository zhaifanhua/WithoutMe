<!-- 可视化效果组件 -->

<template>
  <canvas ref="visualizer" class="visualizer" :style="{ opacity: show ? 0.8 : 0 }"></canvas>
</template>

<script setup lang="ts">
  import { useTemplateRef, onMounted, onUnmounted, watch } from "vue";

  const props = defineProps<{
    show: boolean;
    analyser: AnalyserNode | null;
  }>();

  const visualizer = useTemplateRef<HTMLCanvasElement | null>("visualizer");
  let animationFrameId: number | null = null;

  // 绘制可视化
  function drawVisualization() {
    if (!props.show || !visualizer.value || !props.analyser) return;

    const canvas = visualizer.value;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const width = canvas.width;
    const height = canvas.height;
    const bufferLength = props.analyser.frequencyBinCount;
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

      props.analyser!.getByteFrequencyData(dataArray);
      const average = dataArray.reduce((a, b) => a + b) / bufferLength;
      const intensity = average / 255;

      // 增加时间增量,使效果更快
      time += 0.03 * (1 + intensity * 2);

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

  watch(
    () => props.show,
    newValue => {
      if (newValue) {
        drawVisualization();
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
  );

  onMounted(() => {
    if (props.show) {
      drawVisualization();
    }
  });

  onUnmounted(() => {
    if (animationFrameId) {
      cancelAnimationFrame(animationFrameId);
    }
  });
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .visualizer {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    opacity: 0.8;
    border-radius: $border-radius;
    transition: opacity 0.3s ease;
    pointer-events: none; // 添加这一行
  }
</style>
