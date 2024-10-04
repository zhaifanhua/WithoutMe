<!-- 进度条组件 -->

<template>
  <div class="progress-bar">
    <div class="slider">
      <input type="range" :min="min" :max="max" :value="value" @input="updateValue" :style="progressBarStyle" />
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from "vue";

  const props = defineProps({
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    value: { type: Number, required: true },
    color: { type: String, default: "var(--bg-color-active)" },
    backgroundColor: { type: String, default: "var(--bg-color-inactive)" },
  });

  const emit = defineEmits<{
    (e: "update:value", value: number): void;
  }>();

  function updateValue(event: Event) {
    const target = event.target as HTMLInputElement;
    const newValue = Number(target.value);
    emit("update:value", newValue);
  }

  // 添加 progressBarStyle 计算属性
  const progressBarStyle = computed(() => {
    const percentage = ((props.value - props.min) / (props.max - props.min)) * 100;
    return {
      background: `linear-gradient(to right, ${props.color} ${percentage}%, ${props.backgroundColor} ${percentage}%)`,
    };
  });
</script>

<style scoped lang="scss">
  @import "./_variables.scss";

  .progress-bar {
    width: 100%; // 确保进度条占满父容器
    height: 8px; // 设置进度条高度

    .slider {
      input[type="range"] {
        -webkit-appearance: none; // 移除默认样式
        appearance: none;
        width: 100%;
        height: 8px; // 确保滑块的高度与进度条一致
        border-radius: 4px;
        outline: none;

        &::-webkit-slider-thumb {
          -webkit-appearance: none; // 移除默认样式
          appearance: none;
          height: 12px; // 增加滑块的高度
          width: 12px; // 增加滑块的宽度
          border-radius: 50%;
          background-color: var(--bg-color-active);
          cursor: pointer;
          margin-top: -2; // 调整滑块位置以对齐进度条
        }
        &::-moz-range-thumb {
          height: 12px; // 增加滑块的高度
          width: 12px; // 增加滑块的宽度
          border-radius: 50%;
          background-color: var(--bg-color-active);
          cursor: pointer;
        }
      }
    }
  }
</style>
