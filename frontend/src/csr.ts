/**
 * 客户端渲染入口
 */

import { computed, watch } from 'vue';
import { createWebHistory } from 'vue-router';
import { createMainApp } from '@/app/main';
import lozad from '@/composables/lozad';
import { createDefer } from '@/composables/defer';
// import { createMusic } from '@/composables/music';
// import { createPopup } from '@/composables/popup';
import { getSSRStateValue, getSSRContextData, getSSRContextValue } from '@/universal';
import { Language, LanguageKey } from '@/language';
import { getLayoutByRouteMeta } from '@/transforms/layout';

// app
const { app, router, head, globalState, i18n, store, getGlobalHead } = createMainApp({
  historyCreator: createWebHistory,
  language: navigator.language,
  userAgent: navigator.userAgent,
  region: getSSRStateValue('region')!,
  layout: getSSRStateValue('layout')!,
  theme: getSSRStateValue('theme')!,
  error: getSSRContextValue('error'),
});

// services
const defer = createDefer();
// const popup = createPopup();
// const music = createMusic({ delay: 668, continueNext: true });

// plugins & services
// app.use(music);
app.use(lozad, { exportToGlobal: true });
app.use(defer, { exportToGlobal: true });
// app.use(popup, { exportToGlobal: true });

const globalHead = computed(() => getGlobalHead());
const mainHead = head.push(globalHead, { mode: 'client' });
watch(globalHead, newValue => mainHead.patch(newValue));

// router ready -> mount
router.isReady().finally(() => {
  // UI layout: set UI layout by route (for SPA)
  globalState.setLayoutColumn(getLayoutByRouteMeta(router.currentRoute.value.meta));
  // mount (force hydrate)
  app.mount('#app', true).$nextTick(() => {
    // set hydration state
    globalState.setHydrate();
    // reset: i18n language
    i18n.set(globalState.userAgent.isZhUser ? Language.Chinese : Language.English);
    // init user identity state
    // store.stores.identity.initOnClient();
  });
});
