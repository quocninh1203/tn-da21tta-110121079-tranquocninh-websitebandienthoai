import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
    server: {
    host: '0.0.0.0', // Cho phép truy cập từ thiết bị ngoài
    port: 5173,       // hoặc port khác nếu bạn dùng
    allowedHosts: "all", // Cho phép truy cập từ mọi host
  }
})

