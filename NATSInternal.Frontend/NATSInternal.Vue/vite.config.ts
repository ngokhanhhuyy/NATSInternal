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
      jsxDirective: true,
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
    allowedHosts: ["frontend.khanhhuy.dev"],
  },
});
