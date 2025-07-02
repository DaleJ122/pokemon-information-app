<template>
  <div class="max-w-xs mx-auto bg-white border-4 border-red-500 rounded-xl shadow-xl overflow-hidden mt-10">
    <div class="bg-red-500 text-white text-center py-2 font-bold text-lg pokemon-font">
      <span class="capitalize">{{ formatName(data.name) }}</span>
      <span class="text-sm">
        #{{ String(data.id).padStart(3, '0') }}
      </span>    </div>

    <div class="flex justify-center my-4">
      <img :src="data.spriteUrl" 
           alt="{{ formatName(data.name) }} sprite" 
           class="w-36 h-36 hover:scale-125 transition-transform"
      />
    </div>

    <div class="px-4 pb-4">
      <div class="flex flex-wrap justify-center gap-2 mb-2">
        <span
            v-for="type in data.types"
            :key="type"
            class="px-2 py-0.5 rounded-full text-xs font-semibold"
            :class="badgeClasses(type)"
        >
          {{ capitalize(type) }}
        </span>
      </div>

      <div class="flex flex-wrap justify-center gap-2 mb-4">
        <span
            v-for="ab in data.abilities"
            :key="ab"
            class="px-2 py-0.5 rounded-full text-xs font-medium bg-gray-200 text-gray-800"
        >
          {{ capitalize(ab) }}
        </span>
      </div>

      <div class="text-center text-sm text-gray-700">
        <div>Height: {{ data.height }}</div>
        <div>Weight: {{ data.weight }}</div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
export interface PokemonData {
  id: number
  name: string
  spriteUrl: string
  height: number
  weight: number
  abilities: string[]
  types: string[]
}

defineProps<{ data: PokemonData }>()

function badgeClasses(type: string) {
  switch (type.toLowerCase()) {
    case 'fire':     return 'bg-red-200 text-red-800'
    case 'water':    return 'bg-blue-200 text-blue-800'
    case 'grass':    return 'bg-green-200 text-green-800'
    case 'electric': return 'bg-yellow-200 text-yellow-800'
    case 'psychic':  return 'bg-pink-200 text-pink-800'
    case 'ice':      return 'bg-teal-200 text-teal-800'
    case 'dark':     return 'bg-gray-800 text-white'
    case 'fairy':    return 'bg-pink-100 text-pink-700'
    default:         return 'bg-gray-200 text-gray-800'
  }
}

function capitalize(str: string) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

function formatName(raw: string) {
  const parts = raw.split('-');
  if (parts.length > 1 && ['male', 'female'].includes(parts[parts.length - 1].toLowerCase())) {
    const main = parts[0];
    const variant = parts[parts.length - 1];
    return `${capitalize(main)} (${capitalize(variant)})`;
  }
  return parts.map(capitalize).join(' ');
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap');

.pokemon-font {
  font-family: 'Press Start 2P', cursive;
}
</style>
