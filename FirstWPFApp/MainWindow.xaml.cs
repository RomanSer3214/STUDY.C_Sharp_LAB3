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

namespace FirstWPFApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            //this.Close();
        }

        private void ToWin1_Click(object sender, RoutedEventArgs e)
        {
            FirstWindow mw;
            mw = new FirstWindow();
            mw.Show();
            Close();
        }

        private void ToWin2_Click(object sender, RoutedEventArgs e)
        {
            SecondWindow sw = new SecondWindow();
            sw.Show();
            Close();
        }

        private void ToWin3_Click(object sender, RoutedEventArgs e)
        {
            ThirdWindow tw = new ThirdWindow();
            tw.Show();
            Close();
        }

        private void ToWin4_Click(object sender, RoutedEventArgs e)
        {
            ForthWindow tr = new ForthWindow();
            tr.Show();
            Close();
        }
    }
}
