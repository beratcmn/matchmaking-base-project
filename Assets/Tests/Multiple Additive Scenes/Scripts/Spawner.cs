using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace Test.MAS {

    internal class Spawner {
        internal static void InitialSpawn(Scene scene) {
            if (!NetworkServer.active) return;

            /* for (int i = 0; i < 10; i++)
                SpawnReward(scene); */
        }
    }
}