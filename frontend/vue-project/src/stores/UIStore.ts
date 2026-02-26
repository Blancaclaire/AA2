import { defineStore } from 'pinia'

export const useUIStore = defineStore('UIStore', {
  state: () => ({
    theme: localStorage.getItem('theme') || 'light',
    locale: localStorage.getItem('locale') || 'es',
  }),
  actions: {
    toggleTheme() {
      this.theme = this.theme === 'light' ? 'dark' : 'light'
      localStorage.setItem('theme', this.theme)
    },
    setLocale(lang: string) {
      this.locale = lang
      localStorage.setItem('locale', lang)
    },
  },
})