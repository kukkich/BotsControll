import Vuex from 'vuex';
import { botsConnectionsModule as bots } from '@/modules/bots/store';

export interface RootState {

}

export default new Vuex.Store<RootState>({
  state: {
  },
  modules: {
    bots,
  },
  mutations: {

  },
  actions: {

  },
});