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
        private int nextId = 0;
        private MediaElement _mediaElementObject;
        private ICommand _selectFile;
        private ICommand _playVideo;
        private string[] _selectedFile;
        private ICommand _stopVideo;
        private ObservableCollection<Song> _songs;
        public Song SelectedSong { get; set; }
        public Song PlayingSong { get; set; }
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

        public ICommand CmdSkipSong { get; set; }
        public VideoViewModel()
        {
            CmdSelectFile = new ActionCommand(OpenFileAction);
            CmdPlayVideo = new ActionCommand(PlayVideoAction);
            CmdStopVideo = new ActionCommand(StopVideoAction);
            CmdSkipSong = new ActionCommand(SkipVideoAction);
        }

        public void StopVideoAction()
        {
            MediaElementObject?.Pause();
        }

        public void PlayVideoAction()
        {
            MediaElementObject.Source = new Uri(SelectedSong.Path);
            PlayingSong = SelectedSong;
            MediaElementObject?.Play();
        }
        public void SkipVideoAction()
        {

            var songId= Songs.FirstOrDefault(song => song.Path == PlayingSong.Path).Id;
            songId++;
            var nextSongToplay = Songs.FirstOrDefault(s => s.Id == songId);
            if (nextSongToplay==null)
            {
                songId = 0;
                nextSongToplay = Songs.FirstOrDefault(s => s.Id == songId);
            }
            MediaElementObject.Source=new Uri(nextSongToplay.Path);
            MediaElementObject?.Play();
            PlayingSong = nextSongToplay;
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
                    Song s = new Song() { Path = song,Id = nextId++};
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
