﻿using IFCConverto.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IFCConverto.ViewModels
{
    public class IFCConvertViewModel : ViewModelBase
    {
        #region Private Fields

        private string sourcePath;
        private string destinationPath;

        private ICommand sourceLocationAccessCommand;
        private ICommand destinationLocationAccessCommand;
        private ICommand convertCommand;

        #endregion

        #region Properties

        public string SourcePath
        {
            get
            {
                return sourcePath;
            }
            set
            {
                sourcePath = value;
                OnPropertyChanged("SourcePath");
            }
        }

        public string DestinationPath
        {
            get
            {
                return destinationPath;
            }
            set
            {
                destinationPath = value;
                OnPropertyChanged("DestinationPath");
            }
        }

        #endregion

        #region Commands

        public ICommand SourceLocationAccessCommand
        {
            get
            {
                return sourceLocationAccessCommand;
            }

            set
            {
                sourceLocationAccessCommand = (DelegateCommand)value;
                OnPropertyChanged("SourceLocationAccessCommand");
            }
        }

        public ICommand DestinationLocationAccessCommand
        {
            get
            {
                return destinationLocationAccessCommand;
            }

            set
            {
                destinationLocationAccessCommand = (DelegateCommand)value;
                OnPropertyChanged("DestinationLocationAccessCommand");
            }
        }

        public ICommand ConvertCommand
        {
            get
            {
                return convertCommand;
            }

            set
            {
                convertCommand = (DelegateCommand)value;
                OnPropertyChanged("ConvertCommand");
            }
        }

        #endregion      

        public IFCConvertViewModel()
        {
            SourceLocationAccessCommand = new DelegateCommand(AccessSourceLocation, () => true);
            DestinationLocationAccessCommand = new DelegateCommand(AccessDestinationLocation, () => true);
            ConvertCommand = new DelegateCommand(ConvertFiles, () => true);
        }

        private void ConvertFiles()
        {
            throw new NotImplementedException();
        }

        private void AccessDestinationLocation()
        {
            DestinationPath = string.Empty;
        }

        private void AccessSourceLocation()
        {
            SourcePath = string.Empty;
        }
    }
}
