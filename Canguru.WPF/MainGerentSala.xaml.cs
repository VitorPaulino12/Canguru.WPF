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
using Canguru.Core;
using Canguru.WPF;

namespace Canguru.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGerentClasse : Window
    {
        private Usuario usuarioLogado;
        public MainGerentClasse(Usuario usuario)
        {
            InitializeComponent();
            NomeProf.Text = usuario.Nome;
            usuarioLogado = usuario;
            if (!string.IsNullOrEmpty(usuarioLogado.CaminhoFotoPerfil))
            {
                try
                {
                    string caminhoFoto = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "FotosPerfil",
                        usuarioLogado.CaminhoFotoPerfil);

                    if (System.IO.File.Exists(caminhoFoto))
                    {
                        var imagem = new BitmapImage(new Uri(caminhoFoto));
                        FotoProfBrush.ImageSource = imagem;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar foto de perfil: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {

            TelaHome telaHome = new TelaHome(usuarioLogado);
                telaHome.Show();
                this.Close();
        }

        private void NomeProf_TextChanged(object sender, TextChangedEventArgs e)
        {
            
                //NomeProf.Text = usuarioLogado.Nome;
        }
    }
}
