﻿/*///////////////////////////////////////////////////////////////////
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

using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;

namespace EasyFarm.States
{
    public class PullState : CombatBaseState
    {
        public PullState(IMemoryAPI fface) : base(fface)
        {
            Executor = new Executor(fface);
        }

        private Executor Executor { get; }

        /// <summary>
        ///     Allow component to run when moves need to be triggered or
        ///     FightStarted state needs updating.
        /// </summary>
        /// <returns></returns>
        public override bool CheckComponent()
        {
            if (IsFighting) return false;
            if (new RestState(fface).CheckComponent()) return false;
            if (!UnitFilters.MobFilter(fface, Target)) return false;
            return Config.Instance.BattleLists["Pull"].Actions.Any(x => x.IsEnabled);
        }

        public override void EnterComponent()
        {
            fface.Navigator.Reset();
        }

        /// <summary>
        ///     Use pulling moves if applicable to make the target
        ///     mob aggressive to us.
        /// </summary>
        public override void RunComponent()
        {
            var actions = Config.Instance.BattleLists["Pull"].Actions.ToList();
            var usable = actions.Where(x => ActionFilters.TargetedFilter(fface, x, Target)).ToList();
            Executor.UseTargetedActions(usable, Target);
        }

        /// <summary>
        ///     Handle all cases of setting fight started to proper values
        ///     so other components can fire.
        /// </summary>
        public override void ExitComponent()
        {
        }
    }
}