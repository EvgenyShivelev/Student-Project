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

namespace WPFPyramidx2
{
    class Tringle
    {
        const int Size = 7;
        const int SizeL = 12;
        private Line[] lines = new Line[SizeL];
        private double[,] cor = new double[Size, 3];
        private double[,] cors = new double[Size, 2];
        private double[,] coru = new double[Size, 2];
        /// <summary>
        /// построение точек по математическим формулам
        /// </summary>
        /// <param name="h">высота</param>
        /// <param name="s">площадь основания</param>
        /// <param name="corw">мировые координаты вершины</param>
        public Tringle(int h, int s, double[] corw)
        {
            for (int i = 0; i < SizeL; i++)
            {
                lines[i] = new Line();
                lines[i].Stroke = Brushes.Black;
            }
            double sp = Math.Sqrt(s);
            for (int i = 0; i < 3; i++)
                cor[0, i] = corw[i];
            cor[5, 0] = corw[0];
            cor[5, 1] = corw[1];
            cor[5, 2] = corw[2] + 2 * h;
            for (int i = 0; i < Size - 3; i++)
            {
                cor[i + 1, 2] = corw[2] + h;
                if (i < 2)
                    cor[i + 1, 1] = corw[1] - (sp / 2);
                else
                    cor[i + 1, 1] = corw[1] + (sp / 2);
                if (i == 0 || i == 3)
                    cor[i + 1, 0] = corw[0] + (sp / 2);
                else
                    cor[i + 1, 0] = corw[0] - (sp / 2);
            }
            int k = 0;
            for (int i = 0; i < Size - 1; i++)
            {
                cor[Size - 1, 0] += cor[i, 0];
                cor[Size - 1, 1] += cor[i, 1];
                cor[Size - 1, 2] += cor[i, 2];
                k++;
            }
            for (int i = 0; i < 3; i++)
                cor[Size - 1, i] /= k;
        }
        public Line this[int i]
        {
            get { return lines[i]; }
        }
        /// <summary>
        /// вращение вокруг оси X
        /// </summary>
        /// <param name="n">градусы вращения</param>
        /// <param name="plus">переменая для определения вращения по часовой или против</param>
        public void TurnX(double n, bool plus)
        {
            double Y, Z;
            for (int i = 0; i < Size; i++)
            {
                Y = cor[i, 1];
                Z = cor[i, 2];
                if (plus)
                {
                    cor[i, 1] = (Y - cor[Size - 1, 1]) * Math.Cos(n) - (Z - cor[Size - 1, 2]) * Math.Sin(n) + cor[Size - 1, 1];
                    cor[i, 2] = (Y - cor[Size - 1, 1]) * Math.Sin(n) + (Z - cor[Size - 1, 2]) * Math.Cos(n) + cor[Size - 1, 2];
                }
                else
                {
                    cor[i, 1] = (Y - cor[Size - 1, 1]) * Math.Cos(n) + (Z - cor[Size - 1, 2]) * Math.Sin(n) + cor[Size - 1, 1];
                    cor[i, 2] = -(Y - cor[Size - 1, 1]) * Math.Sin(n) + (Z - cor[Size - 1, 2]) * Math.Cos(n) + cor[Size - 1, 2];
                }

            }
        }
        /// <summary>
        /// вращение вокруг оси Z
        /// </summary>
        /// <param name="n">градусы вращения</param>
        /// <param name="plus">переменая для определения вращения по часовой или против</param>
        public void TurnZ(double n, bool plus)
        {
            double Y, X;
            for (int i = 0; i < Size; i++)
            {
                Y = cor[i, 1];
                X = cor[i, 0];
                if (plus)
                {
                    cor[i, 0] = (X - cor[Size - 1, 0]) * Math.Cos(n) - (Y - cor[Size - 1, 1]) * Math.Sin(n) + cor[Size - 1, 0];
                    cor[i, 1] = (X - cor[Size - 1, 0]) * Math.Sin(n) + (Y - cor[Size - 1, 1]) * Math.Cos(n) + cor[Size - 1, 1];
                }
                else
                {
                    cor[i, 0] = (X - cor[Size - 1, 0]) * Math.Cos(n) + (Y - cor[Size - 1, 1]) * Math.Sin(n) + cor[Size - 1, 0];
                    cor[i, 1] = -(X - cor[Size - 1, 0]) * Math.Sin(n) + (Y - cor[Size - 1, 1]) * Math.Cos(n) + cor[Size - 1, 1];
                }
            }
        }
        /// <summary>
        /// вращение вокруг оси Y
        /// </summary>
        /// <param name="n">градусы вращения</param>
        /// <param name="plus">переменая для определения вращения по часовой или против</param>
        public void TurnY(double n, bool plus)
        {
            double Z, X;
            double[] mas = new double[3];
            for (int i = 0; i < Size; i++)
            {
                Z = cor[i, 2];
                X = cor[i, 0];
                if (plus)
                {
                    cor[i, 0] = (X - cor[Size - 1, 0]) * Math.Cos(n) - (Z - cor[Size - 1, 2]) * Math.Sin(n) + cor[Size - 1, 0];
                    cor[i, 2] = (X - cor[Size - 1, 0]) * Math.Sin(n) + (Z - cor[Size - 1, 2]) * Math.Cos(n) + cor[Size - 1, 2];
                }
                else
                {
                    cor[i, 0] = (X - cor[Size - 1, 0]) * Math.Cos(n) + (Z - cor[Size - 1, 2]) * Math.Sin(n) + cor[Size - 1, 0];
                    cor[i, 2] = -(X - cor[Size - 1, 0]) * Math.Sin(n) + (Z - cor[Size - 1, 2]) * Math.Cos(n) + cor[Size - 1, 2];
                }
            }
        }
        /// <summary>
        /// переход от мировых координат к экранным
        /// </summary>
        /// <param name="focus">фокус</param>
        /// <param name="Zs">экранная плоскость</param>
        /// <param name="worldands">x1,x2,xs1,xs2</param>
        public void TransformXY(double[] focus, double Zs, double[,] worldands)
        {
            for (int i = 0; i < Size; i++)
            {
                coru[i, 0] = ((Zs - focus[2]) / (cor[i, 2] - focus[2])) * (cor[i, 0] - focus[0]) + focus[0];
                coru[i, 1] = ((Zs - focus[2]) / (cor[i, 2] - focus[2])) * (cor[i, 1] - focus[1]) + focus[1];
            }
            for (int i = 0; i < Size; i++)
            {
                cors[i, 0] = (coru[i, 0] - worldands[0, 0]) * ((worldands[3, 0] - worldands[2, 0]) / (worldands[1, 0] - worldands[0, 0])) + worldands[2, 0];
                cors[i, 1] = (coru[i, 1] - worldands[0, 1]) * ((worldands[3, 1] - worldands[2, 1]) / (worldands[1, 1] - worldands[0, 1])) + worldands[2, 1];
            }
        }
        /// <summary>
        /// сдвиг
        /// </summary>
        /// <param name="a">сдвиг по оси X</param>
        /// <param name="b">сдвиг по оси Y</param>
        /// <param name="c">сдвиг по оси Z</param>
        public void Shift(double a = 0, double b = 0, double c = 0)
        {
            for (int i = 0; i < Size; i++)
            {
                cor[i, 0] += a;
                cor[i, 1] += b;
                cor[i, 2] += c;
            }
        }
        /// <summary>
        /// построение пирамиды
        /// </summary>
        public void BuildLine()
        {
            for (int i = 0; i < 4; i++)
            {
                lines[i].X1 = cors[0, 0];
                lines[i].Y1 = cors[0, 1];
                lines[i].X2 = cors[i + 1, 0];
                lines[i].Y2 = cors[i + 1, 1];
            }
            for (int i = 0; i < 4; i++)
            {
                lines[i + 4].X1 = cors[i + 1, 0];
                lines[i + 4].Y1 = cors[i + 1, 1];
                if (i < 3)
                {
                    lines[i + 4].X2 = cors[i + 2, 0];
                    lines[i + 4].Y2 = cors[i + 2, 1];
                }
                else
                {
                    lines[i + 4].X2 = cors[1, 0];
                    lines[i + 4].Y2 = cors[1, 1];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                lines[i + 8].X1 = cors[5, 0];
                lines[i + 8].Y1 = cors[5, 1];
                lines[i + 8].X2 = cors[i + 1, 0];
                lines[i + 8].Y2 = cors[i + 1, 1];
            }
        }
    }

    public partial class MainWindow : Window
    {
        double[] focus = { 0, 75, -200 };
        const double Zs = -50;
        double[,] worldands =
        {
            {-100,100},
            {100,-100},
            {-500,500},
            {500,-500}
        };
        const int parametr = 40;
        static double[] corw = { 0, 0, 0 };
        Tringle t = new Tringle(50, 2500, corw);
        double a, b, c;
        public MainWindow()
        {
            InitializeComponent();
            t.TransformXY(focus, Zs, worldands);
            t.BuildLine();
            for (int i = 0; i < 12; i++)
            {
                tringle.Children.Add(t[i]);
                Canvas.SetLeft(t[i], 100);
                Canvas.SetTop(t[i], 50);
            }
        }
        /// <summary>
        /// считывание данных с текстбокса
        /// </summary>
        /// <param name="e">текстбокс с которого считывается сдвиг по какой либо оси</param>
        /// <returns></returns>
        double ParseXYZ(TextBox e)
        {
            double k;
            try
            {
                if (e.Text != "")
                    k = double.Parse(e.Text);
                else k = 0;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Incorect Format");
                throw;
            }
            catch (System.OverflowException)
            {
                MessageBox.Show("Overflow");
                throw;
            }
            return k;
        }

        private void RotationX(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TurnX.Value > a)
                t.TurnX(((TurnX.Value * Math.PI) / 180) / parametr, true);
            else
                t.TurnX(((TurnX.Value * Math.PI) / 180) / parametr, false);
            t.TransformXY(focus, Zs, worldands);
            t.BuildLine();
            a = TurnX.Value;
        }

        private void RotationY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TurnY.Value > b)
                t.TurnY(((TurnY.Value * Math.PI) / 180) / parametr, true);
            else
                t.TurnY(((TurnY.Value * Math.PI) / 180) / parametr, false);
            t.TransformXY(focus, Zs, worldands);
            t.BuildLine();
            b = TurnY.Value;
        }

        private void RotationZ(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TurnZ.Value > c)
                t.TurnZ(((TurnZ.Value * Math.PI) / 180) / parametr, true);
            else
                t.TurnZ(((TurnZ.Value * Math.PI) / 180) / parametr, false);
            t.TransformXY(focus, Zs, worldands);
            t.BuildLine();
            c = TurnZ.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double a, b, c;
            try
            {
                a = ParseXYZ(X);
                b = ParseXYZ(Y);
                c = ParseXYZ(Z);
                t.Shift(a, b, c);
                t.TransformXY(focus, Zs, worldands);
            }
            catch (System.OverflowException)
            {

            }
            catch (System.FormatException)
            {

            }
            t.BuildLine();
        }
    }
}
