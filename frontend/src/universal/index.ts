import { isServer, isClient } from '@/app/siteInfo';
export * from './context';
export * from './prefetch';
export * from './hydration';

export const onClient = (callback: any) => {
  isClient && callback();
};

export const onServer = (callback: any) => {
  isServer && callback();
};
