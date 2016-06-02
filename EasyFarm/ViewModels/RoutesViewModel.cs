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

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using Prism.Commands;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.ViewModels
{
    public class RoutesViewModel : ViewModelBase
    {
        public string ViewName => "Routes";

        private PathRecorder _recorder;

        private readonly SettingsManager _settings;

        private string _recordHeader;

        public RoutesViewModel()
        {
            _settings = new SettingsManager("ewl", "EasyFarm Waypoint List");
                        
            ClearCommand = new DelegateCommand(ClearRoute);
            RecordCommand = new DelegateCommand(Record);
            SaveCommand = new DelegateCommand(Save);
            LoadCommand = new DelegateCommand(Load);
            ResetNavigatorCommand = new DelegateCommand(ResetNavigator);
            RecordHeader = "Record";

            // Create recorder on loaded fface session. 
            OnSessionSet += ViewModelBase_OnSessionSet;
        }

        private void ViewModelBase_OnSessionSet(IMemoryAPI fface)
        {
            _recorder = new PathRecorder(fface);
            _recorder.OnPositionAdded += _recorder_OnPositionAdded;
        }

        private void _recorder_OnPositionAdded(Position position)
        {
            Application.Current.Dispatcher.Invoke(() => Route.Add(position));
        }

        public string RecordHeader
        {
            get { return _recordHeader; }
            set { SetProperty(ref _recordHeader, value); }
        }

        /// <summary>
        ///     Exposes the list of waypoints to the user.
        /// </summary>
        public ObservableCollection<Position> Route
        {
            get { return Config.Instance.Waypoints; }
            set { SetProperty(ref Config.Instance.Waypoints, value); }
        }

        /// <summary>
        /// Makes the bot run a straight path. 
        /// </summary>
        public bool Straight
        {
            get { return Config.Instance.StraightRoute; }
            set { SetProperty(ref Config.Instance.StraightRoute, value); }
        }

        /// <summary>
        ///     Binding for the record command for the GUI.
        /// </summary>
        public ICommand RecordCommand { get; set; }

        /// <summary>
        ///     Binding for the clear command for the GUI.
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        ///     Binding for the save command for the GUI.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        ///     Binding for the load command for the GUI.
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// A command to stop the player from running navigator 
        /// throwing an error. 
        /// </summary>
        public ICommand ResetNavigatorCommand { get; set; }

        /// <summary>
        ///     Clears the waypoint list.
        /// </summary>
        private void ClearRoute()
        {
            Route.Clear();
        }

        /// <summary>
        ///     Pauses and resumes the path recorder based on
        ///     its current state.
        /// </summary>
        private void Record()
        {
            // Return when the user has not selected a process. 
            if (FFACE == null)
            {
                AppServices.InformUser("No process has been selected.");
                return;
            }

            if (!_recorder.IsRecording)
            {
                _recorder.Start();
                RecordHeader = "Recording!";
            }
            else
            {
                _recorder.Stop();
                RecordHeader = "Record";
            }
        }

        /// <summary>
        ///     Saves the route data.
        /// </summary>
        private void Save()
        {
            AppServices.InformUser(_settings.TrySave(Route) ? "Path has been saved." : "Failed to save path.");
        }

        /// <summary>
        ///     Loads the route data.
        /// </summary>
        private void Load()
        {
            Route = _settings.TryLoad<ObservableCollection<Position>>();
            AppServices.InformUser(Route != null ? "Path has been loaded." : "Failed to load the path.");
        }

        /// <summary>
        /// Stops the player from running continously. 
        /// </summary>
        private void ResetNavigator()
        {
            // Return when the user has not selected a process. 
            if (FFACE == null)
            {
                AppServices.InformUser("No process has been selected.");
                return;
            }

            FFACE.Navigator.Reset();
        }
    }
}