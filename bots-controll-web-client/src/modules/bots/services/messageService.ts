import $api from "@/Api";
import { Bot } from "@/Models/Bot";
import FormData from 'form-data';

export class messageService {
    static async send(bot: Bot, message: string) : Promise<void> {
        try {
            const data = new FormData();
            data.append('connectionId', bot.id);
            data.append('message', message);

            let config = {
                method: 'post',
                maxBodyLength: Infinity,
                url: '/message/bot',
                data : data
            };

            await $api.request(config)
        }
        catch (e) {
            console.log(e)
            throw e;
        }
    }
}