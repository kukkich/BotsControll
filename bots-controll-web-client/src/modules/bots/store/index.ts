import { botService } from './../services/botService';
import { Bot } from '@/Models/Bot';
import {ActionContext, ActionTree, GetterTree, Module, MutationTree} from 'vuex'
import { RootState } from '@/store';
import {signalRActions, signalRConnection} from "@/Api/signalR";

export interface BotsState {
  connectedBots : Array<Bot>,
  isTrackingConnections: boolean,
}

export const state: BotsState = {
  connectedBots : [],
  isTrackingConnections: false,
};

const getters: GetterTree<BotsState, RootState> = {
};

const mutations: MutationTree<BotsState> = {
  setConnection(state, connection: Array<Bot>) {
    state.connectedBots = connection;
  },
  makeConnected(state, bot: Bot) {
    const updated = state.connectedBots.map((x: Bot) => {
      if (x.id !== bot.id)
        return x
      x.isActive = true;
      return x;
    })
    state.connectedBots = updated
  },
  makeDisconnected(state, bot: Bot){
    const updated = state.connectedBots.map((x: Bot) => {
      if (x.id !== bot.id)
        return x
      x.isActive = false;
      return x;
    })
    state.connectedBots = updated
  },
  setIsTrackingConnections(state, newValue: boolean) {
    state.isTrackingConnections = newValue
  }
};

const actions: ActionTree<BotsState, RootState> = {
  async fetchCurrentConnections(context: ActionContext<BotsState, RootState>) {
    const connections = await botService.getOwn();
    context.commit('setConnection', connections);
  },
  startTrackingConnectionsIfDidnt(context: ActionContext<BotsState, RootState>){
    if (context.state.isTrackingConnections) return;

    console.log("Отслеживание включено")
    signalRConnection.on(signalRActions.OnBotConnection, (x: Bot) => {
      console.log("Подключился")
      context.commit("makeConnected", x)
    });

    signalRConnection.on(signalRActions.OnBotDisconnection, (x: Bot) => {
      console.log("Отключился")
      context.commit("makeDisconnected", x)
    })

    context.commit("setIsTrackingConnections", true)
  }
};

const namespaced: boolean = true;

export const botsConnectionsModule: Module<BotsState, RootState> = {
  namespaced,
  state,
  getters,
  actions,
  mutations,
};
