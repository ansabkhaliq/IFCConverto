﻿using IFCConverto.MVVM;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFCConverto.ViewModels
{
    /// <summary>
    /// View Model where we make the list of menu items that we want to display in the Hamburger Menu. If we want to add more items, add to existing list in the same pattern
    /// </summary>
    public class MenuViewModel : MenuItemViewModelBase
    {
        public MenuViewModel()
        {
            // Build the menus
            Menu.Add(new MenuItemViewModel()
            {
                Icon = new PackIconMaterial { Kind = PackIconMaterialKind.CogClockwise },
                Text = "IFC Convert",
                NavigationDestination = new Uri("Views/IFCConvert.xaml", UriKind.RelativeOrAbsolute),
                Command = new DelegateCommand(() => Services.NavigationService.Navigate(new Uri("Views/IFCConvert.xaml", UriKind.RelativeOrAbsolute)))
            });

            Menu.Add(new MenuItemViewModel()
            {
                Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FileCog },
                Text = "Textfile Processing",
                NavigationDestination = new Uri("Views/TextfileProcessing.xaml", UriKind.RelativeOrAbsolute),
                Command = new DelegateCommand(() => Services.NavigationService.Navigate(new Uri("Views/TextfileProcessing.xaml", UriKind.RelativeOrAbsolute)))
            });           
        }
    }
}
