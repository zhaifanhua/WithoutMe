/**
 * 插件初始化
 */

import type { App } from "vue";

import titleFocusPlugin from "@/plugins/titleFocus";
import themePlugin from "@/plugins/theme";
// import lozadPlugin from '@/plugins/lozad';

export async function setupPlugins(app: App<Element>) {
  app.use(titleFocusPlugin);
  app.use(themePlugin);
  // app.use(lozadPlugin);
}
