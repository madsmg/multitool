using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiTool.ViewModel.SystemInfo
{
    public class SystemInfoViewModel : ViewModelBase

    {
        public SystemInfoViewModel()

        {
            ButtonClicked = new RelayCommand<EventArgs>(ExecuteButtonClicked, CanExecuteButtonClicked);

        }

        private bool CanExecuteButtonClicked(EventArgs arg)
        {
         return false;
            //return _info != "ExecuteButtonClicked";
        }

        private void ExecuteButtonClicked(EventArgs obj)
        {
            //Info = "ExecuteButtonClicked";
        }
        

        public ICommand ButtonClicked { get; private set; }

    }
}
