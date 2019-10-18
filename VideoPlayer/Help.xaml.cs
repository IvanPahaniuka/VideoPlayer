using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VideoPlayer
{
    /// <summary>
    /// Логика взаимодействия для Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        private bool isAnimationPlay = false;

        public Help()
        {
            InitializeComponent();

            (imgPath.Resources["xxStory"] as Storyboard).Completed += AnimCompleted;
        }

        private void Button_Click(object sender = null, RoutedEventArgs e = null)
        {
            Close();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void AnimCompleted(object sender, EventArgs e)
        {
            isAnimationPlay = false;
        }
        private void imgPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (isAnimationPlay)
                return;

            isAnimationPlay = true;

            imgPath.BeginStoryboard(imgPath.Resources["xxStory"] as Storyboard);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H)
                Close();
        }
    }
}
