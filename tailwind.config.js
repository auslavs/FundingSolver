const defaultTheme = require('tailwindcss/defaultTheme')

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./index.html", "./src/**/*.{fs,js,jsx}"],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'Segoe UI', ...defaultTheme.fontFamily.sans]
      },
    },
  },
  plugins: [],
}