/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#fff3f0',
          100: '#ffe0d9',
          200: '#ffb8a8',
          300: '#ff8f6e',
          400: '#ff6a42',
          500: '#f04e22',
          600: '#d03d14',
          700: '#a82e0e',
          800: '#7a2009',
          900: '#4d1205',
        },
      },
    },
  },
  plugins: [],
};
