using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstWPFApp
{
    public partial class ForthWindow : Window
    {
        public ForthWindow()
        {
            CreateForthWindowUI();
        }

        private void CreateForthWindowUI()
        {
            this.Title = "Інформація про розробника";
            this.Height = 324;
            this.Width = 800;
            this.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            Button GoToMainWin = new Button();
            Grid grid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(67, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(13, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            Label infoLabel = new Label
            {
                Content = "Створив: Сєргєєв Роман Романович\nГрупа: КН-31\nРік: 2025",
                Margin = new Thickness(33, 40, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 114,
                Width = 351,
                FontFamily = new FontFamily("Comic Sans MS"),
                FontSize = 20,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            grid.Children.Add(infoLabel);

            GoToMainWin = new Button
            {
                Content = "До головного вікна",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(607, 205, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 141,
                Height = 60,
                Background = new SolidColorBrush(Color.FromRgb(58, 236, 123))
            };
            GoToMainWin.Click += GoToMainWin_Click;
            Grid.SetColumnSpan(GoToMainWin, 2);
            grid.Children.Add(GoToMainWin);

            this.Content = grid;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void GoToMainWin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }
    }
}