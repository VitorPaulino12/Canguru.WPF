using Canguru.Business;
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
    /// Interaction logic for CriacaoSessao.xaml
    /// </summary>
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

           
            GerenciadorSessao.AddSessao(nome, descricao);
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