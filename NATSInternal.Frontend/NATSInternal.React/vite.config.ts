import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import { fileURLToPath, URL } from "node:url";

// https://vite.dev/config/
export default defineConfig({
  plugins: [tailwindcss(), react()],
  
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    }
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
      "^/images": {
        target: "http://localhost:5000",
        changeOrigin: false,
        secure: false,
        ws: true,
        rewrite: (path) => path.replace(/^\/images/, "/images"),
      },
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
