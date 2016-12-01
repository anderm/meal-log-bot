using MealChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MealChatBot.Dialog
{
    [Serializable]
    public class MealLogDialog : IDialog<object>
    {
        private string applicationId = "Add-Your-Key-Here!";
        private string subscriptionKey = "Add-Your-Key-Here!";

        private async Task<LuisResultWithComposite> GetLuisResult(string query)
        {
            var luisApiPath = "https://api.projectoxford.ai/luis/v1/application?id={0}&subscription-key={1}&q={2}";

            LuisResultWithComposite result = null;
            var path = string.Format(luisApiPath, this.applicationId, this.subscriptionKey, query);
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<LuisResultWithComposite>(await response.Content.ReadAsStringAsync());
            }

            return result;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;

            var luisResult = await this.GetLuisResult(message.Text);
            var bestResult = luisResult?.Intents?.First();

            switch (bestResult.Intent)
            {
                case "None":
                    await this.None(context, luisResult);
                    break;

                case "LogMeal":
                    await this.LogMeal(context, luisResult);
                    break;

                default:
                    context.Done(true);
                    break;
            }
        }

        public async Task None(IDialogContext context, LuisResultWithComposite result)
        {
            string message = $"None Intent";
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task LogMeal(IDialogContext context, LuisResultWithComposite result)
        {
            var count = result.CompositeEntities?.Count ?? 0;
            string message = $"I found {count} composite entities.";
            await context.PostAsync(message);

            foreach (var entity in result.CompositeEntities)
            {
                string compositeEntity = "";
                foreach(var child in entity.Children)
                {
                    compositeEntity += $"{child.Type} - {child.Value}";
                }

                await context.PostAsync(compositeEntity);
            }

            context.Wait(this.MessageReceivedAsync);
        }
    }
}