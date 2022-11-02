namespace AgarioShared.AgarioShared.Interfaces
{
    public interface IJson
    {
        public string Serialize<T>(T theObject);
        public T Deserialize<T>(string json);
    }
}