using AgarioShared.AgarioShared.Enums;

namespace AgarioShared.AgarioShared.Messages
{
    [System.Serializable]
    public class PositionMessage
    {
        public int T = 'P';
        public float X;
        public float Y;
        public float Z;
        public PlayerCounter playerCounter;
    }
}
