using System.Collections.Generic;
using AgarioShared.AgarioShared.Enums;

namespace AgarioShared.AgarioShared.Messages
{
    public class PositionDictionaryMessage
    {
        public int T = 'M';
        public Dictionary<PlayerCounter, PositionMessage> PositionMessages = new();
    }
}
