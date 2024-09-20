/**
 * 主入口文件
 */

import { createApp } from 'vue';
import App from './App.vue';
import { setupLibraries } from './setupLibrary';
import { setupPlugins } from './setupPlugin';

export async function createMainApp() {
  const app = createApp(App);
  await setupLibraries(app);
  await setupPlugins(app);
  app.mount('#app');
}
