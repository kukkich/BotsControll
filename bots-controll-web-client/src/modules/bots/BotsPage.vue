<template>
  <div style="padding-top: 10px">
    <bot-card v-for="bot in connectedBots" 
        :bot="bot" :key="bot.name">
    </bot-card>
  </div>
</template>

<script>
import {mapActions, mapGetters, mapMutations, mapState} from "vuex";
import BotCard from "@/modules/bots/components/BotCard";

export default {
  name: "BotsPage",
  components: {BotCard},
  computed: {
    ...mapState({
      connectedBots: state => state.bots.connectedBots
    }),
    // ...mapGetters({
    //   fullName: "bots/fullName"
    // })
  },
  methods: {
    ...mapMutations({
      addConnection: 'bots/addConnection',
      removeConnection: 'bots/removeConnection',
    }),
    ...mapActions({
      fetchCurrentConnections: "bots/fetchCurrentConnections",
      startTrackingConnectionsIfDidnt: "bots/startTrackingConnectionsIfDidnt"
    })
  },
  async mounted() {
    await this.fetchCurrentConnections();
    this.startTrackingConnectionsIfDidnt();
  }
}
</script>
