/**
 * 全局状态管理
 */

import { App, inject, ref, computed, reactive, readonly } from 'vue';
import { HttpCode } from '@/api/http-code';
import { uaParser, isZhUser } from '@/transforms/ua';
import { onClient } from '@/universal';
import logger from '@/utils/app/logger';

type RenderErrorValue = RenderError | null;
interface RenderError {
  code: number;
  message: string;
}

enum LayoutColumn {
  Normal = 0,
  Wide = 1, // 3 column
  Full = 2, // full page
}

interface GlobalRawState {
  renderError: RenderErrorValue;
  userAgent: string;
  language: string;
  layout: LayoutColumn;
}

interface GlobalStateConfig {
  userAgent: string;
  language: string;
  layout: LayoutColumn;
  error?: RenderErrorValue;
}

const GlobalStateSymbol = Symbol('globalState');
type GlobalState = ReturnType<typeof createGlobalState>;
const createGlobalState = (config: GlobalStateConfig) => {
  // Render error
  const renderError = ref<RenderErrorValue>(config.error ?? null);
  const defaultError = { code: HttpCode.INVALID_ERROR };
  const setRenderError = (error: any) => {
    onClient(() => {
      logger.failure('error:', error);
    });
    if (!error) {
      // clear error
      renderError.value = null;
    } else if (error instanceof Error) {
      // new Error
      renderError.value = {
        code: (error as any).code ?? defaultError.code,
        message: error.message,
      };
    } else if (typeof error === 'string') {
      // error message
      renderError.value = {
        ...defaultError,
        message: error,
      };
    } else {
      // error object -> axios | component
      renderError.value = {
        ...error,
        code: error.status || error.code || defaultError.code,
      };
    }
  };

  // Hydrated
  const isHydrated = ref(false);
  const setHydrate = () => {
    isHydrated.value = true;
  };

  // UserAgent & device info
  const userAgent = reactive({
    original: config.userAgent,
    language: config.language,
    isZhUser: isZhUser(config.language),
    ...uaParser(config.userAgent),
  });

  // UI layout
  const layoutValue = ref(config.layout);
  const layoutColumn = computed(() => ({
    layout: layoutValue.value,
    isNormal: layoutValue.value === LayoutColumn.Normal,
    isWide: layoutValue.value === LayoutColumn.Wide,
    isFull: layoutValue.value === LayoutColumn.Full,
  }));
  const setLayoutColumn = (layout: LayoutColumn) => {
    if (layout !== layoutValue.value) {
      layoutValue.value = layout;
    }
  };

  // Global switchers
  const switcher = reactive({
    sponsor: false,
    feedback: false,
    statement: false,
  });
  const toggleSwitcher = (key: keyof typeof switcher, value: boolean) => {
    switcher[key] = value;
  };

  // export state
  const toRawState = (): GlobalRawState => ({
    renderError: renderError.value,
    userAgent: userAgent.original,
    language: userAgent.language,
    layout: layoutValue.value,
  });

  const globalState = {
    toRawState,
    // Render error state
    renderError: readonly(renderError),
    setRenderError,
    // Hydrate state
    isHydrated: readonly(isHydrated),
    setHydrate,
    // Layout state
    layoutColumn,
    setLayoutColumn,
    // Device state
    userAgent: readonly(userAgent),
    // Global switchers
    switcher: readonly(switcher),
    toggleSwitcher,
  };

  return {
    ...globalState,
    install(app: App) {
      app.provide(GlobalStateSymbol, globalState);
    },
  };
};

const useGlobalState = (): GlobalState => {
  return inject(GlobalStateSymbol) as GlobalState;
};

export {
  RenderError,
  LayoutColumn,
  GlobalRawState,
  GlobalStateConfig,
  RenderErrorValue,
  GlobalState,
  createGlobalState,
  useGlobalState,
};
