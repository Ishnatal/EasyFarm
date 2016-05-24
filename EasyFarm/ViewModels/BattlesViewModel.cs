/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using Prism.Commands;

namespace EasyFarm.ViewModels
{
    [ViewModel("Battles")]
    public class BattlesViewModel : ViewModelBase
    {
        public BattlesViewModel()
        {
            AddActionCommand = new DelegateCommand(AddAction);
            DeleteActionCommand = new DelegateCommand(DeleteAction);
            ClearActionsCommand = new DelegateCommand(ClearActions);
        }

        public ObservableCollection<BattleList> BattleLists
        {
            get { return Config.Instance.BattleLists; }
            set
            {
                var lists = (ObservableCollection<BattleList>) Config.Instance.BattleLists;
                SetProperty(ref lists, value);
            }
        }

        /// <summary>
        ///     The currently selected ability.
        /// </summary>
        public AbilityViewModel SelectedAbilityViewModel { get; set; }

        /// <summary>
        ///     The currently selected list.
        /// </summary>
        public BattleList SelectedList { get; set; }

        /// <summary>
        ///     Action to add an new move to the currently selected list.
        /// </summary>
        public ICommand AddActionCommand { get; set; }

        /// <summary>
        ///     Action to delete an existing move from the currently selected list.
        /// </summary>
        public ICommand DeleteActionCommand { get; set; }

        /// <summary>
        ///     Action to clear all moves from the currently selected list.
        /// </summary>
        public ICommand ClearActionsCommand { get; set; }

        /// <summary>
        ///     Finds the list containing the given battle ability.
        /// </summary>
        /// <param name="abilityViewModel"></param>
        /// <returns></returns>
        private BattleList FindListContainingAbility(AbilityViewModel abilityViewModel)
        {
            return Config.Instance.BattleLists
                .FirstOrDefault(x => x.Actions.Contains(abilityViewModel));
        }

        /// <summary>
        ///     Add an move to the currently selected list.
        /// </summary>
        private void AddAction()
        {
            // Check to see if an ability is selected. If so, find the list with which to add the 
            // new ability under. 
            if (SelectedAbilityViewModel != null)
            {
                SelectedList = FindListContainingAbility(SelectedAbilityViewModel);
            }

            // Now, if the user has either selected an ability or a list, 
            // add the new ability. 
            if (SelectedList != null)
            {
                var action = new AbilityViewModel();
                SelectedList.Actions.Add(action);
            }
        }

        /// <summary>
        ///     Remove an move from the currently selected list.
        /// </summary>
        private void DeleteAction()
        {
            // Check if the user has selected an ability, do nothing if not. 
            if (SelectedAbilityViewModel != null)
            {
                // Get the list with the selected ability in it. 
                if (SelectedList == null)
                {
                    SelectedList = FindListContainingAbility(SelectedAbilityViewModel);
                }

                // Remove the selected item from the list. 
                if (SelectedList != null)
                {
                    // Removed the selected ability. 
                    SelectedList.Actions.Remove(SelectedAbilityViewModel);

                    // Ensure there is atleast one ability. 
                    KeepOne();
                }
            }
        }

        /// <summary>
        ///     Clear the currently selected list.
        /// </summary>
        private void ClearActions()
        {
            // If the user has not selected a list...
            if (SelectedList == null)
            {
                // If they've selected an ability, use the list containing that ability. 
                if (SelectedAbilityViewModel != null)
                {
                    SelectedList = FindListContainingAbility(SelectedAbilityViewModel);
                }
            }

            // If they've selected a list or we've found a list containing an ability... 
            if (SelectedList != null)
            {
                // Remove all list items. 
                SelectedList.Actions.Clear();

                // Ensure there is atleast one ability. 
                KeepOne();
            }
        }

        /// <summary>
        ///     Ensures a list has at least one ability item in it.
        /// </summary>
        public void KeepOne()
        {
            if (SelectedList == null) return;

            // Ensure there is atleast one ability. 
            if (!SelectedList.Actions.Any())
            {
                var action = new AbilityViewModel();
                SelectedList.Actions.Add(action);
            }
        }
    }
}