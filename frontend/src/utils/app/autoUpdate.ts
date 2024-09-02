/**
 * 自动更新
 */

// 假设JavaScript文件的路径和名称类似于 'app.<hash>.js'
const scriptPathRegex = /(.*)\.(.{8})\.js$/;

// 从当前页面的JavaScript文件中提取hash值
const extractHash = (): string | null => {
  const scripts = document.querySelectorAll('script[src]');
  for (const script in scripts) {
    const match = script.src.match(scriptPathRegex);
    if (match) {
      return match[2];
    }
  }
  return null;
};

// 存储当前页面的JavaScript文件hash值
const storeCurrentHash = (hash: string): void => {
  localStorage.setItem('jsHash', hash);
};

// 检查是否有新版本的JavaScript文件
const checkForUpdates = (): void => {
  const currentHash = extractHash();
  if (currentHash) {
    const lastHash = localStorage.getItem('jsHash');
    if (lastHash !== currentHash) {
      notifyUserOfUpdate();
      storeCurrentHash(currentHash);
    }
  }
};

// 通知用户刷新页面的函数
const notifyUserOfUpdate = (): void => {
  if (confirm('A new version is available. Refresh the page to apply updates.')) {
    window.location.reload();
  }
};

// 在页面加载完成后检查更新
window.addEventListener('load', checkForUpdates);

export { checkForUpdates, notifyUserOfUpdate };
