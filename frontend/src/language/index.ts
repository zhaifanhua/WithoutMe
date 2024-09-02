import type { I18nLanguage, I18nValueRender } from '@/composables/i18n';
import { LanguageKey } from '@/language/key';
import { zhLangMap } from '@/language/lang-zh';
import { enLangMap } from '@/language/lang-en';

export { LanguageKey } from '@/language/key';

export type LanguageValue = string | I18nValueRender;
export type LanguageMap = Record<LanguageKey, LanguageValue>;

export enum Language {
  English = 'en',
  Chinese = 'zh',
}

export const languages: I18nLanguage<LanguageKey>[] = [
  {
    code: Language.Chinese,
    iso: 'zh-CN',
    name: '简体中文',
    data: zhLangMap,
  },
  {
    code: Language.English,
    iso: 'en-US',
    name: 'English',
    data: enLangMap,
  },
];
