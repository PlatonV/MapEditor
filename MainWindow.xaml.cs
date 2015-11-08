using Microsoft.Win32;
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
using System.Windows.Navigation;
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

            mat_width = 10;
            mat_height = 10;
            cell_size = 800 / mat_height;
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

        private void PutBlock(int x, int y)
        {
            mat[y / cell_size, x / cell_size] = selectedBlock;

            Rectangle rect = new Rectangle()
            {
                Width = cell_size,
                Height = cell_size,
                Fill = Brushes.Red
            };

            Canvas.SetLeft(rect, x / cell_size * cell_size);
            Canvas.SetTop(rect, y / cell_size * cell_size);

            Canvas.Children.Add(rect);
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
                && y < cell_size * mat_height && mat[x / cell_size, y / cell_size] == 0)
            {
                PutBlock(x, y);
            }

            if (Mouse.RightButton == MouseButtonState.Pressed && x < cell_size * mat_width
                && y < cell_size * mat_height && mat[x / cell_size, y / cell_size] != 0)
            {
                RemoveBlock(x, y);
            }
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
    }
}
