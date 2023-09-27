using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(ObservableObject sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        //public static event PropertyChangedEventHandler StaticPropertyChanged;
        //public static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
