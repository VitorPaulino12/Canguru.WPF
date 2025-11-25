using Canguru.Business;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Canguru.WPF
{
    public partial class NotificacoesWindow : Window
    {
        private Usuario usuarioLogado;

        public NotificacoesWindow(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            foreach (var item in GerenciadorNotificacoes.Notificacoes)
                CriarNotificacao(item.Texto, item.Acao);
            GerenciadorNotificacoes.NovaNotificacao += CriarNotificacao;
        }
        private void CriarNotificacao(string texto, Action acao)
        {
            Border border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 10, 0, 0)
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

            TextBlock txt = new TextBlock
            {
                Text = texto,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(26, 26, 46))
            };

            Button btn = new Button
            {
                Content = "Abrir",
                Width = 150,
                Height = 50,
                Background = new SolidColorBrush(Color.FromRgb(124, 198, 141)),
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };
            btn.Click += (s, e) =>
            {
                acao?.Invoke();
                this.Close();
            };

            grid.Children.Add(txt);
            Grid.SetColumn(btn, 1);
            grid.Children.Add(btn);

            border.Child = grid;
            stkNotificacoes.Children.Add(border);
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
