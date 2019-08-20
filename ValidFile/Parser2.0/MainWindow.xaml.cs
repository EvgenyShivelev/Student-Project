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

namespace Parser2._0
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


        /// <summary>
        /// Класс для добавления в listview
        /// </summary>
        class Module
        {
            private string mainmodulename;
            private string childmodulename;

            public string ModuleName
            {
                get { return mainmodulename; }
                set { mainmodulename = value; }
            }
            public string ChildModuleName
            {
                get { return childmodulename; }
                set { childmodulename = value; }
            }
            public Module(string Name, string ModulesName)
            {
                ModuleName = Name;
                ChildModuleName = ModulesName;
            }
        }


        /// <summary>
        /// Процедура для рекурсивного прохода по всем модулям.
        /// </summary>
        /// <param name="path">Путь основного файла</param>
        /// <param name="FileName">Название модуля</param>
        /// <param name="filePath"></param>
        static public void Analysismodule(string path, string FileName, string filePath)
        {
            string fileContent = string.Empty;
            using (StreamReader reader = new StreamReader(@path))
            {
                Match match = RegexForSearch.Match(fileContent = reader.ReadToEnd());
                if (match.Success)
                {
                    string ModulesNames = (Regex.Replace(match.Groups["Modules"].ToString(), @"(//.*)|({.*})", "")).Replace("\r", "").Replace("\n", ",").Replace(" ", "");
                    List<String> ArrayModulesNames = ModulesNames.Split(new char[] { ',',' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Module Module = new Module(FileName, ModulesNames.Replace(",", " ").Trim(' '));
                    Modules.Add(Module);
                    ModulesNameForRepeats.Add(FileName);
                    for (int i = 0; i < ArrayModulesNames.Count; i++)
                    {
                        try
                        {
                            if (!ModulesNameForRepeats.Contains(ArrayModulesNames[i].ToLower()) & !ModulesNameForRepeats.Contains(ArrayModulesNames[i].ToUpper()) & !ModulesNameForRepeats.Contains(ArrayModulesNames[i]))
                            {
                                Analysismodule(filePath.ToString().Remove(filePath.LastIndexOf("\\") + 1) + ArrayModulesNames[i] + ".pas", ArrayModulesNames[i], filePath);
                            }
                        }
                        catch
                        {
                            Module ChildModule = new Module(ArrayModulesNames[i], "NOT FOUND or SYSTEM *REC*");
                            Modules.Add(ChildModule);
                            ModulesNameForRepeats.Add(ArrayModulesNames[i]);
                        }
                    }
                }
            }
        }

        #region Переменные, Списки, регулярки
     static Regex RegexForSearch = new Regex(@"[u,U]ses(?<Modules>.*?);*$", RegexOptions.Singleline);
    static Regex RegexForDelete = new Regex(@"(//.*)|({.*})");
    static List<string> ModulesNameForRepeats = new List<string>();
    static List<Module> Modules = new List<Module>();
        #endregion

        /// <summary>
/// Эвент кнопки, содержит похожий код, что и для рекурсивного обхода.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
    {
            Modules.Clear();
            ModulesNameForRepeats.Clear();
            ModulesList.ItemsSource = null;
            string filePath = string.Empty;
            string fileContent = string.Empty;
            string StringOne = string.Empty;
            string StringSec = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "(*.pas)|*.pas";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                string FileName = openFileDialog.SafeFileName.ToString().Remove(openFileDialog.SafeFileName.LastIndexOf('.'));
                var fileStream = openFileDialog.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    Match match = RegexForSearch.Match(fileContent = reader.ReadToEnd());
                    if (match.Success)
                    {
                        string ModulesNames = string.Empty;
           
                        string s = Regex.Replace(match.Groups["Modules"].ToString(), @"({.*})|(//.*)", "").Replace("\r", "").Replace("\n", ",").Replace(" ", "");
                        ModulesNames = (Regex.Replace(s, @"({.*})|(//.*)|(;.*)", "")).Replace("\r", "").Replace("\n", ",").Replace(" ", "");
                        Hello.Content = s;
                        //ModulesNames = (Regex.Replace(ModulesNames, @"(//.*)|({.*})", "")).Replace("\r", "").Replace("\n", ",").Replace(" ", "");
                        //beb.Content = ModulesNames.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                        List<String> ArrayModulesNames = ModulesNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        Module Module = new Module(FileName, ModulesNames.Replace(","," ").Trim(' '));
                        Modules.Add(Module);
                        ModulesNameForRepeats.Add(FileName);
                        for (int i = 0; i < ArrayModulesNames.Count; i++)
                        {
                            try
                            {
                                if (!ModulesNameForRepeats.Contains(ArrayModulesNames[i].ToLower()) & !ModulesNameForRepeats.Contains(ArrayModulesNames[i].ToUpper()) & !ModulesNameForRepeats.Contains(ArrayModulesNames[i]))
                                {
                                    Analysismodule(filePath.ToString().Remove(filePath.LastIndexOf("\\") + 1) + ArrayModulesNames[i] + ".pas", ArrayModulesNames[i], filePath);
                                }
                            }
                            catch
                            {
                                Module ChildModule = new Module(ArrayModulesNames[i], "NOT FOUND or SYSTEM");
                                Modules.Add(ChildModule);
                                ModulesNameForRepeats.Add(ArrayModulesNames[i]);
                            }
                        }
                    }
                    ModulesList.ItemsSource = Modules;
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
