<!-- 语言切换组件 -->

<template>
  <div id="lang-switch" class="lang-switch">
    <button @click="toggleLanguage" title="Switch language">
      <!-- <Icon :icon="languageIcon" /> -->
    </button>
  </div>
</template>

<script setup lang="ts">
  import { computed } from "vue";
  import { Icon } from "@iconify/vue";
  // import { Language } from '@/language';
  // import { useEnhancer } from '@/app/enhancer';

  // const { language } = useEnhancer();

  const languageIcon = computed(() => {
    // const languageIconMap = {
    //   [Language.Chinese]: 'uil:letter-chinese-a',
    //   [Language.English]: 'uil:letter-english-a',
    // };
    // return languageIconMap[language.language.value];
  });

  const toggleLanguage = () => {
    // language.toggle();
  };
</script>

<style scoped lang="scss">
  @use "@/styles/base/mixins" as mixins;
  @use "@/styles/base/themes" as themes;

  .lang-switch {
    grid-area: lang-switch;
    width: 30px;
    height: 30px;
    @include mixins.useFlexBox;
    @include mixins.useTransition;

    button {
      border: none;
      border-radius: 30%;
      background-color: transparent;
      background: transparent;
      cursor: pointer;
      width: 26px;
      height: 26px;
      @include mixins.useFlexBox;
      @include themes.useTheme {
        color: themes.getVar(text-color);
      }

      &:hover {
        @include themes.useTheme {
          background-color: themes.getVar(btn-color-hover);
        }
      }
    }
  }
</style>
