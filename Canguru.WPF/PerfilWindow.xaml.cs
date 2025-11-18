using Canguru.Core;
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
        private Usuario usuarioLogado; // pra conseguir pegar o usuário que veio da tela Home
        private string pathImagemSelecionada = string.Empty; //guarda a imagem do usuário para usar nos paineis
        public PerfilWindow(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            NomeUsuarioLogado.Text = usuarioLogado.Nome;
            txtEmail.Text = usuarioLogado.Email;
            if (usuarioLogado.Tipo == TipoUsuario.Aluno)
            {
                txtTipoUsuario.Text = "ALUNO";
            }else { txtTipoUsuario.Text = "PROFESSOR"; }

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
                        imgPerfil.ImageSource = imagem;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar foto de perfil: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

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
