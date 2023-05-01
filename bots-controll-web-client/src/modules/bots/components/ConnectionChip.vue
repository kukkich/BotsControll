<template>
  <div>
    <v-chip
        class="ma-2"
        :color="color"
        text-color="white"
    >
      Green Chip
    </v-chip>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: "ConnectionChip",
    props: {
        isConnected: {
            type: Boolean,
            required: true
        },
    },
  computed: {
    color() {
      return this.isConnected ? 'green' : 'error';
    }
  },
})
</script>
<style>
    
</style>