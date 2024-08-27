using Bolt;
using UnityEngine;

namespace Core
{
    public class PhotonBoltSharedListener : PhotonBoltBaseListener
    {
        [SerializeField] private PhotonBoltReference photon;

        public override void SceneLoadLocalDone(string map, IProtocolToken token)
        {
            base.SceneLoadLocalDone(map, token);

            if (BoltNetwork.IsConnected && BoltNetwork.IsClient)
            {
                World.MapManager.InitializeLoadedMap(1);
            }
        }
    }
}
