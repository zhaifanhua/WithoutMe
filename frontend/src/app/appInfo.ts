/**
 * 应用信息
 */

// 在 vite.config.ts 中定义
const pkgAppInfo = __APP_INFO__;

export const appInfo = {
  name: pkgAppInfo.name,
  version: pkgAppInfo.version,
  revisionDate: pkgAppInfo.revisionDate,
  author: pkgAppInfo.author,
  license: pkgAppInfo.license,
  homepage: pkgAppInfo.homepage,
  repository: pkgAppInfo.repository,
  dependencies: pkgAppInfo.dependencies,
  devDependencies: pkgAppInfo.devDependencies,
};

// 初始化控制台信息
export const initConsole = () => {
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
