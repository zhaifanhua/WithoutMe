/**
 * IndexedDB 操作
 */

interface DBItem {
  id: number;
  [key: string]: any;
}

const dbName = 'myDatabase';
const storeName = 'myStore';
const version = 1;

// 打开数据库并初始化
const openDB = async (): Promise<IDBDatabase> => {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(dbName, version);
    request.onerror = event => {
      reject('Database error: ' + event.target);
    };
    request.onupgradeneeded = event => {
      const db = request.result;
      if (!db.objectStoreNames.contains(storeName)) {
        db.createObjectStore(storeName, { keyPath: 'id', autoIncrement: true });
      }
    };
    request.onsuccess = event => {
      resolve(request.result);
    };
  });
};

// 添加数据
const addData = async (data: DBItem): Promise<void> => {
  const db = await openDB();
  const transaction = db.transaction([storeName], 'readwrite');
  const store = transaction.objectStore(storeName);
  const request = store.add(data);
  request.onsuccess = () => console.log('Data added to the database');
  request.onerror = event => console.error('Error adding data: ', event.target);
};

// 获取数据
const getData = async (id: number): Promise<DBItem | undefined> => {
  const db = await openDB();
  const transaction = db.transaction([storeName], 'readonly');
  const store = transaction.objectStore(storeName);
  const request = store.get(id);
  request.onsuccess = () => request.result;
  request.onerror = event => console.error('Error getting data: ', event.target);
  return new Promise(resolve => {
    transaction.oncomplete = () => resolve(request.result);
  });
};

// 更新数据
const updateData = async (id: number, newData: Partial<DBItem>): Promise<void> => {
  const db = await openDB();
  const transaction = db.transaction([storeName], 'readwrite');
  const store = transaction.objectStore(storeName);
  const request = store.get(id);

  request.onsuccess = () => {
    const data = request.result;
    if (data) {
      // 合并新数据和旧数据
      const updatedData = { ...data, ...newData };
      const updateRequest = store.put(updatedData);
      updateRequest.onsuccess = () => {
        console.log('Data updated in the database');
      };
      updateRequest.onerror = event => {
        console.error('Error updating data: ', event.target);
      };
    }
  };
  request.onerror = event => {
    console.error('Error getting data to update: ', event.target);
  };
};

// 删除数据
const deleteData = async (id: number): Promise<void> => {
  const db = await openDB();
  const transaction = db.transaction([storeName], 'readwrite');
  const store = transaction.objectStore(storeName);
  const request = store.delete(id);
  request.onsuccess = () => console.log('Data deleted from the database');
  request.onerror = event => console.error('Error deleting data: ', event.target);
};

// 清空数据库
const clearStore = async (): Promise<void> => {
  const db = await openDB();
  const transaction = db.transaction([storeName], 'readwrite');
  const store = transaction.objectStore(storeName);
  const request = store.clear();
  request.onsuccess = () => console.log('Store cleared');
  request.onerror = event => console.error('Error clearing store: ', event.target);
};

export { openDB, addData, getData, updateData, deleteData, clearStore };
