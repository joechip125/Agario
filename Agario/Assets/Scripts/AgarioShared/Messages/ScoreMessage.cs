namespace AgarioShared.AgarioShared.Messages
{
    [System.Serializable]
    public class ScoreMessage
    {
        public int T = 'S';
        public int Score;
        public int Rank;
        public string Name;
        public bool sizeUp;
    }
}
