﻿using CocktailBuddy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CocktailBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedView : ContentPage
    {
        public DetailedView(DetailedViewViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}