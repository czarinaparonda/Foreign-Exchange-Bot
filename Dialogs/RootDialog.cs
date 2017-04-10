using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;


namespace Foreign_Exchange_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public double amt = 0, uamt = 0, camt = 0, auamt = 0, eamt = 0;
        public string amount, bcurrency, tcurrency, fcurrency, statement;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(WelcomeAsync);
            return Task.CompletedTask;
        }
        private async Task WelcomeAsync(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                await context.PostAsync("Welcome to Foreign Exchange Bot");
                await context.PostAsync("We are exchanging your current currency to your desired currency. \n Please select on the convertible currency and know your exchange rate!");
                await Task.Run(() => ChooseBase(context, result));
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task ChooseBase(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                await Task.Run(() => GenerateBaseCard(context, result));
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task ChooseBase_1(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                bcurrency = activity.Text;
                if (bcurrency.ToLower() == "us dollars" || bcurrency.ToLower() == "canadian dollars" || bcurrency.ToLower() == "australian dollars" || bcurrency.ToLower() == "euro" || bcurrency.ToLower() == "peso")
                {
                    await Task.Run(() => GenerateRepCard(context, result));
                }
                else { await Task.Run(() => GenerateBaseCard(context, result)); }
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task ChooseBase_2(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                fcurrency = activity.Text;
                if (fcurrency.ToLower() == "us dollars" || fcurrency.ToLower() == "canadian dollars" || fcurrency.ToLower() == "australian dollars" || fcurrency.ToLower() == "euro" || fcurrency.ToLower() == "peso")
                {
                    if (bcurrency == fcurrency)
                    {
                        await context.PostAsync("Sorry. You should enter different currency.");
                        await context.PostAsync("Try Again");
                        await Task.Run(() => GenerateRepCard(context, result));
                    }
                    else
                    {
                        await context.PostAsync("How much do you want to convert?");
                        context.Wait(ChooseBase_3);
                    }
                }
                else
                {
                    await Task.Run(() => GenerateRepCard(context, result));
                }

            }
            catch (Exception e) { await context.PostAsync(e.Message); }

        }
        private async Task ChooseBase_3(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amount = activity.Text;
                await Task.Run(() => BaseComputing(context, result));

            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task BaseComputing(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                if (bcurrency == "us dollars")
                {
                    tcurrency = Convert.ToString(Convert.ToDouble(amount) * 50.1911);
                    await Task.Run(() => Solve(context, result));

                }
                else if (bcurrency == "canadian dollars")
                {
                    tcurrency = Convert.ToString(Convert.ToDouble(amount) * 37.3464);
                    await Task.Run(() => Solve(context, result));
                }
                else if (bcurrency == "australian dollars")
                {
                    tcurrency = Convert.ToString(Convert.ToDouble(amount) * 38.7434);
                    await Task.Run(() => Solve(context, result));
                }
                else if (bcurrency == "euro")
                {
                    tcurrency = Convert.ToString(Convert.ToDouble(amount) * 54.1201);
                    await Task.Run(() => Solve(context, result));
                }
                else if (bcurrency == "peso")
                {
                    tcurrency = Convert.ToString(Convert.ToDouble(amount) * 1);
                    await Task.Run(() => Solve(context, result));
                }
                else
                {
                    await context.PostAsync("I'm sorry you've entered an invalid keyword. Please Enter a valid currency.");
                    context.Wait(GenerateRepCard);
                }


            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task GenerateBaseCard(IDialogContext context, IAwaitable<object> result)
        {
            statement = "What is your current currency?";
            await Task.Run(() => GenerateCardList(context, result));
            context.Wait(ChooseBase_1);
        }
        private async Task GenerateRepCard(IDialogContext context, IAwaitable<object> result)
        {
            statement = "What currency value do you wish to be converted? ";
            await Task.Run(() => GenerateCardList(context, result));
            context.Wait(ChooseBase_2);
        }
        private async Task Solve(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;

                if (fcurrency == "us dollars")
                {
                    await Task.Run(() => USDFunc(context, result));

                }
                else if (fcurrency == "canadian dollars")
                {
                    await Task.Run(() => CADFunc(context, result));
                }
                else if (fcurrency == "australian dollars")
                {
                    await Task.Run(() => AUSFunc(context, result));
                }
                else if (fcurrency == "euro")
                {
                    await Task.Run(() => EUFunc(context, result));
                }
                else if (fcurrency == "peso")
                {
                    await Task.Run(() => PHPFunc(context, result));
                }
                else
                {
                    await context.PostAsync("I'm sorry you've entered an invalid keyword. Please Enter a valid currency.");
                    await Task.Run(() => GenerateRepCard(context, result));
                }
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task USDFunc(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amt = Convert.ToDouble(tcurrency);
                uamt = amt / Convert.ToDouble(50.1122);
                await context.PostAsync("You have " + Convert.ToString(Math.Round(uamt, 2) + " US Dollar/s"));
                await context.PostAsync("Do you wish to have another convertion? ");
                context.Wait(LoopConvertion);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task CADFunc(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amt = Convert.ToDouble(tcurrency);
                uamt = amt / Convert.ToDouble(37.6434);
                await context.PostAsync("You have " + Convert.ToString(Math.Round(uamt, 2) + " Canadian Dollar/s"));
                await context.PostAsync("Do you wish to have another convertion? ");
                context.Wait(LoopConvertion);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task AUSFunc(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amt = Convert.ToDouble(tcurrency);

                uamt = amt / 38.7434;
                await context.PostAsync("You have " + Convert.ToString(Math.Round(uamt, 2) + " Australian Dollar/s"));
                await context.PostAsync("Do you wish to have another convertion? ");
                context.Wait(LoopConvertion);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task EUFunc(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amt = Convert.ToDouble(tcurrency);
                uamt = amt / 54.1201;
                await context.PostAsync("You have " + Convert.ToString(Math.Round(uamt, 2) + " Euro/s."));
                await context.PostAsync("Do you wish to have another convertion? ");
                context.Wait(LoopConvertion);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task PHPFunc(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                amt = Convert.ToDouble(tcurrency);
                uamt = amt / 1;
                await context.PostAsync("You have " + Convert.ToString(Math.Round(uamt, 2) + " Peso/s."));
                await context.PostAsync("Do you wish to have another convertion? ");
                context.Wait(LoopConvertion);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task LoopConvertion(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
                if (activity.Text.ToLower() == "yes")
                {
                    await context.PostAsync("You choose to have another conversation.");
                    await Task.Run(() => GenerateBaseCard(context, result));

                }
                else if (activity.Text.ToLower() == "no")
                {
                    await context.PostAsync("Thank you for using Foreign Exchange Bot!");
                    context.Wait(WelcomeAsync);
                }
                else
                {
                    await context.PostAsync("You've enter an invalid keyword.");
                    context.Wait(LoopConvertion);
                }

            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
        private async Task GenerateCardList(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;
 
                Activity replyToConversation = activity.CreateReply("Foreign Exchange Currency");
                replyToConversation.Recipient = activity.From;
                replyToConversation.Type = "message";
                replyToConversation.Attachments = new List<Attachment>();

                List<CardAction> cardButtons = new List<CardAction>();

                CardAction plButton = new CardAction()
                {
                    Value = "us dollars",
                    Type = "imBack",
                    Title = "US Dollars"
                };
                cardButtons.Add(plButton);
                CardAction plButton2 = new CardAction()
                {
                    Value = "canadian dollars",
                    Type = "imBack",
                    Title = "Canadian dollars",
                };
                cardButtons.Add(plButton2);
                CardAction plButton3 = new CardAction()
                {
                    Value = "australian dollars",
                    Type = "imBack",
                    Title = "Australian Dollars"
                };
                cardButtons.Add(plButton3);
                CardAction plButton4 = new CardAction()
                {
                    Value = "euro",
                    Type = "imBack",
                    Title = "Euro"
                };
                cardButtons.Add(plButton4);
                CardAction plButton5 = new CardAction()
                {
                    Value = "peso",
                    Type = "imBack",
                    Title = "Peso"
                };
                cardButtons.Add(plButton5);
                HeroCard plCard = new HeroCard()
                {
                    Title = statement,
                    Buttons = cardButtons
                };


                Attachment plAttachment = plCard.ToAttachment();
                replyToConversation.Attachments.Add(plAttachment);
                await context.PostAsync(replyToConversation);
            }
            catch (Exception e) { await context.PostAsync(e.Message); }
        }
    }
}

