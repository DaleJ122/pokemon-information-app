<template>
  <div class="max-w-lg mx-auto p-4">
    <h1 class="text-4xl mb-4 text-center">
      <span class="font-pokemon text-yellow-400 [-webkit-text-stroke:1px_blue] tracking-wider">Pokémon</span> Search
    </h1>
    <form @submit.prevent="onSearch" class="flex gap-2 mb-4">
      <input
          v-model="term"
          placeholder="Name or ID"
          class="flex-1 p-2 border rounded bg-white text-black placeholder-gray-500"
      />
      <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded">
        Search
      </button>
    </form>

    <div v-if="loading" class="text-gray-500">Loading…</div>
    <div v-if="error" class="text-red-500">{{ error }}</div>

    <PokemonCard v-if="pokemon" :data="pokemon" class="center" />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { usePokemonStore } from '@/stores/pokemon'
import PokemonCard from '@/components/PokemonCard.vue'

const term = ref('')
const store = usePokemonStore()

const pokemon = computed(() => store.current)
const loading = computed(() => store.loading)
const error   = computed(() => store.error)

function onSearch() {
  if (term.value.trim()) {
    store.fetch(term.value.trim())
  }
}
</script>
