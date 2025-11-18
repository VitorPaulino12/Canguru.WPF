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

            try
            {
                
                GerenciadorSessao.AddSessao(nome, descricao);

                MessageBox.Show("Sessão criada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar sessão: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}