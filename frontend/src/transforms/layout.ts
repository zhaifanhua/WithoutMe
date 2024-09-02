import { RouteMeta } from 'vue-router';
import { LayoutColumn } from '/@/app/state';

export const getLayoutByRouteMeta = (routeMeta: RouteMeta) => {
  return routeMeta.layout === LayoutColumn.Wide
    ? LayoutColumn.Wide
    : routeMeta.layout === LayoutColumn.Full
      ? LayoutColumn.Full
      : LayoutColumn.Normal;
};
