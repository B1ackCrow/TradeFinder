import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        isAuthenticated: false,
        role: null,
        email: null,
    })
})