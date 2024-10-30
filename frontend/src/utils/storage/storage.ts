/**
 * webStorage 操作
 */

/**
 * 存储数据到localStorage或sessionStorage
 * @param key 存储数据时使用的键名
 * @param value 要存储的数据
 * @param isSession 是否使用sessionStorage，默认为false，使用localStorage
 */
const setStorageItem = <T>(key: string, value: T, isSession = false): void => {
  const storage = isSession ? sessionStorage : localStorage;
  try {
    storage.setItem(key, JSON.stringify(value));
  } catch (e) {
    console.error("Storage is full or storage not available.", e);
  }
};

/**
 * 从localStorage或sessionStorage获取数据
 * @param key 获取数据时使用的键名
 * @param isSession 是否从sessionStorage获取，默认为false，从localStorage获取
 * @returns 存储的数据，如果键不存在或存储未初始化，则返回null
 */
const getStorageItem = <T>(key: string, isSession = false): T | null => {
  const storage = isSession ? sessionStorage : localStorage;
  return <T>JSON.parse(storage.getItem(key));
};

/**
 * 从localStorage或sessionStorage删除数据
 * @param key 删除数据时使用的键名
 * @param isSession 是否从sessionStorage删除，默认为false，从localStorage删除
 */
const removeStorageItem = (key: string, isSession = false): void => {
  const storage = isSession ? sessionStorage : localStorage;
  storage.removeItem(key);
};

/**
 * 清除所有localStorage或sessionStorage数据
 * @param isSession 是否清除sessionStorage，默认为false，清除localStorage
 */
const clearStorage = (isSession = false): void => {
  const storage = isSession ? sessionStorage : localStorage;
  storage.clear();
};

/**
 * 检查存储空间是否已满
 * @returns 存储空间是否已满
 */
const isStorageFull = (): boolean => {
  const testKey = "storageTest";
  try {
    localStorage.setItem(testKey, "1");
    localStorage.removeItem(testKey);
    return false;
  } catch (e) {
    return true;
  }
};

export { setStorageItem, getStorageItem, removeStorageItem, clearStorage, isStorageFull };
