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
    /// Interaction logic for MainGerentSessao.xaml
    /// </summary>
    public partial class MainGerentSessao : Window
    {
        private Usuario usuarioLogado;
        private int SessaoSelecionadaId;

        public MainGerentSessao(Usuario usuario)
        {
            InitializeComponent();
            CarregarSessoes();
            usuarioLogado = usuario;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }
        private void CarregarSessoes()
        {
            var sessoes = GerenciadorSessao.GetSessoes();

            if (sessoes != null && sessoes.Count > 0)
            {
                ListaDeSessoes.ItemsSource = sessoes;
            }
            else
            {
                MessageBox.Show("Nenhuma sessão cadastrada ainda.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ListaDeSessoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaDeSessoes.SelectedItem is Sessao sessaoSelecionada)
            {
                SessaoSelecionadaId = sessaoSelecionada.Id;

                MessageBox.Show($"Sessão selecionada: {sessaoSelecionada.NomeSessao} (ID: {sessaoSelecionada.Id})","Sessão Selecionada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //SALVAR
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            /*NÃO TA FUNCIONANDO :D
             Sessao sessao = _norteSessao.FirstOrDefault(s => s.Id == id);
            if (sessao != null)
            {
                _norteSessao.Remove(sessao);

                // TODO: também remover as perguntas associadas a essa sessão
                // GerenciadorPergunta.ExcluirPerguntasPorSessao(id);

                return true;
            }
            return false;
            */
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //ATUALIZAR
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CriacaoSessao abrirTela = new CriacaoSessao(usuarioLogado);

            // Abre a tela como modal (bloqueia até ser fechada)
            abrirTela.ShowDialog();

            // Recarrega as sessões após a criação
            CarregarSessoes();

        }
    }
}
