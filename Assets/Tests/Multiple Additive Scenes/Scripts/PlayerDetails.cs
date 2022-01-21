using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Test.MAS {
    public class PlayerDetails : NetworkBehaviour {
        [SyncVar]
        public int playerNumber;
        [SyncVar]
        public int matchIndex;
        public int clientMatchIndex = -1;

    }
}