import $api from "@/Api";
import { Bot } from "@/Models/Bot";

export class botService {
    static async getOwn() : Promise<Array<Bot>> {
        try {
            const response = await $api.get<Array<Bot>>("/bots/own");
            return response.data
        }
        catch (e) {
            console.log(e)
            throw e;
        }
    }
}