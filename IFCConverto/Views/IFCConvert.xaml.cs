﻿using IFCConverto.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IFCConverto.Views
{
    /// <summary>
    /// Interaction logic for IFCConvert.xaml
    /// </summary>
    public partial class IFCConvert : Page
    {
        public IFCConvertViewModel ViewModel { get; set; }

        public IFCConvert()
        {
            InitializeComponent();
            ViewModel = new IFCConvertViewModel(DialogCoordinator.Instance);
            DataContext = ViewModel;
        }
    }
}
