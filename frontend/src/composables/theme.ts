/**
 * Composition API的一部分，它们是可复用的函数，用于在组件中组织和重用逻辑
 * 实现主题切换
 * 主题模式：哀悼模式、手动模式、系统模式、自动模式
 * 主题色调：暗色、亮色、灰色
 */

import { App, inject, ref, readonly } from 'vue';
import { getStorageItem, setStorageItem } from '@/utils/storage/storage';

const ThemeSymbol = Symbol('theme');
const THEME_STORAGE_KEY = 'theme';

// 主题模式
enum ThemeMode {
  Mourning = 'mourning',
  Manual = 'manual',
  System = 'system',
  Auto = 'auto',
}
// 主题色调
enum ThemeTone {
  Light = 'light',
  Dark = 'dark',
  Grey = 'grey',
}
// 系统主题查询
enum SystemThemeQuery {
  Light = '(prefers-color-scheme: light)',
  Dark = '(prefers-color-scheme: dark)',
}

interface ReturnTheme {
  mode: ThemeMode;
  tone: ThemeTone;
}

/**
 * 获取哀悼模式主题，客户端不可控制
 */
const getMourningTheme = (): ReturnTheme | null => {
  const mourningDay = {
    4.4: '清明节',
    5.12: '汶川大地震纪念日',
    12.13: '南京大屠杀死难者国家公祭日',
  };

  const myDate = new Date();
  const mon = myDate.getMonth() + 1;
  const date = myDate.getDate();
  const key = `${mon}.${date}`;
  if (Object.prototype.hasOwnProperty.call(mourningDay, key)) {
    const style = document.createElement('style');
    style.innerHTML = `html {filter: grayscale(100%);}`;
    document.head.appendChild(style);
    return {
      mode: ThemeMode.Mourning,
      tone: ThemeTone.Grey,
    };
  }
  return null;
};

/**
 * 获取手动模式主题
 * @returns ReturnTheme
 */
const getManualTheme = (): ReturnTheme | null => {
  const manualTheme = getStorageItem<ReturnTheme>(THEME_STORAGE_KEY);
  if (manualTheme) {
    return {
      mode: ThemeMode.Manual,
      tone: manualTheme.tone === ThemeTone.Dark ? ThemeTone.Dark : ThemeTone.Light,
    };
  }
  return null;
};
/**
 * 获取系统主题
 * @returns ReturnTheme
 */
const getSystemTheme = (): ReturnTheme | null => {
  if (window.matchMedia(SystemThemeQuery.Dark).matches) {
    return {
      mode: ThemeMode.System,
      tone: ThemeTone.Dark,
    };
  } else if (window.matchMedia(SystemThemeQuery.Light).matches) {
    return {
      mode: ThemeMode.System,
      tone: ThemeTone.Light,
    };
  }
  return null;
};
/**
 * 获取自动主题（根据时间）
 * @returns ReturnTheme
 */
const getAutoTheme = (): ReturnTheme => {
  const hour = new Date().getHours();
  // 定义 6:00 - 18:00 为白天
  if (hour < 6 || hour >= 18) {
    return {
      mode: ThemeMode.Auto,
      tone: ThemeTone.Dark,
    };
  } else {
    return {
      mode: ThemeMode.Auto,
      tone: ThemeTone.Light,
    };
  }
};

/**
 * 获取客户端本地主题
 * @returns ThemeMode
 */
const getClientLocalTheme = (): ReturnTheme => {
  // 优先级：手动模式 > 系统模式 > 自动模式
  const manualTheme = getManualTheme();
  if (manualTheme) return manualTheme;
  const systemTheme = getSystemTheme();
  if (systemTheme) return systemTheme;
  return getAutoTheme();
};

const themes = [ThemeTone.Light, ThemeTone.Dark];
/**
 * 创建主题
 * @param initTheme 初始主题
 * @returns themeState
 */
const createTheme = (initTheme: ThemeTone) => {
  const theme = ref<ThemeTone>(
    initTheme === ThemeTone.Dark ? ThemeTone.Dark : initTheme === ThemeTone.Light ? ThemeTone.Light : ThemeTone.Grey
  );

  /**
   * 自动设置
   */
  const autoSet = () => {
    let currentTheme: ReturnTheme | null = {
      mode: ThemeMode.Auto,
      tone: ThemeTone.Light,
    };

    // 优先级：哀悼模式 > 系统模式 > 自动模式
    const mourningTheme = getMourningTheme();
    if (mourningTheme) {
      currentTheme = mourningTheme;
    } else {
      const manualTheme = getManualTheme();
      if (manualTheme && manualTheme.mode === ThemeMode.Manual) {
        currentTheme = manualTheme;
      } else {
        let systemTheme = getSystemTheme();
        if (systemTheme) {
          currentTheme = systemTheme;
        } else {
          let autoSetTheme = getAutoTheme();
          currentTheme = autoSetTheme;
        }
      }
    }

    theme.value = currentTheme.tone;
    setStorageItem<ReturnTheme>(THEME_STORAGE_KEY, currentTheme);
  };
  /**
   * 手动设置
   */
  const manualSet = (newTheme: ThemeTone) => {
    let currentTheme: ReturnTheme | null = {
      mode: ThemeMode.Manual,
      tone: ThemeTone.Light,
    };
    if (themes.includes(newTheme) && newTheme !== theme.value) {
      currentTheme.tone = newTheme;
    }
    theme.value = newTheme;
    setStorageItem<ReturnTheme>(THEME_STORAGE_KEY, currentTheme);
  };
  /**
   * 绑定客户端系统
   */
  const bindClientSystem = () => {
    window
      .matchMedia(SystemThemeQuery.Dark)
      .addEventListener('change', ({ matches }) => matches && manualSet(ThemeTone.Dark));
    window
      .matchMedia(SystemThemeQuery.Light)
      .addEventListener('change', ({ matches }) => matches && manualSet(ThemeTone.Light));
  };
  /**
   * 切换主题
   * @returns void
   */
  const toggle = () => manualSet(theme.value === ThemeTone.Dark ? ThemeTone.Light : ThemeTone.Dark);

  const themeState = {
    theme: readonly(theme),
    autoSet,
    manualSet,
    toggle,
    bindClientSystem,
  };

  return {
    ...themeState,
    install(app: App) {
      app.provide(ThemeSymbol, themeState);
    },
  };
};

const useTheme = () => {
  return inject(ThemeSymbol) as Omit<ReturnType<typeof createTheme>, 'install'>;
};

export { ThemeMode, ThemeTone, THEME_STORAGE_KEY, getClientLocalTheme, createTheme, useTheme };
