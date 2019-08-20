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

namespace Matrix
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Обьявление всех необходимых листов, массивов и флагов
        /// </summary>
        #region Листы и переменные
        private List<Button> ImportantButtons = new List<Button>();
        private List<TextBox> TextBoxes = new List<TextBox>();
        private TextBox[,] TextBoxMatrix = new TextBox[3, 4];
        private List<Label> ResultLabes = new List<Label>();
        private static double[,] Matrix = new double[3, 4];
        public static bool flag = false;
        #endregion

        /// <summary>
        /// Генерация интерфейса
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InterfaceCreator(Container, TextBoxes, TextBoxMatrix, ResultLabes);
            ButtonCreator(Container, ImportantButtons, SolveButton_Click, NextStep_Click);
        }

        /// <summary>
        /// Покрас БекГраунда
        /// </summary>
        /// <param name="Container"></param>
        /// <param name="TextBoxes"></param>
        /// <param name="TextBoxMatrix"></param>
        /// <param name="ResultLabes"></param>
        private static void InterfaceCreator(Grid Container, List<TextBox> TextBoxes, TextBox[,] TextBoxMatrix, List<Label> ResultLabes)
        {
            App.Current.MainWindow.Background = Brushes.White;
            var Rectangle = new Rectangle();
            Rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            Rectangle.Height = 200;
            Rectangle.Width = 340;
            Rectangle.Margin = new Thickness(60, 0, 0, 200);
            Rectangle.Fill = Brushes.White;

            Container.Children.Add(Rectangle);
            for (int i = 0; i < 3; i++)
            {
                var label = new Label();
                label.Content = "                   X   +                   Y  +                     Z  =  ";
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Margin = new Thickness(60, 90 + (50 * i), 80, 0);
                Container.Children.Add(label);

                var Result = new Label();
                Result.HorizontalAlignment = HorizontalAlignment.Left;
                Result.Margin = new Thickness(70, 325 + (60 * i), 80, 0);
                Result.Content = "-";
                Container.Children.Add(Result);
                ResultLabes.Add(Result);

                for (int j = 0; j < 4; j++)
                {
                    var textbox = new TextBox();
                    textbox.HorizontalAlignment = HorizontalAlignment.Left;
                    textbox.Height = 24;
                    textbox.Margin = new Thickness(70 + 90 * j, 90 + (50 * i), 80, 0);
                    textbox.VerticalAlignment = VerticalAlignment.Top;
                    textbox.Width = 50;
                    textbox.Text = "0";
                    Container.Children.Add(textbox);
                    TextBoxMatrix[i, j] = textbox;
                    TextBoxes.Add(textbox);
                }

            }

        }

        /// <summary>
        /// Динамическое создание кнопок
        /// </summary>
        /// <param name="Container"></param>
        /// <param name="ImportantButtons"></param>
        /// <param name="SolveButton_Click"></param>
        /// <param name="NextStep_Click"></param>
        private static void ButtonCreator(Grid Container, List<Button> ImportantButtons, RoutedEventHandler SolveButton_Click, RoutedEventHandler NextStep_Click)
        {
            var SolverButton = new Button();
            SolverButton.HorizontalAlignment = HorizontalAlignment.Left;
            SolverButton.Height = 30;
            SolverButton.Margin = new Thickness(70, -60, 10, 0);
            SolverButton.Width = 320;
            SolverButton.Name = "SolveButton";
            SolverButton.Content = "Solve?";
            SolverButton.Click += new RoutedEventHandler(SolveButton_Click);
            Container.Children.Add(SolverButton);
            ImportantButtons.Add(SolverButton);
            var NextStep = new Button();
            NextStep.HorizontalAlignment = HorizontalAlignment.Left;
            NextStep.Height = 30;
            NextStep.Margin = new Thickness(70, -60, 10, 0);
            NextStep.Width = 320;
            NextStep.Content = "Next Step";
            NextStep.Name = "NextStep";
            NextStep.Click += new RoutedEventHandler(NextStep_Click);
            NextStep.Visibility = Visibility.Hidden;
            Container.Children.Add(NextStep);
            ImportantButtons.Add(NextStep);
        }

        /// <summary>
        /// Метод заполнения матрицы
        /// </summary>
        /// <param name="TextBoxes"></param>
        /// <param name="Matrix"></param>
        /// <param name="TextBoxMatrix"></param>
        /// <param name="CorrectInput"></param>
        /// <returns>Возвращает заполненную матрицу, либо выдает предупреждение об ошибке</returns>
        private static bool FillMatrix(List<TextBox> TextBoxes, Double[,] Matrix, TextBox[,] TextBoxMatrix, ref bool CorrectInput)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    try
                    {
                        Matrix[i, j] = float.Parse(TextBoxMatrix[i, j].Text);
                        TextBoxMatrix[i, j].BorderBrush = Brushes.DarkCyan;
                    }
                    catch (OverflowException)
                    {
                        TextBoxMatrix[i, j].BorderBrush = Brushes.Violet;
                        CorrectInput = false;
                    }
                    catch (FormatException)
                    {
                        TextBoxMatrix[i, j].BorderBrush = Brushes.Red;
                        CorrectInput = false;
                    }
                }
            }
            return CorrectInput;
        }

        /// <summary>
        /// Чекер для быстрого вывода о решаемости СЛАУ
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        private static bool Checker(Double[,] Matrix)
        {
            return (
                ((Matrix[0, 0] == 0 && Matrix[0, 1] == 0 && Matrix[0, 2] == 0) && Matrix[0, 3] != 0))
              | ((Matrix[1, 0] == 0 && Matrix[1, 1] == 0 && Matrix[1, 2] == 0 && Matrix[1, 3] != 0))
              | ((Matrix[2, 0] == 0 && Matrix[2, 1] == 0 && Matrix[2, 2] == 0 && Matrix[2, 3] != 0));
        }

        /// <summary>
        /// Кнопка, вызов всех методом, метод заранее привязан к кнопке.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            bool CorrectInput = true;
            FillMatrix(TextBoxes, Matrix, TextBoxMatrix, ref CorrectInput);
            if (CorrectInput == true)
            {
                if (Checker(Matrix))
                {
                    ResultLabes[0].Content = "Уравнение не имеет решений";
                }
                else
                {
                    ImportantButtons[0].Visibility = Visibility.Hidden;
                    ImportantButtons[1].Visibility = Visibility.Visible;
                    foreach (TextBox textbox in TextBoxMatrix)
                    {
                        textbox.Background = Brushes.LightGreen;
                        textbox.IsReadOnly = true;
                    }
                    MatrixGenerator = GettingStepwiseMatrix(Matrix).GetEnumerator();
                }
            }

        }

        /// <summary>
        /// Генератор
        /// </summary>
        private IEnumerator<double[,]> MatrixGenerator = GettingStepwiseMatrix(Matrix).GetEnumerator();

        /// <summary>
        /// Эвент продолжения решения, нужно для YIELD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            foreach (var Label in ResultLabes)
            {
                Label.Content = "";
            }
            MatrixGenerator.MoveNext();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    TextBoxMatrix[i, j].Text = MatrixGenerator.Current[i, j].ToString("0.0000");
                }
            }
            if (flag)
            {
                if (Checker(Matrix)) {
                    foreach(var TX in ResultLabes)
                    {
                        TX.Content = "";
                    }
                    ResultLabes[0].Content = "Система не имеет решения";
                }
                else
                {
                    //снова появляется возможность ввести матрицу
                    foreach (TextBox textbox in TextBoxMatrix)
                    {
                        textbox.Background = Brushes.White;
                        textbox.IsReadOnly = false;
                    }
                    if (Double.IsInfinity(Matrix[2, 3] / Matrix[2, 2]) & Double.IsInfinity(Matrix[1, 3] / Matrix[1, 1]) | Double.IsNaN(Matrix[1, 3] / Matrix[1, 1]) & Double.IsNaN(Matrix[2, 3] / Matrix[2, 2]))
                    {
                        ResultLabes[0].Content = $"X = {Matrix[0, 3] / Matrix[0, 0]} - {Matrix[0, 1] / Matrix[0, 0]} * Y - {Matrix[0, 2] / Matrix[0, 1]} * Z  ";
                    }
                    else if (Double.IsInfinity(Matrix[0, 3] / Matrix[0, 0]))
                    {
                        ResultLabes[0].Content = "X = Произвольное число";
                    }
                    else if (Double.IsNaN(Matrix[0, 3] / Matrix[0, 0]))
                    {
                        ResultLabes[0].Content = "X = Произвольное число";
                    }
                    else
                    {
                        ResultLabes[0].Content = (Matrix[0, 3] / Matrix[0, 0]).ToString("0.0000");
                    }
                    if (Double.IsInfinity(Matrix[1, 3] / Matrix[1, 1]))
                    {
                        ResultLabes[1].Content = "Y = Произвольное число";
                    }
                    else if (Double.IsNaN(Matrix[1, 3] / Matrix[1, 1]))
                    {
                        ResultLabes[1].Content = "Y = Произвольное число";
                    }
                    else
                    {
                        ResultLabes[1].Content = (Matrix[1, 3] / Matrix[1, 1]).ToString("0.0000");
                    }

                    if (Double.IsInfinity(Matrix[2, 3] / Matrix[2, 2]))
                    {
                        ResultLabes[2].Content = "Z = Произвольное число";
                    }
                    else if (Double.IsNaN(Matrix[2, 3] / Matrix[2, 2]))
                    {
                        ResultLabes[2].Content = "Z = Произвольное число";
                    }
                    else
                    {
                        ResultLabes[2].Content = (Matrix[2, 3] / Matrix[2, 2]).ToString("0.0000");
                    }

                     if (Double.IsInfinity(Matrix[2, 3] / Matrix[2, 2]) & Double.IsInfinity(Matrix[0, 3] / Matrix[0, 0]) | Double.IsNaN(Matrix[0, 3] / Matrix[0, 0]) & Double.IsNaN(Matrix[2, 3] / Matrix[2, 2]))
                    {
                        ResultLabes[1].Content = $"Y = {Matrix[1, 3] / Matrix[1, 1]} - {Matrix[0, 2] / Matrix[1, 1]} * Z  ";
                    }
                }
               // ResultLabes[2].Content = (Matrix[2, 3] / Matrix[2, 2]).ToString("0.0000");
                ImportantButtons[0].Visibility = Visibility.Visible;
                ImportantButtons[1].Visibility = Visibility.Hidden;
                MatrixGenerator = GettingStepwiseMatrix(Matrix).GetEnumerator();
                flag = !flag;
            }


        }

        /// <summary>
        /// Функция Свапа для переменных
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        private static void swap(ref double x1, ref double x2)
        {
            var temp = x1;
            x1 = x2;
            x2 = temp;
        }

        /// <summary>
        /// Метод решения матрицы
        /// </summary>
        /// <param name="Matrix"></param>
        /// <returns></returns>
        private static IEnumerable<double[,]> GettingStepwiseMatrix(double[,] Matrix)
        {
            if (Matrix[0, 0] == 0) //меняем местами строчки если 1 элемент 1 строки равен 0 
            {
                if (Matrix[1, 0] != 0)
                {
                    swap(ref Matrix[0, 0], ref Matrix[1, 0]);
                    swap(ref Matrix[0, 1], ref Matrix[1, 1]);
                    swap(ref Matrix[0, 2], ref Matrix[1, 2]);
                    swap(ref Matrix[0, 3], ref Matrix[1, 3]);
                    yield return Matrix;
                }
                else if (Matrix[2, 0] != 0)
                {
                    swap(ref Matrix[0, 0], ref Matrix[2, 0]);
                    swap(ref Matrix[0, 1], ref Matrix[2, 1]);
                    swap(ref Matrix[0, 2], ref Matrix[2, 2]);
                    swap(ref Matrix[0, 3], ref Matrix[2, 3]);
                    yield return Matrix;
                }

            }


            if (Matrix[1, 0] != 0) // получаем 0 в 1 элементе 2 строки 
            {
                checked
                {
                    var multiplier1 = Matrix[0, 0];
                    var multiplier2 = Matrix[1, 0];
                    Matrix[1, 0] = Matrix[1, 0] - Matrix[0, 0] * multiplier2 / multiplier1;
                    Matrix[1, 1] = Matrix[1, 1] - Matrix[0, 1] * multiplier2 / multiplier1;
                    Matrix[1, 2] = Matrix[1, 2] - Matrix[0, 2] * multiplier2 / multiplier1;
                    Matrix[1, 3] = Matrix[1, 3] - Matrix[0, 3] * multiplier2 / multiplier1;
                    yield return Matrix;
                }

            }

            if (Matrix[2, 0] != 0)// получаем 0 в 1 элементе 3 строки 
            {
                var multiplier1 = Matrix[0, 0];
                var multiplier2 = Matrix[2, 0];
                Matrix[2, 0] = Matrix[2, 0] - Matrix[0, 0] * multiplier2 / multiplier1;
                Matrix[2, 1] = Matrix[2, 1] - Matrix[0, 1] * multiplier2 / multiplier1;
                Matrix[2, 2] = Matrix[2, 2] - Matrix[0, 2] * multiplier2 / multiplier1;
                Matrix[2, 3] = Matrix[2, 3] - Matrix[0, 3] * multiplier2 / multiplier1;
                yield return Matrix;
            }


            if (Matrix[0, 0] == 0) //рассматриваем когда 1 элемент равен единице 
            {
                if (Matrix[0, 1] == 0) //получаем в 1 элементе 2 столбца не 0 если это возможно 
                {
                    if (Matrix[1, 1] != 0)
                    {
                        swap(ref Matrix[0, 0], ref Matrix[1, 0]);
                        swap(ref Matrix[0, 1], ref Matrix[1, 1]);
                        swap(ref Matrix[0, 2], ref Matrix[1, 2]);
                        swap(ref Matrix[0, 3], ref Matrix[1, 3]);
                        yield return Matrix;
                    }
                    else if (Matrix[2, 1] != 0)
                    {
                        swap(ref Matrix[0, 0], ref Matrix[2, 0]);
                        swap(ref Matrix[0, 1], ref Matrix[2, 1]);
                        swap(ref Matrix[0, 2], ref Matrix[2, 2]);
                        swap(ref Matrix[0, 3], ref Matrix[2, 3]);
                        yield return Matrix;
                    }
                }

                if (Matrix[0, 1] != 0) // зануляем 2 столбец 
                {
                    if (Matrix[1, 1] != 0)
                    {
                        var multiplier1 = Matrix[0, 1];
                        var multiplier2 = Matrix[1, 1];
                        Matrix[1, 1] = Matrix[1, 1] - Matrix[0, 1] * multiplier2 / multiplier1;
                        Matrix[1, 2] = Matrix[1, 2] - Matrix[0, 2] * multiplier2 / multiplier1;
                        Matrix[1, 3] = Matrix[1, 3] - Matrix[0, 3] * multiplier2 / multiplier1;
                        yield return Matrix;
                    }

                    if (Matrix[2, 1] != 0)
                    {
                        var multiplier1 = Matrix[0, 1];
                        var multiplier2 = Matrix[2, 1];
                        Matrix[2, 1] = Matrix[2, 1] - Matrix[0, 1] * multiplier2 / multiplier1;
                        Matrix[2, 2] = Matrix[2, 2] - Matrix[0, 2] * multiplier2 / multiplier1;
                        Matrix[2, 3] = Matrix[2, 3] - Matrix[0, 3] * multiplier2 / multiplier1;
                        yield return Matrix;
                    }
                    if (Matrix[1, 2] != 0) // переходим к 3 столбцу 
                    {
                        if (Matrix[0, 2] != 0)
                        {
                            var multiplier1 = Matrix[1, 2];
                            var multiplier2 = Matrix[0, 2];
                            Matrix[0, 2] = Matrix[0, 2] - Matrix[1, 2] * multiplier2 / multiplier1;
                            Matrix[0, 3] = Matrix[0, 3] - Matrix[1, 3] * multiplier2 / multiplier1;
                            yield return Matrix;
                        }

                        if (Matrix[2, 2] != 0)
                        {
                            var multiplier1 = Matrix[1, 2];
                            var multiplier2 = Matrix[2, 2];
                            Matrix[2, 2] = Matrix[2, 2] - Matrix[1, 2] * multiplier2 / multiplier1;
                            Matrix[2, 3] = Matrix[2, 3] - Matrix[1, 3] * multiplier2 / multiplier1;
                            yield return Matrix;
                        }
                    }
                    else if (Matrix[2, 2] != 0)
                    {
                        if (Matrix[0, 2] != 0)
                        {
                            var multiplier1 = Matrix[2, 2];
                            var multiplier2 = Matrix[0, 2];
                            Matrix[0, 2] = Matrix[0, 2] - Matrix[2, 2] * multiplier2 / multiplier1;
                            Matrix[0, 3] = Matrix[0, 3] - Matrix[2, 3] * multiplier2 / multiplier1;
                        }
                        swap(ref Matrix[1, 2], ref Matrix[2, 2]);
                        swap(ref Matrix[1, 3], ref Matrix[2, 3]);
                        yield return Matrix;
                    }
                }
                else //поскольку 2 столбец состоит из 0 переходим к 3 столбцу 
                {
                    if (Matrix[0, 2] == 0)
                    {
                        if (Matrix[1, 2] != 0)
                        {
                            swap(ref Matrix[0, 2], ref Matrix[1, 2]);
                            swap(ref Matrix[0, 3], ref Matrix[1,
                            3]);
                        }
                        if (Matrix[2, 2] != 0)
                        {
                            swap(ref Matrix[0, 2], ref Matrix[2, 2]);
                            swap(ref Matrix[0, 3], ref Matrix[2, 3]);
                        }
                    }
                    if (Matrix[1, 2] != 0)
                    {
                        Matrix[1, 2] = 0;
                        yield return Matrix;
                    }
                    if (Matrix[2, 2] != 0)
                    {
                        Matrix[2, 2] = 0;
                        yield return Matrix;
                    }
                }

            }
            else // расматриваем случай когда 1 элемент 1 столбца не 0 
            {
                if (Matrix[1, 1] == 0)
                {
                    if (Matrix[2, 1] != 0) // получаем в 2 элементе 2 строки не 0 если это возможно 
                    {
                        swap(ref Matrix[1, 0], ref Matrix[2, 0]);
                        swap(ref Matrix[1, 1], ref Matrix[2, 1]);
                        swap(ref Matrix[1, 2], ref Matrix[2, 2]);
                        swap(ref Matrix[1, 3], ref Matrix[2, 3]);
                        yield return Matrix;
                    }
                    else if (Matrix[0, 1] != 0) //случай когда 1 и 2 элемент 1 строки не 0 
                    {
                        if (Matrix[1, 2] != 0)
                        {
                            Matrix[0, 2] = 0;
                            yield return Matrix;
                            Matrix[2, 2] = 0;
                            yield return Matrix;
                        }
                        if (Matrix[2, 2] != 0)
                        {
                            Matrix[0, 2] = 0;

                            yield return Matrix;
                        }
                    }
                    else // случай когда 1 элемент 1 строки не равен 0 а 2 элемент равен 0 
                         //приоритет даётся 3 элементу 1 строки 
                    {
                        if (Matrix[0, 2] != 0)
                        {
                            Matrix[1, 2] = 0;
                            yield return Matrix;
                            Matrix[2, 2] = 0;
                            yield return Matrix;
                        }
                        if (Matrix[1, 2] != 0)
                        {
                            Matrix[2, 2] = 0;
                            //swap(ref Matrix[0, 2], ref Matrix[1, 2]); 
                            yield return Matrix;
                        }
                        if (Matrix[2, 2] != 0)
                        {
                            swap(ref Matrix[1, 2], ref Matrix[2, 2]);
                            yield return Matrix;
                        }
                    }

                }


                if (Matrix[0, 1] != 0 & Matrix[1, 1] != 0) // случай когда 1 элемент 1 строки не 0 и 2 элемент 2 строки не 0 
                {
                    var multiplier1 = Matrix[1, 1];
                    var multiplier2 = Matrix[0, 1];

                    Matrix[0, 1] = Matrix[0, 1] - Matrix[1, 1] * multiplier2 / multiplier1;
                    Matrix[0, 1] = Math.Round(Matrix[0, 1]);
                    Matrix[0, 2] = Matrix[0, 2] - Matrix[1, 2] * multiplier2 / multiplier1;
                    Matrix[0, 3] = Matrix[0, 3] - Matrix[1, 3] * multiplier2 / multiplier1;
                    yield return Matrix;
                }
                if (Matrix[2, 1] != 0)
                {
                    var multiplier1 = Matrix[1, 1];
                    var multiplier2 = Matrix[2, 1];

                    Matrix[2, 1] = Matrix[2, 1] - Matrix[1, 1] * multiplier2 / multiplier1;
                    Matrix[2, 2] = Matrix[2, 2] - Matrix[1, 2] * multiplier2 / multiplier1;
                    Matrix[2, 3] = Matrix[2, 3] - Matrix[1, 3] * multiplier2 / multiplier1;
                    yield return Matrix;
                }
                if (Matrix[2, 2] != 0)
                {
                    Double multiplier2 = Matrix[1, 2];
                    Double multiplier1 = Matrix[2, 2];
                    Matrix[1, 2] = Matrix[1, 2] - Matrix[2, 2] * multiplier2 / multiplier1;
                    Matrix[1, 3] = Matrix[1, 3] - Matrix[2, 3] * multiplier2 / multiplier1;
                    var xyz = multiplier2 / multiplier1;
                    //Matrix[1, 2] = 0; 
                    yield return Matrix;
                    multiplier2 = Matrix[0, 2];
                    multiplier1 = Matrix[2, 2];
                    Matrix[0, 2] = Matrix[0, 2] - Matrix[2, 2] * multiplier2 / multiplier1;
                    Matrix[0, 3] = Matrix[0, 3] - Matrix[2, 3] * multiplier2 / multiplier1;
                    //Matrix[0, 2] = 0; 
                    yield return Matrix;
                }

                /* if (Matrix[1,2] != 0) 
                { 
                Matrix[0, 2] = 0; 
                yield return Matrix; 
                } 
                */
            }
            flag = true;
            yield return Matrix;
        }


    }


}
