/**
 * 打印应用信息
 */

import { appInfo } from '@/app/appInfo';

// 初始化控制台信息
const initConsole = () => {
  const versionConsole = `The Current Version: v${appInfo.version}_${appInfo.revisionDate}`;
  const authorConsole = `Designed and developed by: ${appInfo.author.name}`;
  consoleInfo(versionConsole);
  consoleInfo(authorConsole);
};

// 打印
const consoleInfo = (info: string) => {
  console.info(
    `%c ${info}`,
    'color:#fff; background: linear-gradient(270deg, #986fee, #8695e6, #68b7dd, #18d7d3); padding: 8px 20px; border-radius: 0 10px 0 10px'
  );
};

export { initConsole };
