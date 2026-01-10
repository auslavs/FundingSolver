import { defineConfig } from 'vite'
import preact from "@preact/preset-vite";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [preact()],
  base: '/FundingSolver/',
  build: {
    rollupOptions: {
      output: {
        manualChunks: false,
        entryFileNames: 'funding-solver.js',
      },
    },
  },
})
