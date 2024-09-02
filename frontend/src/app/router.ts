/**
 * 路由
 */

import { createRouter, createWebHistory, NavigationGuard, NavigationGuardNext, RouterHistory } from 'vue-router';
import { routes } from '@/routes';
import { LayoutColumn } from '@/app/state';
import { scrollToPageTop } from '@/utils/app/scroller';

import 'vue-router';

declare module 'vue-router' {
  interface RouteMeta {
    responsive?: boolean;
    layout?: LayoutColumn;
    validator?: (params: any) => Promise<any>;
    /** seconds | infinity | false: disabled  */
    ssrCacheTTL: number | false;
  }
}

enum CategorySlug {
  Code = 'code',
  Insight = 'insight',
}

interface RouterCreatorOptions {
  history: RouterHistory;
  beforeMiddleware?: NavigationGuard | NavigationGuard[];
  afterMiddleware?: NavigationGuardNext | NavigationGuardNext[];
}
const createUniversalRouter = (options: RouterCreatorOptions) => {
  const router = createRouter({
    routes,
    strict: true,
    history: options.history,
    linkActiveClass: 'link-active',
    scrollBehavior: () => scrollToPageTop(),
  });

  if (options.beforeMiddleware) {
    Array.isArray(options.beforeMiddleware)
      ? options.beforeMiddleware.forEach(router.beforeResolve)
      : router.beforeResolve(options.beforeMiddleware);
  }
  if (options.afterMiddleware) {
    Array.isArray(options.afterMiddleware)
      ? options.afterMiddleware.forEach(router.afterEach)
      : router.afterEach(options.afterMiddleware);
  }

  return router;
};

export { CategorySlug, RouterCreatorOptions, createUniversalRouter };
