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

namespace Canguru.WPF
{
    /// <summary>
    /// Lógica interna para EditarPerfilWindow.xaml
    /// </summary>
    public partial class EditarPerfilWindow : Window
    {
        public EditarPerfilWindow()
        {
            InitializeComponent();
        }

        private void btnCancelarAtualiza_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
