<!-- 主题切换组件 -->

<template>
  <div id="theme-switch" class="theme-switch">
    <button @click="toggleTheme" :disabled="isDisabled" title="Switch theme">
      <Icon :icon="themeIcon" />
    </button>
  </div>
</template>

<script setup lang="ts">
  import { computed, onMounted } from "vue";
  import { Icon } from "@iconify/vue";
  import { ThemeMode, ThemeTone } from "@/plugins/app/theme";
  // import { useEnhancer } from '@/app/enhancer';

  const { isGreyTheme, theme } = useEnhancer();
  const isDisabled = isGreyTheme;
  const themeIcon = computed(() => {
    const themeIconMap = {
      [ThemeTone.Light]: "ph:sun-fill",
      [ThemeTone.Dark]: "ph:moon-fill",
      [ThemeTone.Grey]: "mingcute:candles-fill",
    };
    return themeIconMap[theme.theme.value];
  });

  const toggleTheme = () => {
    theme.toggle();
  };

  onMounted(() => {
    nextTick(() => {
      theme.autoSet();
    });
  });
</script>

<style scoped lang="scss">
  @import "@/styles/base/mixins.scss";
  @import "@/styles/base/themes.scss";

  .theme-switch {
    grid-area: theme-switch;
    width: 30px;
    height: 30px;
    @include useFlexBox;
    @include useTransition;

    button {
      border: none;
      border-radius: 30%;
      background-color: transparent;
      background: transparent;
      cursor: pointer;
      width: 26px;
      height: 26px;
      @include useFlexBox;
      @include useTheme {
        color: getVar(text-color);
      }

      &:hover {
        @include useTheme {
          background-color: getVar(btn-color-hover);
        }
      }

      &:disabled {
        cursor: not-allowed;
      }
    }
  }
</style>
