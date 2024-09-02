/**
 * Cookie 操作
 */

/**
 * 设置Cookie
 * @param name Cookie的名称
 * @param value Cookie的值
 * @param days 过期时间（天数）。如果设置为负数，则删除Cookie
 */
const setCookie = (name: string, value: string, days: number): void => {
  let expires = '';
  if (days && days > 0) {
    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    expires = `; expires=${date.toUTCString()}`;
  }
  document.cookie = `${name}=${value}${expires}; path=/`;
};

/**
 * 获取Cookie的值
 * @param name Cookie的名称
 * @returns Cookie的值，如果不存在则返回null
 */
const getCookie = (name: string): string | null => {
  const nameEQ = `${name}=`;
  const ca = document.cookie.split(';');
  for (let i = 0; i < ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) === ' ') c = c.substring(1, c.length);
    if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
  }
  return null;
};

/**
 * 删除Cookie
 * @param name Cookie的名称
 */
const deleteCookie = (name: string): void => {
  setCookie(name, '', -1);
};

/**
 * 检查浏览器是否启用了Cookie
 * @returns {boolean} - 如果浏览器启用了Cookie，则返回true，否则返回false
 */
const isCookiesEnabled = (): boolean => {
  const testKey = 'cookieTest';
  setCookie(testKey, '1', 1);
  if (getCookie(testKey) === '1') {
    deleteCookie(testKey);
    return true;
  }
  return false;
};

export { setCookie, getCookie, deleteCookie, isCookiesEnabled };
