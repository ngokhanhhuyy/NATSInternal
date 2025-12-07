import { defineConfig } from "@rsbuild/core";
import { pluginBabel } from "@rsbuild/plugin-babel";
import { pluginSolid } from "@rsbuild/plugin-solid";

export default defineConfig({
  plugins: [
    pluginBabel({
      include: /\.(?:jsx|tsx)$/,
    }),
    pluginSolid()
  ],
  source: {
    entry: {
      index: "./src/main.tsx"
    }
  },
  output: {
    filename: {
      js: "[name].[hash].js",
      css: "[name].[hash].css"
    },
    cssModules: {
      namedExport: false,
    },
  },
  performance: {
    buildCache: false,
    chunkSplit: {
      strategy: "all-in-one"
    }
  },
  resolve: {
    aliasStrategy: "prefer-alias",
    alias: {
      "@": "./src"
    },
  },
  server: {
    host: "0.0.0.0",
    port: 5173,
    historyApiFallback: true,
    publicDir: {
      name: "./src/assets"
    },
    headers: {
      "Allow-Control-Allow-Origin": "*",
      "Allow-Control-Allow-Methods": "GET,POST,PUT,DELETE,OPTIONS",
      "Allow-Control-Allow-Headers": "Content-Type,Authorization",
    },
    proxy: {
      "/api": {
        target: "http://localhost:5000",
        pathRewrite: { "^/api": "/api" },
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      "/images": {
        target: "http://localhost:5000",
        pathRewrite: { "^/images": "/images" },
        changeOrigin: true,
        secure: false
      }
    }
  },
  dev: {
    client: {
      // host: "frontend-workstation.khanhhuy.dev",

    }
  }
});