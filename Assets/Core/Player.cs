using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Core
{
    struct Player
    {
        internal readonly string Name;
        internal readonly string Id;
        internal Score Score;

        internal Player(string id, string name)
        {
            Name = name;
            Score = new Score();
            Id = id;
        }

        /// <summary>
        /// Check if the player is initialized
        /// </summary>
        internal bool IsInitialized
        {
            get { return Id != ""; }
        }
    }
}