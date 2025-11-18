using Canguru.Business;
using Canguru.Core;
using System.Windows;

namespace Canguru.WPF
{
    public partial class CriacaoSessao : Window
    {
        private Usuario usuarioLogado;

        public CriacaoSessao(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNomeSessao.Text.Trim();
            string descricao = txtDescricaoSessao.Text.Trim();

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Por favor, insira o nome da sessão.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GerenciadorGlobal.AdicionarSessao(nome, descricao);
            MessageBox.Show("Sessão adicionada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            txtNomeSessao.Clear();
            txtDescricaoSessao.Clear();
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}