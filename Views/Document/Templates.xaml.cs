﻿using Documentor.ViewModels.Document;
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

namespace Documentor.Views
{
    /// <summary>
    /// Logique d'interaction pour Templates.xaml
    /// </summary>
    public partial class Templates : Page
    {
        private TemplatesViewModel viewModel;

        public Templates()
        {
            InitializeComponent();
            this.viewModel = new TemplatesViewModel();
            this.DataContext = viewModel;
        }
    }
}