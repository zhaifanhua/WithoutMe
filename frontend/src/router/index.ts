import type { App } from 'vue';
import type { RouteRecordRaw } from 'vue-router';
import { createRouter, createWebHashHistory } from 'vue-router';
import { basicRoutes } from './routes/basic';

// 路由白名单，其中应该包含基本静态路由
const WHITE_NAME_LIST: string[] = [];
const getRouteNames = (array: RouteRecordRaw[]) =>
  array.forEach(item => {
    WHITE_NAME_LIST.push(item.name as string);
    getRouteNames(item.children || []);
  });
getRouteNames(basicRoutes);

// 创建应用路由
export const appRouter = createRouter({
  history: createWebHashHistory(import.meta.env.VITE_PUBLIC_PATH),
  routes: basicRoutes,
  strict: true,
});

// 重置路由
export function resetRouter() {
  appRouter.getRoutes().forEach(route => {
    const { name } = route;
    if (name && !WHITE_NAME_LIST.includes(name as string)) {
      appRouter.hasRoute(name) && appRouter.removeRoute(name);
    }
  });
}

// 配置路由
export function setupRouter(app: App<Element>) {
  app.use(appRouter);
}
