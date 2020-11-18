// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with CoreBot .NET Template version v4.10.3

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot.Dialogs
{
    public class RemindMeDialog : CancelAndHelpDialog
    {
        private const string TimeStepMsgText = "What day/time would you like to be reminded?";
        private const string ReminderStepMsgText = "What would you like me to remind you about?";

        public RemindMeDialog()
            : base(nameof(RemindMeDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ReminderStepAsync,
                TimeStepAsync,
                // TravelDateStepAsync,
                // ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ReminderStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reminderDetails = (ReminderDetails)stepContext.Options;

            if (reminderDetails.ReminderTimes == null)
            {
                var promptMessage = MessageFactory.Text(TimeStepMsgText, TimeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(reminderDetails.ReminderItems, cancellationToken);
        }

        private async Task<DialogTurnResult> TimeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reminderDetails = (ReminderDetails)stepContext.Options;

            // reminderDetails.Destination = (string)stepContext.Result;

            if (reminderDetails.ReminderItems == null)
            {
                var promptMessage = MessageFactory.Text(ReminderStepMsgText, ReminderStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(reminderDetails.ReminderItems, cancellationToken);
        }

        // private async Task<DialogTurnResult> OriginStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     var reminderDetails = (ReminderDetails)stepContext.Options;

        //     reminderDetails.Destination = (string)stepContext.Result;

        //     if (reminderDetails.Origin == null)
        //     {
        //         var promptMessage = MessageFactory.Text(OriginStepMsgText, OriginStepMsgText, InputHints.ExpectingInput);
        //         return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        //     }

        //     return await stepContext.NextAsync(reminderDetails.Origin, cancellationToken);
        // }

        // private async Task<DialogTurnResult> TravelDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     var reminderDetails = (ReminderDetails)stepContext.Options;

        //     reminderDetails.Origin = (string)stepContext.Result;

        //     if (reminderDetails.TravelDate == null || IsAmbiguous(reminderDetails.TravelDate))
        //     {
        //         return await stepContext.BeginDialogAsync(nameof(DateResolverDialog), reminderDetails.TravelDate, cancellationToken);
        //     }

        //     return await stepContext.NextAsync(reminderDetails.TravelDate, cancellationToken);
        // }

        // private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     var reminderDetails = (ReminderDetails)stepContext.Options;

        //     reminderDetails.TravelDate = (string)stepContext.Result;

        //     var messageText = $"Please confirm, I have you traveling to: {reminderDetails.Destination} from: {reminderDetails.Origin} on: {reminderDetails.TravelDate}. Is this correct?";
        //     var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

        //     return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        // }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var reminderDetails = (ReminderDetails)stepContext.Options;

                return await stepContext.EndDialogAsync(reminderDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
