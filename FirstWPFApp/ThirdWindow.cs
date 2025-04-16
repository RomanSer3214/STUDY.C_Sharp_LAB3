using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class ThirdWindow : Window
    {
        string output = "0";
        char[] operationsigns = { '.', '+', '-', '×', '÷' };
        private TextBlock OutputText;
        public ThirdWindow()
        {
            CreateThirdWindowUI();
        }

        private void CreateThirdWindowUI()
        {
            this.Title = "Калькулятор";
            this.Height = 650;
            this.Width = 376;
            this.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            for (int i = 0; i < 7; i++)
            {
                RowDefinition row = new RowDefinition();
                if (i == 0)
                    row.Height = new GridLength(100);
                else if (i < 6)
                    row.Height = new GridLength(85);
                else
                    row.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < 4; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });
            }

            OutputText = new TextBlock
            {
                Text = "0",
                Margin = new Thickness(8),
                FontSize = 42,
                TextAlignment = TextAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Grid.SetRow(OutputText, 0);
            Grid.SetColumn(OutputText, 0);
            Grid.SetColumnSpan(OutputText, 4);
            grid.Children.Add(OutputText);

            Border outputBorder = new Border
            {
                BorderBrush = Brushes.DimGray,
                BorderThickness = new Thickness(1),
                Height = 80,
                Width = 340
            };
            Grid.SetRow(outputBorder, 0);
            Grid.SetColumn(outputBorder, 0);
            Grid.SetColumnSpan(outputBorder, 4);
            grid.Children.Add(outputBorder);

            void AddButton(string name, string content, int row, int column, RoutedEventHandler clickHandler)
            {
                Button btn = new Button
                {
                    Name = name,
                    Content = content,
                    Background = new SolidColorBrush(Color.FromRgb(128, 128, 128)),
                    FontSize = 36
                };
                btn.Click += clickHandler;
                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, column);
                grid.Children.Add(btn);
            }

            AddButton("PlusButton", "+", 5, 3, CalcButton_Click);
            AddButton("DotButton", ".", 5, 2, CalcButton_Click);
            AddButton("ZeroButton", "0", 5, 1, CalcButton_Click);
            AddButton("ChangeSignButton", "+/-", 5, 0, CalcButton_Click);
            AddButton("MinusButton", "-", 4, 3, CalcButton_Click);
            AddButton("ThreeButton", "3", 4, 2, CalcButton_Click);
            AddButton("TwoButton", "2", 4, 1, CalcButton_Click);
            AddButton("OneButton", "1", 4, 0, CalcButton_Click);
            AddButton("MultiplyButton", "×", 3, 3, CalcButton_Click);
            AddButton("SixButton", "6", 3, 2, CalcButton_Click);
            AddButton("FiveButton", "5", 3, 1, CalcButton_Click);
            AddButton("FourButton", "4", 3, 0, CalcButton_Click);
            AddButton("DivideButton", "÷", 2, 3, CalcButton_Click);
            AddButton("NineButton", "9", 2, 2, CalcButton_Click);
            AddButton("EightButton", "8", 2, 1, CalcButton_Click);
            AddButton("SevenButton", "7", 2, 0, CalcButton_Click);
            AddButton("ClearEntryButton", "CE", 1, 3, CalcButton_Click);
            AddButton("ClearButton", "C", 1, 2, CalcButton_Click);
            AddButton("EqualsButton", "=", 1, 1, EqualsButton_Click);

            Border emptyBorder = new Border
            {
                BorderBrush = Brushes.DimGray,
                BorderThickness = new Thickness(1)
            };
            Grid.SetRow(emptyBorder, 1);
            Grid.SetColumn(emptyBorder, 0);
            grid.Children.Add(emptyBorder);

            Button goToMain = new Button
            {
                Name = "GoToMainWin",
                Content = "До головного вікна",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(23, 10, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 140,
                Height = 60,
                Background = new SolidColorBrush(Color.FromRgb(58, 236, 123)),
                RenderTransformOrigin = new Point(0.674, 2.717)
            };
            goToMain.Click += GoToMainWin_Click;
            Grid.SetRow(goToMain, 6);
            Grid.SetColumn(goToMain, 1);
            Grid.SetColumnSpan(goToMain, 2);
            grid.Children.Add(goToMain);

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

        private bool NumHasOneDot(string input)
        {
            string[] parts = input.Split(new char[] { '+', '-', '×', '÷' }, StringSplitOptions.RemoveEmptyEntries);
            string currentNumber = parts.Length > 0 ? parts[parts.Length - 1] : "";

            if (currentNumber.Contains("."))
            {
                return true;
            }
            return false;
        }

        private async void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            string result_string = output;

            if (result_string.Length > 1 && Array.Exists(operationsigns, c => c == result_string[result_string.Length - 1]))
            {
                result_string = result_string.Substring(0, result_string.Length - 1);
                output = result_string;
            }

            if (result_string == "-")
                output = "0";

            result_string = output.Replace('÷', '/')
                                  .Replace('×', '*');

            if (result_string.Contains("/0"))
            {
                OutputText.Text = "Error. div by 0";
                await Task.Delay(1000);
                output = "0";
            }
            else
            {
                if (output.Length > 11)
                {
                    OutputText.Text = "OverFlow";
                    await Task.Delay(1000);
                    output = "0";
                }
                else
                {
                    result_string = new DataTable().Compute(result_string, null).ToString();
                    result_string = result_string.Replace(',', '.');
                    output = result_string;
                }
            }
            OutputText.Text = output;
        }
        private async void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            string operation = clickedButton.Name;

            if (output.Length > 12)
            {
                OutputText.Text = "OverFlow";
                await Task.Delay(1000);
                output = "0";
                OutputText.Text = output;
            }
            else
            {
                switch (operation)
                {
                    case "ClearButton":
                        output = "0";
                        OutputText.Text = output;
                        break;

                    case "ClearEntryButton":
                        if (output.Length > 1)
                        {
                            output = output.Substring(0, output.Length - 1);
                            OutputText.Text = output;
                            break;
                        }
                        else
                        {
                            output = "0";
                            OutputText.Text = output;
                            break;
                        }

                    case "ChangeSignButton":
                        if (output.Length < 1)
                            break;
                        if (output.StartsWith("-"))
                        {
                            output = output.Substring(1);
                            OutputText.Text = output;
                        }
                        else
                        {
                            string result = "-" + output;
                            output = result;
                            OutputText.Text = output;
                            break;
                        }
                        break;


                    case "ZeroButton":
                        if (OutputText.Text == "0")
                            break;
                        output += "0";
                        OutputText.Text = output;
                        break;

                    case "OneButton":
                        if (OutputText.Text == "0")
                            output = "1";
                        else
                            output += "1";
                        OutputText.Text = output;
                        break;

                    case "TwoButton":
                        if (OutputText.Text == "0")
                            output = "2";
                        else
                            output += "2";
                        OutputText.Text = output;
                        break;

                    case "ThreeButton":
                        if (OutputText.Text == "0")
                            output = "3";
                        else
                            output += "3";
                        OutputText.Text = output;
                        break;

                    case "FourButton":
                        if (OutputText.Text == "0")
                            output = "4";
                        else
                            output += "4";
                        OutputText.Text = output;
                        break;

                    case "FiveButton":
                        if (OutputText.Text == "0")
                            output = "5";
                        else
                            output += "5";
                        OutputText.Text = output;
                        break;

                    case "SixButton":
                        if (OutputText.Text == "0")
                            output = "6";
                        else
                            output += "6";
                        OutputText.Text = output;
                        break;

                    case "SevenButton":
                        if (OutputText.Text == "0")
                            output = "7";
                        else
                            output += "7";
                        OutputText.Text = output;
                        break;

                    case "EightButton":
                        if (OutputText.Text == "0")
                            output = "8";
                        else
                            output += "8";
                        OutputText.Text = output;
                        break;

                    case "NineButton":
                        if (OutputText.Text == "0")
                            output = "9";
                        else
                            output += "9";
                        OutputText.Text = output;
                        break;

                    case "PlusButton":
                        if (Array.Exists(operationsigns, c => c == output[output.Length - 1]))
                        {
                            output = output.Substring(0, output.Length - 1);
                            output += "+";
                            OutputText.Text = output;
                            break;
                        }
                        output += "+";
                        OutputText.Text = output;
                        break;

                    case "MinusButton":
                        if (Array.Exists(operationsigns, c => c == output[output.Length - 1]))
                        {
                            output = output.Substring(0, output.Length - 1);
                            output += "-";
                            OutputText.Text = output;
                            break;
                        }
                        output += "-";
                        OutputText.Text = output;
                        break;

                    case "MultiplyButton":
                        if (Array.Exists(operationsigns, c => c == output[output.Length - 1]))
                        {
                            output = output.Substring(0, output.Length - 1);
                            output += "×";
                            OutputText.Text = output;
                            break;
                        }
                        output += "×";
                        OutputText.Text = output;
                        break;

                    case "DivideButton":
                        if (Array.Exists(operationsigns, c => c == output[output.Length - 1]))
                        {
                            output = output.Substring(0, output.Length - 1);
                            output += "÷";
                            OutputText.Text = output;
                            break;
                        }
                        output += "÷";
                        OutputText.Text = output;
                        break;

                    case "DotButton":
                        if (NumHasOneDot(output))
                            break;

                        if (Array.Exists(operationsigns, c => c == output[output.Length - 1]))
                        {
                            output = output.Substring(0, output.Length - 1);
                            output += ".";
                            OutputText.Text = output;
                            break;
                        }
                        output += ".";
                        OutputText.Text = output;
                        break;
                }
            }
        }
    }
}