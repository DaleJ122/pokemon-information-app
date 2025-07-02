import { defineStore } from 'pinia'
import api from '@/services/api'

export const usePokemonStore = defineStore('pokemon', {
    state: () => ({
        current: null as null | {
            id: number
            name: string
            spriteUrl: string
            height: number
            weight: number
            abilities: string[]
            types: string[]
        },
        loading: false,
        error: ''
    }),
    actions: {
        async fetch(term: string) {
            this.loading = true
            this.error = ''
            try {
                const res = await api.get(`/pokemon?term=${encodeURIComponent(term)}`)
                this.current = res.data
            } catch (e: any) {
                this.error = e.response?.status === 404
                    ? 'Not found'
                    : 'Fetch error'
            } finally {
                this.loading = false
            }
        }
    }
})
