﻿using System.Collections.Generic;
using EasyFarm;

namespace MemoryAPI.Memory
{
    public class MemoryWrapper : IMemoryAPI
    {
        public static MemoryWrapper Create(int pid)
        {
            return new EliteMmoWrapper(pid);
        }

        public INavigatorTools Navigator { get; set; }

        public INPCTools NPC { get; set; }

        public Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }

        public IPlayerTools Player { get; set; }

        public ITargetTools Target { get; set; }

        public ITimerTools Timer { get; set; }

        public IWindowerTools Windower { get; set; }
    }
}