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
using System.IO;
using System.Collections;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Threading;

namespace IMAGES
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region Лейблы суммы 
            var label2 = new Label();
            label2.Content = "Текущая сумма всех текстбоксов: ";
            label2.HorizontalAlignment = HorizontalAlignment.Left;
            label2.Margin = new Thickness(10, 290, 0, 0);
            Labels.Add(label2);
            Container.Children.Add(label2);
            var label3 = new Label();
            label3.Content = "?";
            label3.HorizontalAlignment = HorizontalAlignment.Left;
            label3.Margin = new Thickness(200, 290, 0, 0);
            Labels.Add(label3);
            Container.Children.Add(label3);
            #endregion

            #region Кнопка выбора файла
            var SolverButton = new Button();
            SolverButton.HorizontalAlignment = HorizontalAlignment.Left;
            SolverButton.Height = 50;
            SolverButton.Margin = new Thickness(10, 0, 0, -330);
            SolverButton.Width = 220;
            SolverButton.Content = "Выбрать викторину";
            SolverButton.Click += new RoutedEventHandler(Button_Click);
            Container.Children.Add(SolverButton);
            #endregion

            ///Заведение таймера.
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start(); 
        }

        /// <summary>
        /// Метод для Таймера. Постоянное обвновление суммы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            sum = 0;
            foreach (var TextBox in TextBosex)
            {
                sum += Double.Parse(TextBox.Text);
            }
            if (Double.IsInfinity(sum))
            {
                Labels[1].Content = "Произошло переполнение";
            }
            else
            {
                Labels[1].Content = sum;
            }
        }

        #region Переменные,списки
        List<String> FileString = new List<string>();
        List<TextBox> TextBosex = new List<TextBox>();
        List<Label> Labels = new List<Label>();
        List<String> CountOfTextBox = new List<string>();
        List<String> RoundString = new List<string>();
        string filePath = String.Empty;
        const int XCord = 50;
        const int YCord = 100;
        #endregion

        /// <summary>
        /// Процедура(эвент) привязывающийся кнопке, созданной динамически
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var TXT in TextBosex) {
                Container.Children.Remove(TXT);
            }
            TextBosex.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            string fileContent = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file 
                filePath = openFileDialog.FileName;
                string FileName = openFileDialog.SafeFileName.ToString().Remove(openFileDialog.SafeFileName.LastIndexOf('.'));
                //Read the contents of the file into a stream 
                var fileStream = openFileDialog.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    bool flag = true;
                    string FileContent = reader.ReadToEnd();
                    FileContent = FileContent.Replace("\r", "").Replace("\n", "");
                    for (int i = 0; i < FileContent.Length & flag; i++)
                    {
                        if (FileContent[i] != ';')
                            fileContent += FileContent[i];
                        else
                            flag = false;
                    }
                    FileString = fileContent.Split(new char[] { '\r', '\n' }).ToList();
                    RoundString = FileString[0].Split(new char[] { '/',',' }).ToList();
                    CountOfTextBox = FileString[0].Split(new char[] { '/' }).ToList();
                    Random rnd = new Random();
                    for (int i = 1; i<CountOfTextBox.Count-1; i++)
                    {
                        var textbox = new TextBox();
                        textbox.HorizontalAlignment = HorizontalAlignment.Left;
                        textbox.Height = 24;
                        #region X-temp
                        double x1 = 0;
                        double x2 = 0;
                        double x3 = 0;
                        double x4 = 0;
                        try
                        {
                            x1 = Double.Parse(RoundString[i]);
                        }
                        catch
                        {
                            x1 = rnd.Next(XCord, YCord);
                        }
                        try
                        {
                            x2 = Double.Parse(RoundString[i+1]);
                        }
                        catch
                        {
                            x2 = rnd.Next(XCord, YCord);
                        }
                        try
                        {
                            x3 = Double.Parse(RoundString[i+2]);
                        }
                        catch
                        {
                            x3 = rnd.Next(XCord, YCord);
                        }
                        try
                        {
                            x4 = Double.Parse(RoundString[i+3]);
                        }
                        catch
                        {
                            x4 = rnd.Next(XCord, YCord);
                        }
                        #endregion
                        textbox.Margin = new Thickness(x1,x2,x3,x4);
                        textbox.VerticalAlignment = VerticalAlignment.Top;
                        textbox.Width = 50;
                        textbox.Text = "0";
                        textbox.Tag = "Numbers";
                        textbox.TextChanged += new TextChangedEventHandler(TextBox_TextChanged);   
                        TextBosex.Add(textbox);
                        Container.Children.Add(textbox);
                    }
                    //Labels[1].Content = CountOfTextBox[CountOfTextBox.Count-1].ToString();
                    //ControlSum = Double.Parse(CountOfTextBox[CountOfTextBox.Count - 1]);
                    Labels[1].Content = '0';
                }

            }
        }

        #region Переменные 
        Double sum = 0;
        #endregion

        /// <summary>
        /// Эвент для каждого TextBox, проверка на текст, переполнение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            sum = 0;
            foreach(var TextBox in TextBosex)
            {
                try
                {
                  Double.Parse(TextBox.Text);
                }
                catch (OverflowException)
                {
                    TextBox.Text = "0";
                }
                catch (FormatException)
                {
                    TextBox.Text = "0";
                }
            }
        }
    }
}
