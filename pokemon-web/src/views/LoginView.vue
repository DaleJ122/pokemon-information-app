<template>
  <div class="max-w-sm mx-auto p-4">
    <h1 class="text-2xl mb-4 font-bold">Sign In</h1>
    <form @submit.prevent="doLogin" class="space-y-4">
      <div>
        <label class="block mb-1">Username</label>
        <input v-model="user" class="w-full p-2 border rounded" />
      </div>
      <div>
        <label class="block mb-1">Password</label>
        <input v-model="pass" type="password" class="w-full p-2 border rounded" />
      </div>
      <button
          type="submit"
          class="w-full py-2 bg-green-600 text-white rounded"
          :disabled="loading"
      >
        {{ loading ? "Signing in…" : "Sign In" }}
      </button>
      <p v-if="error" class="text-red-500 text-sm">{{ error }}</p>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const user = ref('')
const pass = ref('')
const loading = ref(false)
const error   = ref('')
const auth    = useAuthStore()
const router  = useRouter()

async function doLogin() {
  loading.value = true
  error.value   = ''
  try {
    await auth.login(user.value, pass.value)
    return router.push('/')    // send them to the search page
  } catch (e: any) {
    error.value = 'Invalid credentials'
  } finally {
    loading.value = false
  }
}
</script>
