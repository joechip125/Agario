using AgarioShared.AgarioShared.Enums;

namespace AgarioShared.AgarioShared.Messages
{
    [System.Serializable]
    public class StartMessage
    {
        public int T = 'W';
        public string PlayerName;
        public PlayerCounter PlayerCount;
    }
}
