using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VideoPlayer
{
    /// <summary>
    /// Логика взаимодействия для Help.xaml
    /// </summary>
    public partial class FileWindow : Window
    {
        private string filter;

        public FileWindow(IEnumerable<string> sources, string filter = "Все файлы|*.*")
        {
            InitializeComponent();

            this.filter = filter;
            list.Items.AddRange(sources);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close(true);
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenSourcesDialog();
        }
        private void list_MouseDown(object sender, MouseButtonEventArgs e)
        {
            list.RemoveSelection();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            list.Items.TryRemoveAt(list.SelectedIndex);
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            list.Items.Clear();
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            list.TryMoveSelected(list.SelectedIndex + 1);
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            list.TryMoveSelected(list.SelectedIndex - 1);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (Key.S): Close(true); break;
                case (Key.Escape): Close(false); break;
                case (Key.A):
                case (Key.OemPlus): OpenSourcesDialog(); break;
                case (Key.D):
                case (Key.OemMinus): list.Items.TryRemoveAt(list.SelectedIndex); break;
                case (Key.F): list.RemoveSelection(); break;
                case (Key.C): list.Items.Clear(); break;
                case (Key.N): list.TryMoveSelected(list.SelectedIndex + 1); break;
                case (Key.U): list.TryMoveSelected(list.SelectedIndex - 1); break;
            }
        }
    }
}
