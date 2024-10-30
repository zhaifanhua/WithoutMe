import { resolve } from "node:path";
import { loadEnv, defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import pkg from "./package.json";

const appInfo: AppInfo = {
  name: pkg.name,
  version: pkg.version,
  revisionDate: pkg.revisionDate,
  author: pkg.author,
  license: pkg.license,
  homepage: pkg.homepage,
  repository: pkg.repository,
  dependencies: pkg.dependencies,
  devDependencies: pkg.devDependencies,
};

export default defineConfig(({ command, mode }) => {
  const envConfig = loadEnv(mode, process.cwd(), "");

  return {
    root: resolve(__dirname),
    define: {
      __APP_INFO__: JSON.stringify(appInfo),
    },
    plugins: [vue()],
    resolve: {
      alias: [
        {
          find: "@",
          replacement: resolve(__dirname, "src"),
        },
        {
          find: "/@",
          replacement: resolve(__dirname, "src"),
        },
        {
          find: "#",
          replacement: resolve(__dirname, "src/types"),
        },
        {
          find: "/#",
          replacement: resolve(__dirname, "src/types"),
        },
        {
          find: "~",
          replacement: resolve(__dirname, ""),
        },
        {
          find: "/~",
          replacement: resolve(__dirname, ""),
        },
      ],
    },
    server: {
      host: "0.0.0.0",
      port: 3000,
      open: true,
      proxy: {
        [envConfig.VITE_API_PROXY_URL]: {
          target: envConfig.VITE_API_ONLINE_URL,
          changeOrigin: true,
          rewrite: path => path.replace(/^\/api/, ""),
        },
      },
    },
    build: {
      outDir: "dist",
      assetsDir: "assets",
      sourcemap: false,
      rollupOptions: {
        output: {
          manualChunks(id) {
            if (id.includes("node_modules")) {
              return id.toString().split("node_modules/")[1].split("/")[0].toString();
            }
          },
        },
      },
    },
  };
});
