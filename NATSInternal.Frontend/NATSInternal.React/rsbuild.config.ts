import { defineConfig } from "@rsbuild/core";
import { pluginReact } from "@rsbuild/plugin-react";

export default defineConfig({
  plugins: [pluginReact({
    fastRefresh: true
  })],
  source: {
    entry: {
      index: "./src/main.tsx"
    }
  },
  // html: {
  //   template: "./src/index.html"
  // },
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
      "@": "./src",
    },
  },
  server: {
    host: "0.0.0.0",
    port: 5173,
    historyApiFallback: true,
    publicDir: {
      name: "./public"
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
    }
  }
});