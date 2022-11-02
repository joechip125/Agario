using System;
using System.IO;
using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using AgarioShared.AgarioShared.Enums;
using AgarioShared.AgarioShared.Messages;
using AgarioShared.Assets.Scripts.AgarioShared.Messages;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AgarioServer
{
    public class PlayerLink
    {
        private TcpClient PlayerClient { get; }
        public PlayerCounter PlayerNumber;
        public string PlayerName;
        public Vector3 Position;
        public int Score;
        public int Rank;
        public float Size;
        public float size = 1f;
        
        private  StreamWriter _streamWriter;
        private Game theParent;
        public event Action<float, float, float, PlayerCounter> UpdatePosition;
        
        private readonly JsonSerializerOptions _options = new()
        {
            IncludeFields = true,

        };
        
        public PlayerLink(TcpClient client, Game parentRef, PlayerCounter counter)
        {
            PlayerClient = client;
            theParent = parentRef;
            PlayerNumber = counter;
            
            new Thread(Begin).Start();
        }

        public void SendMessage<T>(T message)
        {
            if (_streamWriter == null)
            {
                _streamWriter = new StreamWriter(PlayerClient.GetStream());
            }
            _streamWriter.WriteLine(JsonSerializer.Serialize(message, _options));
            Console.WriteLine(JsonSerializer.Serialize(message, _options));
            _streamWriter.Flush();
        }

        public void SendMessageJsonConvert<T>(T message)
        {
            if (_streamWriter == null)
            {
                _streamWriter = new StreamWriter(PlayerClient.GetStream());
            }
            var obj =JsonConvert.SerializeObject(message, Formatting.None);

            _streamWriter.WriteLine(obj);
            _streamWriter.Flush();
        }
        
        private MessageTypes GetMessageType(string json)
        {
            var t2 = json.Substring(
                json.IndexOf('T', 0) + 3, 2);

            switch (t2)
            {
                case "80":
                    return MessageTypes.Position;
                case "87":
                    return MessageTypes.Start;
                case "83":
                    return MessageTypes.Score;
                case "75":
                    return MessageTypes.Size;
                case "79":
                    return MessageTypes.Battle;
            }
       
            return MessageTypes.Error;
        }

        public void SetPositionInfo(PositionMessage newPosition)
        {
            Position = new Vector3(newPosition.X, newPosition.Y, newPosition.Z);
        }
        
        private void ProcessStartMessage(string json)
        {
            var loginMessage = JsonSerializer.Deserialize<StartMessage>(json, _options);
            if (loginMessage == null) return;

            Console.WriteLine($"[#{PlayerNumber}] Player '{loginMessage.PlayerName}' logged in.");
            PlayerName = loginMessage.PlayerName;
            loginMessage.PlayerCount = PlayerNumber;
            SendMessage(loginMessage);
            theParent.SendStartMessages();
        }
        
        private void ReadMessage(string json)
        {
            Console.WriteLine(json);
            switch (GetMessageType(json))
            {
                case MessageTypes.Start:
                    ProcessStartMessage(json);
                    break;
                
                case MessageTypes.Position:
                    var positionInfo =  JsonSerializer.Deserialize<PositionMessage>(json, _options);
                    SetPositionInfo(positionInfo);
                    theParent.SendPositionInfo();
                    break;
                
                case MessageTypes.Score:
                    var scoreMessage = JsonSerializer.Deserialize<ScoreMessage>(json, _options);
                    if (scoreMessage == null) return;
                    Score = scoreMessage.Score;
                    theParent.SendScoreInfo();
                    break;
                case MessageTypes.Battle:
                    DoBattle(json);
                    break;
                    
            }
        }


        private void DoBattle(string json)
        {
            var battleMessage = JsonSerializer.Deserialize<BattleMessage>(json, _options);

            if (!theParent.currentFights.Contains(battleMessage))
            {
                theParent.ProcessBattle(battleMessage);
                theParent.currentFights.Add(battleMessage);
            }
        }
        
        private void Begin()
        {
            var streamReader = new StreamReader(PlayerClient.GetStream());
          
            while (true)
            {
                var json = streamReader.ReadLine();
                
                if (json == null) return;

                ReadMessage(json);
            }
        }
    }
}