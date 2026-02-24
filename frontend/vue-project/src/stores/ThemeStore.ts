// stores/ThemeStore.ts
import { defineStore } from 'pinia'
import { watch, ref } from 'vue'

export const useThemeStore = defineStore('theme', () => {
  const theme = ref(localStorage.getItem('theme') || 'light')

  watch(theme, (val) => {
    document.documentElement.setAttribute('data-bs-theme', val)
    localStorage.setItem('theme', val)
  }, { immediate: true })

  const toggleTheme = () => {
    theme.value = theme.value === 'light' ? 'dark' : 'light'
  }

  return { theme, toggleTheme }
})