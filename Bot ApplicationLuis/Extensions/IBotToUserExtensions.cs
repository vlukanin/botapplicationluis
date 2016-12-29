﻿namespace Bot_ApplicationLuis.Extensions
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using System;
    using System.Threading.Tasks;

    using BingTranslator;

    public static class IBotToUserExtensions
    {
        public static async Task PostWithTranslationAsync(this IBotToUser context, string message, string messageLocale, string userLocale)
        {
            if (messageLocale == null) throw new ArgumentNullException(nameof(messageLocale));
            if (userLocale == null) throw new ArgumentNullException(nameof(userLocale));

            try
            {
                var bingTranslatorClient = new BingTranslatorClient("Test187871", "dAnT3r/eIc8KedBRUgRCV+juxpf4Wl312jn1Bd2SXzk=");
                var translatedMessage = await bingTranslatorClient.Translate(message, messageLocale, userLocale);
                await context.PostAsync(translatedMessage, userLocale);

            }
            catch (Exception e)
            {
                await context.PostAsync("Translator service problems, please try again later", "en-US");
            }
        }
    }
}