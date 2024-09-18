import { genMessage } from '../common';

const modules = import.meta.glob('./zh-hans/**/*.ts');
export default {
  message: {
    ...genMessage(modules, 'zh-hans'),
  },
  momentLocale: null,
  momentLocaleName: 'zh-hans',
};
