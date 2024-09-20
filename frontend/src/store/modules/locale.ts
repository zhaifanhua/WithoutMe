import type { LocaleSetting, LocaleType } from '#/config';
import { defineStore } from 'pinia';
import { appStore } from '@/store';
import { getStorageItem, setStorageItem } from '@/utils/storage/storage';
import { localeSetting } from '@/settings/locale';
import { LOCALE_KEY } from '@/enums/cacheEnum';

const lsLocaleSetting = getStorageItem<LocaleSetting>(LOCALE_KEY) || localeSetting;

interface LocaleState {
  localInfo: LocaleSetting;
}

export const useLocaleStore = defineStore({
  id: 'app-locale',
  state: (): LocaleState => ({
    localInfo: lsLocaleSetting,
  }),
  getters: {
    getShowPicker(): boolean {
      return !!this.localInfo?.showPicker;
    },
    getLocale(): LocaleType {
      return this.localInfo?.locale ?? 'zh_hans';
    },
  },
  actions: {
    /**
     * 设置多语言信息和缓存
     * @param info 多语言信息
     */
    setLocaleInfo(info: Partial<LocaleSetting>) {
      this.localInfo = { ...this.localInfo, ...info };
      setStorageItem(LOCALE_KEY, this.localInfo);
    },
    /**
     * 初始化多语言信息并从本地缓存加载现有配置
     */
    initLocale() {
      this.setLocaleInfo({
        ...localeSetting,
        ...this.localInfo,
      });
    },
  },
});

/**
 * 需要在设置之外使用
 */
export function useLocaleStoreWithOut() {
  return useLocaleStore(appStore);
}
