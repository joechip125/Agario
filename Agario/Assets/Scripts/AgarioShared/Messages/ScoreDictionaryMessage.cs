using System.Collections.Generic;
using AgarioShared.AgarioShared.Enums;
using AgarioShared.AgarioShared.Messages;

namespace AgarioShared
{
    public class ScoreDictionaryMessage
    {
        public int T = 'y';
        public Dictionary<PlayerCounter, ScoreMessage> ScoreMessages = new();
    }
}
