using System.Collections.Generic;
using AgarioShared.AgarioShared.Enums;

namespace AgarioShared.AgarioShared.Messages
{
    [System.Serializable]
    public class StartDictionaryMessage
    {
        public int T = 'C';
        public Dictionary<PlayerCounter, StartSetupMessage> StartMessages = new();
    }
}
