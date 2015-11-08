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
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for NewMapDialog.xaml
    /// </summary>
    public partial class NewMapDialog : Window
    {
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        public NewMapDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int w = -1;
            int h = -1;

            try
            {
                w = Convert.ToInt32(tb_Width.Text);
                h = Convert.ToInt32(tb_Height.Text);

                if (w < 1 || h < 1 || w > 100 || h > 100)
                {
                    MessageBox.Show("Please insert values between 1 and 100!", "Error!");
                    tb_Width.Text = "";
                    tb_Height.Text = "";
                    return;
                }

                MapWidth = w;
                MapHeight = h;
                this.DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Please insert valid input!", "Error!");
                tb_Width.Text = "";
                tb_Height.Text = "";
            }  
        }
    }
}
