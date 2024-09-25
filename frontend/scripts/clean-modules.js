// clean.js
import fs from 'fs';
import path from 'path';


const directoriesToDelete = [
  'dist',
  'node_modules'
];

// 删除文件夹
directoriesToDelete.forEach(dir => {
  const dirPath = path.join(process.cwd(), dir);
  if (fs.existsSync(dirPath)) {
    fs.rmSync(dirPath, { recursive: true, force: true }); // 使用 fs.rm 替代 fs.rmdir
    console.log(`Deleted directory: ${dirPath}`);
  } else {
    console.log(`Directory not found: ${dirPath}`);
  }
});