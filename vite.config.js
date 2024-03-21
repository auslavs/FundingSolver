import { defineConfig } from 'vite'
import preact from "@preact/preset-vite";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [preact()],
  base: 'https://cdn.jsdelivr.net/npm/@auslavs/funding-solver/file/dist/',
  build: {
    rollupOptions: {
      output: {
        manualChunks: false,
        entryFileNames: 'funding-solver.js',
      },
    },
  },
})
