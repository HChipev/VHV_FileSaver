/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: "media",
  content: [
    "./components/**/*.{js,vue,ts}",
    "./layouts/**/*.vue",
    "./pages/**/*.vue",
    "./plugins/**/*.{js,ts}",
    "./nuxt.config.{js,ts}",
    "./app.vue",
  ],
  theme: {
    extend: {
      colors: {
        black: "#000000ff",
        xanthous: "#EEB422ff",
        white: "#FEFEFEff",
        "yale-blue": "#093F78ff",
      },
    },
  },
  plugins: [],
};
