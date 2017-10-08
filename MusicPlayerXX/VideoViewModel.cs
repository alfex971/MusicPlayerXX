using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;
using MusicPlayerXX.Annotations;

namespace MusicPlayerXX
{
    public class VideoViewModel:INotifyPropertyChanged
    {
        private MediaElement _mediaElementObject;
        private ICommand _selectFile;
        private ICommand _playVideo;
        private string[] _selectedFile;
        private ICommand _stopVideo;
        private ObservableCollection<Song> _songs;
        public Song SelectedSong { get; set; }

        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value; 
                OnPropertyChanged();
            }
        }

        public MediaElement MediaElementObject
        {
            get { return _mediaElementObject; }
            set { _mediaElementObject = value; OnPropertyChanged(); }
        }

        public ICommand CmdSelectFile
        {
            get { return _selectFile; }
            set { _selectFile = value; }
        }
        public ICommand CmdPlayVideo
        {
            get { return _playVideo; }
            set { _playVideo = value; }
        }

        public ICommand CmdStopVideo
        {
            get { return _stopVideo; }
            set { _stopVideo = value; }
        }

        public VideoViewModel()
        {
            CmdSelectFile = new ActionCommand(OpenFileAction);
            CmdPlayVideo = new ActionCommand(PlayVideoAction);
            CmdStopVideo = new ActionCommand(StopVideoAction);
        }

        public void StopVideoAction()
        {
            MediaElementObject?.Pause();
        }

        public void PlayVideoAction()
        {
            MediaElementObject.Source = new Uri(SelectedSong.Path);
            MediaElementObject?.Play();
        }

        public void OpenFileAction()
        {
            Songs = new ObservableCollection<Song>();
            var fileBrowser = new OpenFileDialog();
            fileBrowser.Multiselect = true;

            if (fileBrowser.ShowDialog() == true)
            {
                _selectedFile = fileBrowser.FileNames;
                foreach (var song in _selectedFile)
                {
                    Song s = new Song() { Path = song };
                    Songs.Add(s);
                }
                MediaElementObject = new MediaElement();
                MediaElementObject.LoadedBehavior = MediaState.Manual;
               // MediaElementObject.Source = new Uri(_selectedFile);
            }
        }

        //public void AddSongsToList(OpenFileDialog openFileDialog)
        //{
        //    Songs = new ObservableCollection<Song>();
        //    _selectedFile = openFileDialog.FileNames;
        //    foreach (var song in _selectedFile)
        //    {
        //        Song s = new Song() { Path = song };
        //        Songs.Add(s);
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
