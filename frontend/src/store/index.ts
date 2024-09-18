import type { App } from 'vue';
import { createPinia } from 'pinia';

// 创建应用状态管理
export const appStore = createPinia();

// 配置状态管理
export function setupStore(app: App<Element>) {
  app.use(appStore);
}
