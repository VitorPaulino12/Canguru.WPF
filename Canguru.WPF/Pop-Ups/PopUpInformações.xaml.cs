using System;
using System.Windows;

namespace Canguru.WPF.Pop_Ups
{
    public partial class PopUpInfoemações : Window
    {
        private int _identificadorNotificacao;
        private Action _acaoConfirmada;

        public PopUpInfoemações(int identificadorNotificacao, Action acaoConfirmada = null)
        {
            InitializeComponent();
            _identificadorNotificacao = identificadorNotificacao;
            _acaoConfirmada = acaoConfirmada;

            PreencherTela(_identificadorNotificacao);
        }
        public PopUpInfoemações(string titulo, string mensagem)
        {
            InitializeComponent();

            txtTituloNotificacao.Text = titulo;
            txtConteudo.Text = mensagem;

            btnVerde.Content = "OK";
            btnVerde.Visibility = Visibility.Visible;
            btnVermelho.Visibility = Visibility.Collapsed;
        }
        public PopUpInfoemações(string titulo, string mensagem, Action acaoConfirmada)
        {
            InitializeComponent();

            txtTituloNotificacao.Text = titulo;
            txtConteudo.Text = mensagem;

            btnVermelho.Content = "Não";
            btnVerde.Content = "Sim";

            btnVermelho.Visibility = Visibility.Visible;
            btnVerde.Visibility = Visibility.Visible;

            _acaoConfirmada = acaoConfirmada;
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnVermelho_Click(object sender, RoutedEventArgs e)
        {
            // NÃO
            this.Close();
        }

        private void btnVerde_Click(object sender, RoutedEventArgs e)
        {
            // SIM
            _acaoConfirmada?.Invoke();
            this.Close();
        }

        public void PreencherTela(int identificadorNotificacao)
        {
            switch (identificadorNotificacao)
            {
                // 1 = Aviso simples
                case 1:
                    txtTituloNotificacao.Text = "Aviso";
                    txtConteudo.Text = "Preencha Login e Senha para prosseguir!";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;

                // 2 = Confirmação para gerar relatório
                case 2:
                    txtTituloNotificacao.Text = "Relatório Gerado";
                    txtConteudo.Text = "Deseja abrir a pasta do arquivo gerado?";
                    btnVerde.Content = "Sim";
                    btnVermelho.Content = "Não";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Visible;
                    break;

                // 3 = Erro arquivo aberto
                case 3:
                    txtTituloNotificacao.Text = "Arquivo em uso";
                    txtConteudo.Text = "O arquivo Excel está aberto. Feche-o e tente novamente.";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;

                default:
                    txtTituloNotificacao.Text = "Aviso";
                    txtConteudo.Text = "Notificação não reconhecida.";
                    btnVerde.Content = "OK";
                    btnVerde.Visibility = Visibility.Visible;
                    btnVermelho.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
