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

namespace FirstWPFApp
{
    public partial class SecondWindow : Window
    {
        private Grid gameGrid;
        private bool _isXTurn = true;
        public SecondWindow()
        {
            CreateSecondWindowUI();
            InitializeComboBoxes();
        }

        private void CreateSecondWindowUI()
        {
            this.Title = "Хрестики-нулики 5х5";
            this.Height = 650;
            this.Width = 520;
            this.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            gameGrid = new Grid();
            gameGrid.Margin = new Thickness(5);
            NameScope.SetNameScope(this, new NameScope());

            for (int i = 0; i < 6; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = i == 5 ? new GridLength(1, GridUnitType.Star) : new GridLength(100)
                });
            }

            for (int i = 0; i < 5; i++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = i == 4 ? new GridLength(1, GridUnitType.Star) : new GridLength(100)
                });
            }

            var comboBoxStyle = new Style(typeof(ComboBox));
            comboBoxStyle.Setters.Add(new Setter(ComboBox.FontSizeProperty, 36.0));
            comboBoxStyle.Setters.Add(new Setter(ComboBox.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            comboBoxStyle.Setters.Add(new Setter(ComboBox.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            this.Resources.Add(typeof(ComboBox), comboBoxStyle);

            var comboBoxItemStyle = new Style(typeof(ComboBoxItem));
            comboBoxItemStyle.Setters.Add(new Setter(ComboBoxItem.FontSizeProperty, 36.0));
            comboBoxItemStyle.Setters.Add(new Setter(ComboBoxItem.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            comboBoxItemStyle.Setters.Add(new Setter(ComboBoxItem.VerticalAlignmentProperty, VerticalAlignment.Center));
            this.Resources.Add(typeof(ComboBoxItem), comboBoxItemStyle);

            for (int i = 1; i <= 4; i++)
            {
                var verticalLine = new Line
                {
                    X1 = i * 100,
                    Y1 = 0,
                    X2 = i * 100,
                    Y2 = 500,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5
                };
                Grid.SetColumnSpan(verticalLine, 5);
                Grid.SetRowSpan(verticalLine, 5);
                gameGrid.Children.Add(verticalLine);

                var horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = i * 100,
                    X2 = 500,
                    Y2 = i * 100,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5
                };
                Grid.SetColumnSpan(horizontalLine, 5);
                Grid.SetRowSpan(horizontalLine, 5);
                gameGrid.Children.Add(horizontalLine);
            }

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    var comboBox = new ComboBox
                    {
                        Width = 80,
                        Height = 80,
                        Name = $"cb{row}{col}"
                    };
                    this.RegisterName(comboBox.Name, comboBox);
                    comboBox.SelectionChanged += ComboBox_SelectionChanged;
                    Grid.SetRow(comboBox, row);
                    Grid.SetColumn(comboBox, col);
                    gameGrid.Children.Add(comboBox);
                }
            }

            var goToMainWinButton = new Button
            {
                Name = "GoToMainWin",
                Content = "До головного вікна",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 140,
                Height = 60,
                Background = new SolidColorBrush(Color.FromRgb(58, 236, 123))
            };
            goToMainWinButton.Click += GoToMainWin_Click;

            Grid.SetRow(goToMainWinButton, 5);
            Grid.SetColumnSpan(goToMainWinButton, 5);
            gameGrid.Children.Add(goToMainWinButton);

            this.Content = gameGrid;
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
        private void InitializeComboBoxes()
        {
            foreach (var child in gameGrid.Children)
            {
                if (child is ComboBox comboBox)
                {
                    comboBox.Items.Add("×");
                    comboBox.Items.Add("○");
                }
            }
        }
        private void ClearComboBoxes()
        {
            foreach (var child in gameGrid.Children)
            {
                if (child is ComboBox comboBox)
                {
                    comboBox.SelectedItem = null;
                    comboBox.IsEnabled = true;
                    _isXTurn = true;
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox == null || comboBox.SelectedItem == null)
                return;

            string selectedValue = comboBox.SelectedItem.ToString();

            if (selectedValue == "×" && !_isXTurn || selectedValue == "○" && _isXTurn)
            {
                MessageBox.Show("Не ваш хід!");
                comboBox.SelectedItem = null;
                return;
            }

            comboBox.IsEnabled = false;

            _isXTurn = !_isXTurn;

            CheckWinner();
        }
        private async void CheckWinner()
        {
            string[,] grid = new string[5, 5];
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    var comboBox = (ComboBox)FindName($"cb{row}{col}");
                    grid[row, col] = comboBox.SelectedItem as string;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    if (grid[i, j] != null && grid[i, j] == grid[i, j + 1] && grid[i, j + 1] == grid[i, j + 2] && grid[i, j + 2] == grid[i, j + 3])
                    {
                        await Task.Delay(1);
                        MessageBox.Show($"{grid[i, j]} виграв!");
                        ClearComboBoxes();
                        return;
                    }

                    if (grid[j, i] != null && grid[j, i] == grid[j + 1, i] && grid[j + 1, i] == grid[j + 2, i] && grid[j + 2, i] == grid[j +3, i])
                    {
                        await Task.Delay(1);
                        MessageBox.Show($"{grid[j, i]} виграв!");
                        ClearComboBoxes();
                        return;
                    }
                }
            }

            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    if (grid[i, j] != null && grid[i, j] == grid[i + 1, j + 1] && grid[i + 1, j + 1] == grid[i + 2, j + 2] && grid[i + 2, j + 2] == grid[i + 3, j + 3])
                    {
                        await Task.Delay(1);
                        MessageBox.Show($"{grid[i, j]} виграв!");
                        ClearComboBoxes();
                        return;
                    }   

                    if (grid[i, 4 - j] != null && grid[i, 4 - j] == grid[i + 1, 3 - j] && grid[i + 1, 3 - j] == grid[i + 2, 2 - j] && grid[i + 2, 2 - j] == grid[i + 3, 1 - j])
                    {
                        await Task.Delay(1);
                        MessageBox.Show($"{grid[i, 4 - j]} виграв!");
                        ClearComboBoxes();
                        return;
                    }
                }
            }

            bool allDisabled = true;

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    var comboBox = (ComboBox)FindName($"cb{row}{col}");

                    if (comboBox != null && comboBox.IsEnabled)
                    {
                        allDisabled = false;
                        break;
                    }
                }

                if (!allDisabled)
                    break;
            }

            if (allDisabled)
            {
                MessageBox.Show("Нічия"); 
                ClearComboBoxes();
            }         
        }
    }
}
