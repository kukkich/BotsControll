import $api from "@/Api";
import { Bot } from "@/Models/Bot";

export class botConnectionService {
    static async getCurrentConnections() : Promise<Array<Bot>> {
        try {
            const response = await $api.get<Array<Bot>>("/connections/bots");
            return response.data
        }
        catch (e) {
            console.log(e)
            throw e;
        }
    }
}