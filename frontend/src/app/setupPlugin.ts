/**
 * 插件初始化
 */

import type { App } from "vue";

import TitleFocusPlugin from "@/plugins/titleFocus";

export function setupPlugins(app: App<Element>) {
  app.use(TitleFocusPlugin);
}
