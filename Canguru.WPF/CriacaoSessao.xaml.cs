using Canguru.Business;
using Canguru.Core;
using Canguru.WPF.Pop_Ups;
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
                //MessageBox.Show("Por favor, insira o nome da sessão.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                var PopUp = new PopUpsGerais(35);
                PopUp.ShowDialog();
                return;
            }

            try
            {
                
                GerenciadorSessao.AddSessao(nome, descricao);

                //MessageBox.Show("Sessão criada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                var PopUp = new PopUpsGerais(36);
                PopUp.ShowDialog();
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                var PopUp = new PopUpsGerais(37);
                PopUp.ShowDialog();
                //MessageBox.Show($"Erro ao criar sessão: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}