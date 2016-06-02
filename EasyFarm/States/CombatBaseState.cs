/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI>
Copyright (C) Mykezero

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
///////////////////////////////////////////////////////////////////*/

using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     A class for defeating monsters.
    /// </summary>
    public class CombatBaseState : BaseState
    {
        static CombatBaseState()
        {
           IsFighting = false;
        }

        protected CombatBaseState(IMemoryAPI fface) : base(fface)
        {
        }

        /// <summary>
        ///     Whether the fight has started or not.
        /// </summary>
        protected static bool IsFighting { get; set; }

        /// <summary>
        ///     Who we are trying to kill currently
        /// </summary>
        protected static Unit Target { get; set; }        
    }
}