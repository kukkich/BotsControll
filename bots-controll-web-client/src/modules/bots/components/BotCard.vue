<template>
    <v-card
      class="mx-auto"
      max-width="400"
      :elevation="showMessageButton ? 6 : 0"
      variant="outlined"
    >
      <template v-slot:loader>
        <v-progress-linear :active="isLoading" color="info" indeterminate></v-progress-linear>
      </template>
        <v-card-title>
          {{ bot.name }}
        </v-card-title>
        <v-card-text>
          <ConnectionChip :isConnected="bot.isActive"/>
        </v-card-text>
        <v-card-actions>
          <v-fade-transition>
            <v-btn
                v-show="showMessageButton"
                color="secondary"
                variant="elevated"
                @click="send"
            >
              Отправить
            </v-btn>
          </v-fade-transition>
          <v-spacer/>
          <v-btn :disabled="!bot.isActive" @click="isMessageButtonClicked=!isMessageButtonClicked">Сообщение</v-btn>
        </v-card-actions>
        <v-expand-transition>
          <div v-show="showMessageButton">
            <v-divider></v-divider>
            <v-card-text>
              <v-text-field v-model="message" label="Сообщение" variant="outlined"/>
            </v-card-text>
        </div>
        </v-expand-transition>
    </v-card>
</template>

<script lang="ts">
import { Bot } from '@/Models/Bot';
import type { PropType } from 'vue'
import { defineComponent } from 'vue'
import {messageService} from "@/modules/bots/services/messageService";
import ConnectionChip from "@/modules/bots/components/ConnectionChip.vue";

export default defineComponent({
    name: "BotCard",
    components: {ConnectionChip},
    props: {
      bot: {
        type: Object as PropType<Bot>,
        required: true
      },
      selected: {
        type: Boolean,
        required: false
      }
    },
    data: () => ({
      isMessageButtonClicked: false,
      message: '',
      isLoading: false
    }),
    methods: {
      async send() {
        this.isLoading = true;
        await messageService.send(this.bot, this.message)
        this.message = '';
        this.isLoading = false;
      }
    },
  computed: {
    showMessageButton() {
      return this.isMessageButtonClicked && this.bot.isActive
    }
  },
  watch: {
    showMessageButton: function(newValue) {
      if (newValue === false){
        this.isMessageButtonClicked = false;
      }
    }
  }
})

</script>
<style lang="">
    
</style>