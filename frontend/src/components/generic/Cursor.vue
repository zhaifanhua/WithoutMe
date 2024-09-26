<!-- 光标组件 -->

<template>
  <div class="cursor-container" v-show="cursorContainerDisplay">
    <div ref="cursor" class="cursor" :class="{ active: mouseEventConfig.isMouseDown }" :style="cursorStyle"></div>
    <div ref="cursorTrajectory" class="cursor-trajectory" :class="{ active: mouseEventConfig.isMouseDown }" :style="cursorTrajectoryStyle"></div>
  </div>
</template>

<script setup lang="ts">
  import { onMounted, reactive, ref, useTemplateRef, computed } from "vue";

  // 定义光标容器、是否显示、光标、光标轨迹、光标轨迹速度
  const cursorRef = useTemplateRef("cursor");
  const cursorTrajectoryRef = useTemplateRef("cursorTrajectory");
  const cursorContainerDisplay = ref<boolean>(false);
  const cursorSpeed = ref(0.16);

  // 定义光标及光标轨迹大小、相对坐标、光标坐标
  const cursorConfig = reactive({
    size: 8,
    axis: {
      top: 0,
      left: 0,
    },
    clientX: 0,
    clientY: 0,
    pageX: 0,
    pageY: 0,
  });
  const cursorTrajectoryConfig = reactive({
    size: 36,
    axis: {
      top: 0,
      left: 0,
    },
  });

  // 光标样式
  const cursorStyle = computed(() => ({
    "--cursor-size": cursorConfig.size + "px",
    top: cursorConfig.axis.top + "px",
    left: cursorConfig.axis.left + "px",
  }));
  const cursorTrajectoryStyle = computed(() => ({
    "--cursor-size": cursorTrajectoryConfig.size + "px",
    top: cursorTrajectoryConfig.axis.top + "px",
    left: cursorTrajectoryConfig.axis.left + "px",
  }));

  // 初始化函数
  const init = (): void => {
    // 移动端不显示
    cursorContainerDisplay.value = "ontouchstart" in window ? false : true;

    loop();
  };
  // 循环动画函数
  const loop = (): void => {
    cursorConfig.clientX = lerp(cursorConfig.clientX, cursorConfig.pageX, cursorSpeed.value);
    cursorConfig.clientY = lerp(cursorConfig.clientY, cursorConfig.pageY, cursorSpeed.value);
    cursorTrajectoryConfig.axis.top = lerpStyleTop(cursorConfig.clientY, cursorTrajectoryConfig.size);
    cursorTrajectoryConfig.axis.left = lerpStyleLeft(cursorConfig.clientX, cursorTrajectoryConfig.size);

    requestAnimationFrame(loop);
  };

  // 定义鼠标按下状态、鼠标按下光标及光标轨迹缩放比例、鼠标按下起始及结束位置
  const mouseEventConfig = reactive({
    isMouseDown: false,
    startY: 0,
    endY: 0,
  });

  // 鼠标事件
  const mouseMove = (e: MouseEvent): void => {
    cursorConfig.pageX = e.pageX;
    cursorConfig.pageY = e.pageY;
    cursorConfig.axis.top = lerpStyleTop(cursorConfig.pageY, cursorConfig.size);
    cursorConfig.axis.left = lerpStyleLeft(cursorConfig.pageX, cursorConfig.size);
  };
  const mouseDown = (e: MouseEvent): void => {
    mouseEventConfig.isMouseDown = true;
    cursorTrajectoryRef.value.classList.add("active");

    mouseEventConfig.isMouseDown = true;
    mouseEventConfig.startY = e.clientY;
  };
  const mouseUp = (e: MouseEvent): void => {
    mouseEventConfig.isMouseDown = false;
    cursorTrajectoryRef.value.classList.remove("active");

    mouseEventConfig.endY = e.clientY || mouseEventConfig.endY;
    if (
      mouseEventConfig.isMouseDown &&
      mouseEventConfig.startY &&
      Math.abs(mouseEventConfig.startY - mouseEventConfig.endY) >= cursorConfig.size + cursorTrajectoryConfig.size
    ) {
      mouseEventConfig.isMouseDown = false;
      mouseEventConfig.startY = null;
      mouseEventConfig.endY = null;
    }
  };

  // 计算光标与光标轨迹距离
  const lerp = (start: number, end: number, proportion: number): number => {
    return (1 - proportion) * start + proportion * end;
  };

  // 计算样式
  const lerpStyleTop = (y: number, size: number): number => {
    let top = y - size / 2;
    let scrollHeight = document.documentElement.scrollHeight;
    return top < 0 ? 0 : top > scrollHeight - size ? scrollHeight - size : top;
  };
  const lerpStyleLeft = (x: number, size: number): number => {
    let left = x - size / 2;
    let scrollWidth = document.documentElement.scrollWidth;
    return left < 0 ? 0 : left > scrollWidth - size ? scrollWidth - size : left;
  };

  // 在组件挂载时执行初始化和事件监听
  onMounted(() => {
    init();
    if (typeof document.body.addEventListener !== "undefined") {
      document.body.addEventListener("mousedown", mouseDown, false);
      document.body.addEventListener("mousemove", mouseMove, false);
      document.body.addEventListener("mouseup", mouseUp, false);
    }
  });
</script>

<style lang="scss">
  @import "@/styles/base/mixins.scss";
  // @import '@/styles/base/themes.scss';

  .cursor,
  .cursor-trajectory {
    width: var(--cursor-size);
    height: var(--cursor-size);
    position: absolute;
    pointer-events: none;
    transition: 0.25s ease-in-out;
    transition-property: scale, transform, background-color, border-color;
    scale: 1;
    @include useZindex(cursor);
  }

  .cursor {
    border-radius: 50%;
    border-color: transparent;
    // @include useTheme {
    //   background-color: getVar(text-color);
    // }
    background-color: black;

    &.active {
      scale: 4;
    }
  }

  .cursor-trajectory {
    border-radius: 30%;
    background-color: transparent;
    // @include useTheme {
    //   border: 2px solid getVar(text-color);
    // }
    border: 2px solid black;

    &.active {
      scale: 0.25;
    }
  }
</style>
