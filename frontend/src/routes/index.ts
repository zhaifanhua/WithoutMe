import { RouteRecordRaw } from 'vue-router';
import { HttpCode } from '@/api';
import { LanguageKey } from '@/language';
import Home from '@/views/Home.vue';
import About from '@/views/About.vue';
import Privacy from '@/views/Privacy.vue';

export enum RouteName {
  Home = 'Home',
  About = 'About',
  Error = 'Error',
}

export const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: RouteName.Home,
    component: Home,
    meta: {
      responsive: true,
      ssrCacheTTL: 60 * 2,
    },
  },
  {
    path: '/about',
    name: RouteName.About,
    component: About,
    meta: {
      responsive: true,
      ssrCacheTTL: 60 * 60 * 4,
    },
  },
  {
    path: '/privacy',
    name: 'Privacy',
    component: Privacy,
    meta: {
      responsive: true,
      ssrCacheTTL: 60 * 60 * 4,
    },
  },
  {
    path: '/:error(.*)',
    name: RouteName.Error,
    component: {},
    meta: {
      ssrCacheTTL: false,
      async validator({ i18n }) {
        return Promise.reject({
          code: HttpCode.NOT_FOUND,
          message: i18n.t(LanguageKey.NOT_FOUND),
        });
      },
    },
  },
];
