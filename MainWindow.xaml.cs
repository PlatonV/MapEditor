using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int mat_width;
        int mat_height;
        int cell_size;
        int selectedBlock;
        int[,] mat = new int[100, 100];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitMap(int width, int height)
        {
            mat_width = width;
            mat_height = height;
            cell_size = Math.Min(800 / mat_height, 1280 * 3 / 4 / mat_width);
            selectedBlock = 1;

            for (int i = 0; i <= mat_width * cell_size; i += cell_size)
            {
                Line line = new Line();

                line.Stroke = Brushes.Black;

                line.X1 = i;
                line.X2 = i;
                line.Y1 = 0;
                line.Y2 = mat_height * cell_size;

                line.StrokeThickness = 0.5f;

                Canvas.Children.Add(line);
            }

            for (int i = 0; i <= mat_height * cell_size; i += cell_size)
            {
                Line line = new Line();

                line.Stroke = Brushes.Black;

                line.X1 = 0;
                line.X2 = cell_size * mat_width;
                line.Y1 = i;
                line.Y2 = i;

                line.StrokeThickness = 0.5f;

                Canvas.Children.Add(line);
            }
        }

        private void ResetMap()
        {
            for (int i = 0; i < mat_height; ++i)
            {
                for (int j = 0; j < mat_width; ++j)
                {
                    RemoveBlock(j * cell_size, i * cell_size);
                    mat[i, j] = 0;
                }
            }
        }

        private void PutBlock(int x, int y)
        {
            if (mat[y / cell_size, x / cell_size] != selectedBlock)
            {
                RemoveBlock(x, y);
                mat[y / cell_size, x / cell_size] = selectedBlock;

                Rectangle rect = new Rectangle()
                {
                    Width = cell_size,
                    Height = cell_size,
                    Fill = selectedBlock == 1 ? Brushes.Red : Brushes.Black
                };

                Canvas.SetLeft(rect, x / cell_size * cell_size);
                Canvas.SetTop(rect, y / cell_size * cell_size);

                Canvas.Children.Add(rect);
            }

        }

        private void RemoveBlock(int x, int y)
        {
            mat[y / cell_size, x / cell_size] = 0;

            foreach (UIElement el in Canvas.Children)
            {
                if (Canvas.GetLeft(el) / cell_size == x / cell_size &&
                    Canvas.GetTop(el) / cell_size == y / cell_size)
                {
                    Canvas.Children.Remove(el);
                    break;
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas c = sender as Canvas;
            int x = (int)e.GetPosition(c).X;
            int y = (int)e.GetPosition(c).Y;

            if (Mouse.LeftButton == MouseButtonState.Pressed && x < cell_size * mat_width
                && y < cell_size * mat_height)
            {
                PutBlock(x, y);
            }

            if (Mouse.RightButton == MouseButtonState.Pressed && x < cell_size * mat_width
                && y < cell_size * mat_height)
            {
                RemoveBlock(x, y);
            }
        }

        private void btn_New_Click(object sender, RoutedEventArgs e)
        {
            NewMapDialog dialog = new NewMapDialog();

            if (dialog.ShowDialog() == true)
            {
                InitMap(dialog.MapWidth, dialog.MapHeight);
                btn_New.IsEnabled = true;
                btn_Reset.IsEnabled = true;
                btn_Save.IsEnabled = true;
                btn_Randomize.IsEnabled = true;
                btn_Red.IsEnabled = true;
                btn_Black.IsEnabled = true;
            }
        }

        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                string exportString = "";

                for (int i = 0; i < mat_height; ++i)
                {
                    for (int j = 0; j < mat_width; ++j)
                        exportString += mat[i, j].ToString() + ' ';
                    exportString += Environment.NewLine;
                }
                File.WriteAllText(saveFileDialog.FileName, exportString);
            }
        }

        private void btn_Randomize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetMap();
        }

        private void btn_Red_Click(object sender, RoutedEventArgs e)
        {
            selectedBlock = 1;
            btn_Red.BorderThickness = new Thickness(3);
            btn_Black.BorderThickness = new Thickness(0);
        }

        private void btn_Black_Click(object sender, RoutedEventArgs e)
        {
            selectedBlock = 2;
            btn_Red.BorderThickness = new Thickness(0);
            btn_Black.BorderThickness = new Thickness(3);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
            {
                selectedBlock = 1;
                btn_Red.BorderThickness = new Thickness(3);
                btn_Black.BorderThickness = new Thickness(0);
            }
            if (e.Key == Key.D2)
            {
                selectedBlock = 2;
                btn_Red.BorderThickness = new Thickness(0);
                btn_Black.BorderThickness = new Thickness(3);
            }
        }
    }
}
