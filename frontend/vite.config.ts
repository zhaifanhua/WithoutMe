import { resolve } from 'node:path';
import { loadEnv, defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import AutoImport from 'unplugin-auto-import/vite';
import pkg from './package.json';

export default defineConfig(({ command, mode }) => {
  const envConfig = loadEnv(mode, process.cwd(), '');

  return {
    root: resolve(__dirname),
    define: {
      __APP_NAME__: JSON.stringify(pkg.name),
      __APP_VERSION__: JSON.stringify(pkg.version),
      __APP_REVISION_DATE__: JSON.stringify(pkg.revisionDate),
      __APP_DESCRIPTION__: JSON.stringify(pkg.description),
      __APP_AUTHOR__: JSON.stringify(pkg.author),
      __APP_LICENSE__: JSON.stringify(pkg.license),
      __APP_HOMEPAGE__: JSON.stringify(pkg.homepage),
      __APP_REPOSITORY__: JSON.stringify(pkg.repository),
    },
    plugins: [
      vue(),
      AutoImport({
        imports: ['vue'],
      }),
    ],
    resolve: {
      alias: [
        {
          find: '@',
          replacement: resolve(__dirname, 'src'),
        },
        {
          find: '/@',
          replacement: resolve(__dirname, 'src'),
        },
        {
          find: '#',
          replacement: resolve(__dirname, 'types'),
        },
        {
          find: '/#',
          replacement: resolve(__dirname, 'types'),
        },
        {
          find: '~',
          replacement: resolve(__dirname, ''),
        },
        {
          find: '/~',
          replacement: resolve(__dirname, ''),
        },
      ],
    },
    server: {
      host: '0.0.0.0',
      port: 3000,
      open: true,
      proxy: {
        [envConfig.VITE_API_PROXY_URL]: {
          target: envConfig.VITE_API_ONLINE_URL,
          changeOrigin: true,
          rewrite: path => path.replace(/^\/api/, ''),
        },
      },
    },
    build: {
      outDir: 'dist',
      assetsDir: 'assets',
      minify: 'terser',
      sourcemap: true,
      rollupOptions: {
        output: {
          manualChunks(id) {
            if (id.includes('node_modules')) {
              return id.toString().split('node_modules/')[1].split('/')[0].toString();
            }
          },
        },
      },
    },
  };
});
