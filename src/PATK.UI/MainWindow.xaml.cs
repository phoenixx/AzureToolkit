using System.Collections.Generic;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace PATK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public IEnumerable<MenuItem> MenuItems()
        {
            var menu = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Name = "Test"
                }
            };
            return menu;
        } 
    }
}
