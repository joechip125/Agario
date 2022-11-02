using System.Collections.Generic;
using AgarioShared.AgarioShared.Messages;

namespace AgarioShared.Assets.Scripts.AgarioShared.Messages
{
    public class SpawnPickups
    {
        public int T = 'b';
        public List<PositionMessage> positions = new();
    }
}