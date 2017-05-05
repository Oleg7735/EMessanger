using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient
{
    
    delegate void DeleteProgressDelegate(FileSendProgress progress);
    class FileSendProgress: INotifyPropertyChanged, IEquatable<FileSendProgress>
    {
        private string _name;
        private int _progress;
        private DeleteProgressDelegate _delete;

        public int Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
            }
        }

        public string Name
        {
            get
            {
                return "Отправка файла " + _name;
            }

            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Progress"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //private List<FileSendProgress> _progressList;
        public FileSendProgress(string name, DeleteProgressDelegate delete)
        {
            _name = name;
            _progress = 0;
            _delete = delete;
        }
        public void Delete()
        {
            _delete?.Invoke(this);
        }
        public void SetProgress(int percent)
        {
            Progress = percent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Progress"));
            if (percent >= 100)
            {
                _delete?.Invoke(this);
            }
        }

        public bool Equals(FileSendProgress other)
        {
            return base.Equals(other) && this.Name == other.Name;
        }
    }
}
