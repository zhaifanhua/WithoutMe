import type { LocaleType, LocaleMenu, LocaleSetting } from '#/config';

export const LOCALE: { [key: string]: LocaleType } = {
  ZH_HANS: 'zh_hans',
  EN_US: 'en_us',
};

// 语言环境设置
export const localeSetting: LocaleSetting = {
  showPicker: true,
  locale: LOCALE.ZH_HANS,
  fallback: LOCALE.ZH_HANS,
  availableLocales: [LOCALE.ZH_HANS, LOCALE.EN_US],
};

// 语言环境列表
export const localeList: LocaleMenu[] = [
  {
    text: '简体中文',
    event: LOCALE.ZH_HANS,
  },
  {
    text: 'English',
    event: LOCALE.EN_US,
  },
];
