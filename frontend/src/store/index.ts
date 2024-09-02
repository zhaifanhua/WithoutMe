/**
 *
 */

import { createPinia } from 'pinia';
import { useStores } from '@/store/_hook';
import { GlobalState } from '@/app/state';
import { getSSRStateValue } from '@/universal';

export interface UniversalStoreConfig {
  globalState: GlobalState;
}

export const createUniversalStore = (config: UniversalStoreConfig) => {
  const pinia = createPinia();
  const fetchBasicStore = () => {
    const stores = useStores(pinia);
    const initFetchTasks = [];
    return Promise.all(initFetchTasks);
  };

  return {
    get stores() {
      return useStores(pinia);
    },
    state: pinia.state,
    install: pinia.install,
    serverPrefetch: fetchBasicStore,
    hydrate() {
      const contextStore = getSSRStateValue('store');
      if (contextStore) {
        // The data passed from the SSR server is used to initialize the pinia
        pinia.state.value = contextStore;
      } else {
        // fallback: when SSR page error
        fetchBasicStore();
      }
    },
  };
};
