using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace Assets.Core
{
    /// <summary>
    /// A list of players that is synced over the network
    /// </summary>
    class PlayerSyncList : SyncListStruct<Player>
    {
        /// <summary>
        /// Find the position of an player by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The position or -1 if nothing was found</returns>
        public int FindIndexById(string id)
        {
            var found = this
                .Select((p, i) => new {p.Id, i})
                .FirstOrDefault(x => x.Id == id);
            return found == null ? -1 : found.i;
        }
    }
}