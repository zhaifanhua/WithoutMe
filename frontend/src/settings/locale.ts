import type { LocaleType, LocaleMenu, LocaleSetting } from "@/types/config";

export const LOCALE: { [key: string]: LocaleType } = {
  ZH_CN: "zh-cn",
  EN_US: "en-us",
};

// 语言环境设置
export const localeSetting: LocaleSetting = {
  showPicker: true,
  locale: LOCALE.ZH_CN,
  fallback: LOCALE.ZH_CN,
  availableLocales: [LOCALE.ZH_CN, LOCALE.EN_US],
};

// 语言环境列表
export const localeList: LocaleMenu[] = [
  {
    text: "简体中文",
    event: LOCALE.ZH_CN,
  },
  {
    text: "English",
    event: LOCALE.EN_US,
  },
];
