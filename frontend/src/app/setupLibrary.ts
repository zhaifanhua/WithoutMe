/**
 * 基础库初始化
 */

import type { App } from "vue";
import { setupStore } from "@/store";
import { setupI18n } from "@/locales";
import { setupRouter } from "@/router";

export async function setupLibraries(app: App<Element>) {
  setupStore(app);
  await setupI18n(app);
  setupRouter(app);
}
