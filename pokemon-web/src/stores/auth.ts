import { defineStore } from 'pinia'
import api from '@/services/api'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        token: localStorage.getItem('token') || ''
    }),
    actions: {
        async login(user: string, pass: string) {
            const { data } = await api.post('/auth/login', {
                userName: user,
                password: pass
            })
            this.token = data.token
            localStorage.setItem('token', data.token)
        },
        logout() {
            this.token = ''
            localStorage.removeItem('token')
        }
    }
})
