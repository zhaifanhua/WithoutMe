import type { App } from 'vue';
import type { I18n, I18nOptions } from 'vue-i18n';
import { createI18n } from 'vue-i18n';
import { setHtmlPageLang, setLoadLocalePool } from './common';
import { localeSetting } from '@/settings/localeSetting';
import { useLocaleStoreWithOut } from '@/store/modules/locale';

const { fallback, availableLocales } = localeSetting;

export let i18n: ReturnType<typeof createI18n>;
// 创建应用国际化配置
async function createAppI18nOptions(): Promise<I18nOptions> {
  const localeStore = useLocaleStoreWithOut();
  const locale = localeStore.getLocale;
  const defaultLocal = await import(`./lang/${locale}.ts`);
  const message = defaultLocal.default?.message ?? {};

  setHtmlPageLang(locale);
  setLoadLocalePool(loadLocalePool => {
    loadLocalePool.push(locale);
  });

  return {
    legacy: false,
    locale: locale,
    fallbackLocale: fallback,
    availableLocales: availableLocales,
    sync: true, // 如果不想从全局范围继承语言环境，则需要将 i18n 组件选项的sync 设置为 false
    silentTranslationWarn: true, // true为警告关闭
    missingWarn: false,
    silentFallbackWarn: true,
    messages: {
      [locale]: message,
    },
  };
}

// 创建应用国际化
async function createAppI18n(): Promise<I18n> {
  const options = await createAppI18nOptions();
  return createI18n(options) as I18n;
}

// 配置国际化
export async function setupI18n(app: App) {
  const appI18n = await createAppI18n();
  app.use(appI18n);
}
