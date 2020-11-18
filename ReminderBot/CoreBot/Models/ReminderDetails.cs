// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with CoreBot .NET Template version v4.10.3

using Microsoft.Bot.Builder.AI.Luis;

namespace CoreBot
{
    public class ReminderDetails
    {
        public DateTimeSpec[] ReminderTimes { get; set; }
        public string[] Emails { get; set; }
        public string[] KeyPhrases { get; set; }
        public OrdinalV2[] Ordinals { get; set; }
        public string[] People { get; set; }
        public string[] ReminderItems { get; set; }
        public string[] Urls { get; set; }
    }
}
