/**
 * 入口
 */
import App from '@/App.vue';
import { createApp } from 'vue';
import { setupStore } from '@/store';
import { setupI18n } from '@/locales';
import { setupRouter } from '@/router';

import titleFocusPlugin from '@/plugins/app/titleFocus';

async function bootstrap() {
  const app = createApp(App);

  setupStore(app);
  await setupI18n(app);
  setupRouter(app);

  app.use(titleFocusPlugin);

  app.mount('#app');
}

await bootstrap();
