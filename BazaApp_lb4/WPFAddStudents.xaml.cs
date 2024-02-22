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
using System.Windows.Shapes;

namespace BazaApp_lb4
{
    /// <summary>
    /// Логика взаимодействия для WPFAddStudents.xaml
    /// </summary>
    public partial class WPFAddStudents : Window
    {
        public WPFAddStudents()
        {
            InitializeComponent();
        }

        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = false;
        }
    }
}
