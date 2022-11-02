using AgarioShared.AgarioShared.Enums;

namespace AgarioShared.AgarioShared.Messages
{
    public class BattleMessage
    {
        public int T = 'O';
        public PlayerCounter thisPlayer;
        public PlayerCounter otherPlayer;
    }
}