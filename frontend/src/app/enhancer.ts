/**
 * 增强器
 */

import { computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useI18n, Language } from '/@/composables/i18n';
import { useTheme, ThemeTone } from '@/composables/theme';
import { useDefer, Defer } from '@/composables/defer';

const useEnhancer = () => {
  const route = useRoute();
  const router = useRouter();
  const language = useI18n();
  const theme = useTheme();
  const defer = useDefer();

  const isGreyTheme = computed(() => theme.theme.value === ThemeTone.Grey);
  const isDarkTheme = computed(() => theme.theme.value === ThemeTone.Dark);
  const isZhLang = computed(() => language.language.value === Language.Chinese);

  return {
    route,
    router,
    language,
    theme,
    defer,
    isGreyTheme,
    isDarkTheme,
    isZhLang,
  };
};

export { useEnhancer };
