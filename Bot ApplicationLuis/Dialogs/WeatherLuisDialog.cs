namespace Bot_ApplicationLuis.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using OpenWeatherMap;

    [Serializable]
    [LuisModel("83439f7d-2571-409a-85f7-6e2567cd68e0", "8356c4b9d8a043488ee5122af937f57c")]
    public class WeatherLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task ProcessNone(IDialogContext context, LuisResult result)
        {
            var message = "Sorry, I couldn't understand you";

            await context.PostAsync(message, "en-US");

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetHelp")]
        public async Task ProcessGetHelp(IDialogContext context, LuisResult result)
        {
            string message;
            //if (Thread.CurrentThread.CurrentCulture.Name == "ru-RU")
            //{
            //    message = "Я - простой погодный бот. Примеры запросов: \n\n" +
            //                  "\"Какая сегодня погода в Москве?\" \n\n" +
            //                  "\"Что с температурой?\" \n\n" +
            //                  "или просто скажите \"Привет\" или \"Спасибо\"";
            //    await context.PostAsync(message, "ru-RU");
            //}
            //else
            //{
            message = "I'm a simple weather bot. Here are some examples of things you can ask me about: \n\n" +
                      "\"What is the weather like in Minsk today?\" \n\n" +
                      "\"Any news about temperature today?\" \n\n" +
                      "or just tell me \"Hello\" or \"Thank you\"";
            await context.PostAsync(message, "en-US");
            //    await context.PostWithTranslationAsync(message, "en-US", Thread.CurrentThread.CurrentCulture.Name);
            //}

            context.Wait(MessageReceived);
        }

        [LuisIntent("Hello")]
        public async Task ProcessHello(IDialogContext context, LuisResult result)
        {
            var c = Thread.CurrentThread.CurrentCulture;

            var messages = new string[]
            {
                "Hello!",
                "Nice to meet you!",
                "Hi! What can I help you with?",
                "I'm here to help you!"
            };

            var message = messages[(new Random()).Next(messages.Count() - 1)];
            await context.PostAsync(message, "en-US");
            //await context.PostWithTranslationAsync(message, "en-US", Thread.CurrentThread.CurrentCulture.Name);

            context.Wait(MessageReceived);
        }

        [LuisIntent("Thanks")]
        public async Task ProcessThanks(IDialogContext context, LuisResult result)
        {
            var messages = new string[]
            {
                "Never mind",
                "You are welcome!",
                "Happy to be useful"
            };

            var message = messages[(new Random()).Next(messages.Count() - 1)];
            await context.PostAsync(message, "en-US");
            //await context.PostWithTranslationAsync(message, "en-US", Thread.CurrentThread.CurrentCulture.Name);

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetWeather")]
        public async Task GetWether(IDialogContext context, LuisResult result)
        {
            var parameter = "temperature";
            var location = "Minsk";
            DateTime date = DateTime.Today.Date;

            EntityRecommendation entityContainer;
            if (result.TryFindEntity("builtin.geography.city", out entityContainer))
            {
                location = entityContainer.Entity;
            }

            if (result.TryFindEntity("builtin.datetime.date", out entityContainer))
            {
                DateTime.TryParse(entityContainer?.Resolution?.SingleOrDefault().Value, out date);
            }

            if (result.TryFindEntity("parameter", out entityContainer))
            {
                parameter = entityContainer.Entity;
            }

            var weatherClient = new WeatherClient("274b7f5f5dacb3d6a982a3af4dec6ea2");
            var forecastArray = await weatherClient.Forecast(location);
            var forecast = forecastArray.SingleOrDefault(f => f.When.Date == date.Date);

            string message;
            if (forecast != null)
            {
                if (parameter.Contains("humid")) { message = $"The humidity on {forecast.Date} in {location} is {forecast.Humidity}%\r\n"; }
                else if (parameter.Contains("pres")) { message = $"The pressure on {forecast.Date} in {location} is {forecast.Pressure}hpa\r\n"; }
                else if (parameter.Contains("temp")) { message = $"The temperature on {forecast.Date} in {location} is {forecast.Temp}°C\r\n"; }
                else { message = "Sorry, unknown parameter \"{parameter}\" requested... Try again"; }
            }
            else { message = "Sorry! I was not able to get the forecast."; }

            await context.PostAsync(message, "en-US");
            //await context.PostWithTranslationAsync(message, "en-US", Thread.CurrentThread.CurrentCulture.Name);

            context.Wait(MessageReceived);
        }

        //    protected override async Task<string> GetLuisQueryTextAsync(IDialogContext context, IMessageActivity message)
        //    {
        //        // return Task.FromResult(message.Text); // in source code

        //        var baseLuisText = await base.GetLuisQueryTextAsync(context, message);

        //        if (message.Locale != null && message.Locale != "en-US")
        //        {
        //            try
        //            {
        //                var bingTranslatorClient = new BingTranslatorClient("Test187871", "dAnT3r/eIc8KedBRUgRCV+juxpf4Wl312jn1Bd2SXzk=");
        //                return await bingTranslatorClient.Translate(baseLuisText, message.Locale, "en-US");

        //            }
        //            catch (Exception)
        //            {
        //                return null;
        //            }
        //        }

        //        return baseLuisText;
        //    }
        //}
    }
}