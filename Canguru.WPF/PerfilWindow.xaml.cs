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
    /// Lógica interna para PerfilWindow.xaml
    /// </summary>
    public partial class PerfilWindow : Window
    {
        public PerfilWindow()
        {
            InitializeComponent();
        }

        private void btnEditarInformacoes_Click(object sender, RoutedEventArgs e)
        {
            EditarPerfilWindow Editar = new EditarPerfilWindow();
            Editar.Show();
        }

        private void btnEditarSobreMim_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
