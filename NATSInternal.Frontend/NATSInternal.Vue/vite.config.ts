import VueJsx from "@vitejs/plugin-vue-jsx";
import VueMacros from "vue-macros/vite";
import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import ReactivityTransform from '@vue-macros/reactivity-transform/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    VueMacros({
      shortEmits: true,

      shortVmodel: {
        prefix: "$"
      },

      reactivityTransform: true,
      shortBind: true,

      defineProp: {
        edition: "kevinEdition"
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
          include: [/\.vue$/, /\.setup\.[cm]?[jt]sx?$/]
        }),

        vueJsx: VueJsx()
      }
    }),
    ReactivityTransform()
  ],
  server: {
    allowedHosts: ["frontend.khanhhuy.dev"]
  }
});