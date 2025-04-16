using System;
using System.Collections.Generic;
using System.IO;
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
    public class Student
    {
        private string studentID;
        private string fullName;
        private string personalData;

        public string GetStudentID()
        {
            return studentID;
        }

        public void SetStudentID(string value)
        {
            studentID = value;
        }

        public void SetFullName(string value)
        {
            fullName = value;
        }

        public void SetPersonalData(string value)
        {
            personalData = value;
        }

        public override string ToString()
        {
            return $"{studentID}, {fullName}, {personalData}";
        }

        public static Student FromString(string input)
        {
            var parts = input.Split(',');
            var student = new Student();
            student.studentID = parts[0].Trim();
            student.fullName = parts[1].Trim();
            student.personalData = parts[2].Trim();
            return student;
        }
    }

    public partial class FirstWindow : Window
    {
        private TextBox StudentIDTB;
        private TextBox FullNameTB;
        private TextBox PersonalDataTB;
        private TextBox FileContentTB;

        public FirstWindow()
        {
            CreateFirstWindowUI();
        }
        private void CreateFirstWindowUI()
        {
            this.Title = "Дані студентів";
            this.Height = 500;
            this.Width = 800;
            this.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            Grid grid = new Grid();
            this.Content = grid;

            StudentIDTB = new TextBox
            {
                Name = "StudentIDTB",
                Width = 260,
                Height = 40,
                Margin = new Thickness(10, 40, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Номер залікової книжки"
            };
            grid.Children.Add(StudentIDTB);

            FullNameTB = new TextBox
            {
                Name = "FullNameTB",
                Width = 260,
                Height = 40,
                Margin = new Thickness(10, 100, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "ПІП"
            };
            grid.Children.Add(FullNameTB);

            PersonalDataTB = new TextBox
            {
                Name = "PersonalDataTB",
                Width = 260,
                Height = 40,
                Margin = new Thickness(10, 160, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Особисті дані"
            };
            grid.Children.Add(PersonalDataTB);

            Button addStudentBtn = new Button
            {
                Content = "Додати студента",
                Width = 120,
                Height = 30,
                Margin = new Thickness(10, 240, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            addStudentBtn.Click += AddStudentButton_Click;
            grid.Children.Add(addStudentBtn);

            Button deleteStudentBtn = new Button
            {
                Content = "Видалити студента",
                Width = 120,
                Height = 30,
                Margin = new Thickness(150, 240, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            deleteStudentBtn.Click += DeleteStudentButton_Click;
            grid.Children.Add(deleteStudentBtn);

            FileContentTB = new TextBox
            {
                Name = "FileContentTB",
                Width = 360,
                Height = 300,
                Margin = new Thickness(0, 40, 10, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                IsReadOnly = true,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            grid.Children.Add(FileContentTB);

            Button goToMainWinBtn = new Button
            {
                Name = "GoToMainWin",
                Content = "До головного вікна",
                Width = 142,
                Height = 60,
                Margin = new Thickness(50, 0, 0, 50),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromRgb(58, 236, 123))
            };
            goToMainWinBtn.Click += GoToMainWin_Click;
            grid.Children.Add(goToMainWinBtn);
            LoadFileContent();
        }
    

        private const string FilePath = "students.txt";
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

        private void LoadFileContent()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string fileContent = File.ReadAllText(FilePath);
                    FileContentTB.Text = fileContent;
                }
                else
                {
                    FileContentTB.Text = "Файл не знайдено.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зчитуванні файлу {ex.Message}");
            }
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var studentID = StudentIDTB.Text;
            var fullName = FullNameTB.Text;
            var personalData = PersonalDataTB.Text;

            if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(personalData))
            {
                MessageBox.Show("Заповніть всі поля");
                return;
            }

            var student = new Student();
            student.SetStudentID(studentID);
            student.SetFullName(fullName);
            student.SetPersonalData(personalData);

            try
            {
                File.AppendAllText(FilePath, student.ToString() + Environment.NewLine);
                LoadFileContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при додаванні студента {ex.Message}");
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var studentIDToDelete = StudentIDTB.Text;

            if (string.IsNullOrEmpty(studentIDToDelete))
            {
                MessageBox.Show("Введіть номер залікової книжки для видалення");
                return;
            }

            try
            {
                var students = File.ReadAllLines(FilePath)
                                   .Select(line => Student.FromString(line))
                                   .ToList();

                var studentToRemove = students.FirstOrDefault(s => s.GetStudentID() == studentIDToDelete);

                if (studentToRemove != null)
                {
                    students.Remove(studentToRemove);
                    File.WriteAllLines(FilePath, students.Select(s => s.ToString()));

                    MessageBox.Show("Студента видалено успішно");
                    LoadFileContent();
                }
                else
                {
                    MessageBox.Show("Студент не знайдений");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при видаленні студента: {ex.Message}");
            }
        }
    }
}