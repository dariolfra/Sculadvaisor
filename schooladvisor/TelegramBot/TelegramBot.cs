using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;
using schooladvisor.Models;
using System.ComponentModel;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace schooladvisor.TelegramBot
{
    public class TelegramBot : BackgroundService
    {
        private TelegramBotClient botClient;
        public Dictionary<int, Dictionary<long, int>> messageId;
        public GestioneDati GD;
        public int[] chatIDs = { 521459468 };
        public Dictionary<int, Dictionary<int, int>> chat;
        //public string[] idchat;

        public TelegramBot()
        {
            //
            botClient = new("7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw");// token
            //botClient = new TelegramBotClient(configuration["TelegramBotToken"]);
            chat = new Dictionary<int, Dictionary<int, int>>();

            foreach (int chatID in chatIDs)
            {
                chat.Add(chatID, new Dictionary<int,int>());
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var cts = new CancellationTokenSource();

            var receiverOprtions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOprtions,
                cancellationToken: cts.Token
                );
            return Task.CompletedTask;


        }
        //Error handling
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        public bool AggiornaStatoCommento(int Id, string Stato)
        {
            //string query = @"UPDATE commenti 
            //         SET stato = @Stato
            //         WHERE Id = @Id";
            //var param = new Commenti
            //{
            //    Stato = Stato,
            //    Id = Id
            //};
            //bool esito;
            //try
            //{
            //    con.Execute(query, param);
            //    esito = true;
            //}
            //catch (Exception ex)
            //{
            //    // Gestione dell'eccezione, se necessario
            //    esito = false;
            //}
            return true;
        }
        public Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery != null)
            {
                var id = update.CallbackQuery.Message.Text.Split("\n")[0];
                var Text = update.CallbackQuery.Message.Text.Split("\n")[2];
                var action = update.CallbackQuery.Data;
                Console.WriteLine($"{id}: {Text} {action}");
                if (action == "Accetta")
                {
                    GD.AccettaRecensioni(Convert.ToInt32(id), "approved");
                    EditMessage(Convert.ToInt32(id), "approvato");
                }
                else if (action == "Rifiuta")
                {
                    GD.AccettaRecensioni(Convert.ToInt32(id), "rejected");
                    EditMessage(Convert.ToInt32(id), "rifiutato");
                }
                return Task.CompletedTask;
            }
            if (update.Type != UpdateType.Message)
            {
                return null;
            }
            if (update.Message!.Type != MessageType.Text)
            {
                return null;
            }



            Console.WriteLine(update.Message.Chat.Id);
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: update.Message.Text,
                cancellationToken: cancellationToken
                );
            return Task.CompletedTask;
        }


        public async Task SendMessage(Review commento)
        {
            var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[]
              {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Accetta"),
                    InlineKeyboardButton.WithCallbackData("Rifiuta")
                }
            });

            commento = GD.RegisterComment(commento);
            foreach(int chatID in chatIDs)
            {
                var mess = await botClient.SendTextMessageAsync(chatID, commento.reviewID + "\n" + "-----------------------------------------\n" + commento.reviewComment + '\n' + "-----------------------------------------\n" + "valutazione: " + commento.reviewRating + "/5", replyMarkup: inlineKeyboardMarkup);
                var dic = chat[chatID];
                dic.Add(commento.reviewID, mess.MessageId);
                chat.Remove(chatID);
                chat.Add(chatID, dic);
            }
            //var tmp = await botClient.SendTextMessageAsync(-4108391972, commento.Id + "\n" + commento.Commento, replyMarkup: inlineKeyboardMarkup);
        }
        public async Task EditMessage(int id, string statoCommento)
        {
            foreach (int chatID in chatIDs)
            {
                var dic = chat[chatID];
                int messID = dic[id];
                await botClient.EditMessageTextAsync(chatID, messageId: messID, text: id + "\n-----------------------------------------\n" + " Il commento è stato " + statoCommento);
                dic.Remove(id);
            }
        }
    }
}
