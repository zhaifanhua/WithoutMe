/**
 * Composition API的一部分，它们是可复用的函数，用于在组件中组织和重用逻辑
 * 用于管理图片懒加载
 */

import lozad from 'lozad';
import { ref, onMounted, onBeforeUnmount, App, Plugin, Directive } from 'vue';

declare global {
  interface Window {
    $lozad: typeof lozad;
  }
}

export const LOZAD_CLASS_NAME = 'lozad';
export const LOADED_CLASS_NAME = 'loaded';

const lozadObserve = (target: lozad.Selector) => {
  const observer = lozad(target, { loaded: element => element.classList.add(LOADED_CLASS_NAME) });
  observer.observe();
  return observer;
};

export const useLozad = (options?: { elementor?: () => HTMLElement; immediate?: boolean; className?: string }) => {
  const container = ref<HTMLElement>();
  const observer = ref<lozad.Observer | null>(null);

  const observe = () => {
    const tagretClass = options?.className || LOZAD_CLASS_NAME;
    const targetElement = options?.elementor?.() || container.value;
    if (targetElement?.querySelectorAll) {
      const lozadElements = targetElement.querySelectorAll(`.${tagretClass}`);
      if (lozadElements?.length) {
        observer.value = lozadObserve(lozadElements);
      }
    }
  };

  const unobserve = () => {
    if (observer.value) {
      observer.value.observer.disconnect();
      observer.value = null;
    }
  };

  onMounted(() => {
    if (options?.immediate ?? true) {
      observe();
    }
  });

  onBeforeUnmount(() => {
    unobserve();
  });

  return { element: container, observer, observe, unobserve };
};

export const lozadDirective: Directive = {
  mounted(element) {
    element.__lozad_observer = lozadObserve(element);
  },
  beforeUnmount(element) {
    element.__lozad_observer?.observer.disconnect();
    element.__lozad_observer = null;
  },
};

export interface LozadPluginConfig {
  exportToGlobal?: boolean;
}
const install: Plugin = (app: App, config?: LozadPluginConfig) => {
  app.directive('lozad', lozadDirective);
  app.config.globalProperties.$lozad = lozad;
  if (config?.exportToGlobal) {
    window.$lozad = lozad;
  }
};

export default { install };
