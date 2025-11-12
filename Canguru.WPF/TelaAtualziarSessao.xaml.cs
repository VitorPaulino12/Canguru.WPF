using Canguru.Business;
using Canguru.Core;
using QuizTeste;
using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Canguru.WPF
{
    /// <summary>
    /// Interaction logic for TelaAtualziarSessao.xaml
    /// </summary>
    public partial class AtualizarSessao : Window
    {
        private int sessaoSelecionadaId;
        public AtualizarSessao(int idSessao)
        {
            InitializeComponent();
            sessaoSelecionadaId = idSessao;
            var sessao = GerenciadorSessao.GetSessoes().FirstOrDefault(s => s.Id == idSessao);
            if (sessao != null)
            {
                txtNomeSessao.Text = sessao.NomeSessao;
                txtDescricaoSessao.Text = sessao.descricaoSessao;
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            GerenciadorSessao.AtualizarSessao(sessaoSelecionadaId, txtNomeSessao.Text, txtDescricaoSessao.Text);
            MessageBox.Show("Sessão atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
