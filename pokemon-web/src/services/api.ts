import axios from 'axios'
import { useAuthStore } from '@/stores/auth'

const api = axios.create({
    baseURL: 'http://localhost:5100/api'
})

api.interceptors.request.use(config => {
    const token = useAuthStore().token
    if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`
    }
    return config
})

export default api
