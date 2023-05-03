using WatchShop.BLL.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WatchShop.TelegramBot
{
    public class TGBot
    {
        private static string token = "6081339274:AAEZY_GB89wKZWolUrTYFSxbaJAU3Ha5M3U";
        //private static string token = Environment.GetEnvironmentVariable("TOKEN");
        private static TelegramBotClient bot;
        private static IUsersBLL _usersBLL;

        public TGBot(IUsersBLL usersBLL)
        {
            bot = new TelegramBotClient(token);
            _usersBLL = usersBLL;

            bot.StartReceiving(Update, Error);
        }

        async static Task Update(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await bot.SendTextMessageAsync(message.Chat, $"Приветствую, {message.From.FirstName} {message.From.LastName}!");
                    return;
                }
                if (message.Text.ToLower() == "/getpass")
                {
                    await bot.SendTextMessageAsync(message.Chat, "Введите свой логин для получения нового пароля:");
                    return;
                    
                }
                if (message.Text.ToLower() == "/help")
                {
                    await bot.SendTextMessageAsync(message.Chat, "Простой чат-бот для генерации одноразовых паролей. Мои доступные команды: /start, /getpass, /help");
                    return;
                }
                var login = message.Text;
                var pass = GeneratePass(16);
                if (await _usersBLL.ChangePass(login, pass))
                {
                    await bot.SendTextMessageAsync(message.Chat, $"Новый пароль для входа в {login}: {pass}");
                }
                else
                {
                    await bot.SendTextMessageAsync(message.Chat, $"Пользователь {login} не найден");
                }
                return;
            }
        }
        private static string GeneratePass(int length)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random rand = new Random();
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += validChars[rand.Next(validChars.Length)];  
            }

            return result;
        }
        private static Task Error(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
}
