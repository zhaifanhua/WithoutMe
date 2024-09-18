/**
 * 此文件与其他声明文件不同，它是一个配置文件，用于配置项目的一些基本信息
 */

// 语言
export declare type LocaleType = 'zh_hans' | 'en_us';

export interface LocaleMenu {
  text: string;
  event: string;
}

export declare interface LocaleSetting {
  // 是否显示语言选择器
  showPicker: boolean;
  // 当前语言
  locale: LocaleType;
  // 默认语言
  fallback: LocaleType;
  // 可用区域设置
  availableLocales: LocaleType[];
}
