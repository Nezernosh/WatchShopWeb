using WatchShop.BLL.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using Telegram.Bot.Polling;

namespace WatchShop.TelegramBot
{
    public class TGBot
    {
        private static string token = "6081339274:AAEZY_GB89wKZWolUrTYFSxbaJAU3Ha5M3U";
        private readonly ITelegramBotClient bot = new TelegramBotClient(token);
        private readonly IUsersBLL _usersBLL;

        public TGBot(IUsersBLL usersBLL)
        {
            _usersBLL = usersBLL;

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                if (message.Text.ToLower() == "/getpass")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Введите логин для которого вы хотите получить новый пароль:");
                    var login = update.Message.Text;
                    var pass = "test23";
                    if (await _usersBLL.ChangePass(login, pass))
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $"Новый пароль для входа в {login}: {pass}");
                        return;
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Непонятная команда");
                    return;
                }
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}