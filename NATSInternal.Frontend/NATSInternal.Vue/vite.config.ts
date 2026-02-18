import VueJsx from "@vitejs/plugin-vue-jsx";
import VueMacros from "vue-macros/vite";
import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import tailwindcss from "@tailwindcss/vite";
import { fileURLToPath, URL } from "node:url";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),
    VueMacros({
      shortEmits: true,

      shortVmodel: {
        prefix: "$",
      },

      reactivityTransform: false,
      shortBind: true,

      defineProp: {
        edition: "kevinEdition",
      },
      defineRender: true,
      defineEmit: true,
      setupComponent: true,
      setupSFC: true,
      exportProps: true,
      exportRender: true,
      chainCall: true,
      jsxDirective: {
        prefix: "",
      },
      booleanProp: true,

      plugins: {
        vue: vue({
          include: [/\.vue$/, /\.setup\.[cm]?[jt]sx?$/],
        }),
        vueJsx: VueJsx(),
      },
    }),
    // ReactivityTransform()
  ],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    allowedHosts: ["frontend.khanhhuy.dev", "frontend-workstation.khanhhuy.dev", "frontend-wsl.khanhhuy.dev"],
    strictPort: true,
    port: 5173, // Development server port,
    headers: {
      "Allow-Control-Allow-Origin": "*",
      "Allow-Control-Allow-Methods": "GET,POST,PUT,DELETE,OPTIONS",
      "Allow-Control-Allow-Headers": "Content-Type,Authorization",
    },
    proxy: {
      "^/api": {
        target: "http://localhost:5000",
        changeOrigin: true,
        secure: false,
        ws: true,
        rewrite: (path) => path.replace(/^\/api/, "/api"),
      },
      // "^/images": {
      //   target: "http://localhost:5000",
      //   changeOrigin: false,
      //   secure: false,
      //   ws: true,
      //   rewrite: (path) => path.replace(/^\/images/, "/images"),
      // },
      "^/proxyWebsocket": {
        target: "http://localhost:5175",
        changeOrigin: true,
        secure: false,
        ws: true,
        rewrite: (path) => path.replace(/^\/proxyWebsocket/, "/proxyWebsocket"),
      },
    },
  },
});
