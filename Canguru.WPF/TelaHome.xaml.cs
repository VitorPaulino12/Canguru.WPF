using Canguru.Business;
using Canguru.Core;
using Canguru.WPF.Pop_Ups;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Canguru.WPF
{
    public partial class TelaHome : Window
    {
        private Usuario usuarioLogado;
        private string placeholderText = "O que você está pensando?";

        public TelaHome(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;

            PreencherPainelUsuarioLogado(usuarioLogado);
            CarregarUsuarios();
            MostrarFeed();

            ConfigurarPermissoes();
        }

        private void ConfigurarPermissoes()
        {

            if (usuarioLogado is Aluno)
            {
                if (BtnGerenciarClasse != null) BtnGerenciarClasse.Visibility = Visibility.Collapsed;
                if (BtnCriarAtividade != null) BtnCriarAtividade.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (BtnGerenciarClasse != null) BtnGerenciarClasse.Visibility = Visibility.Visible;
                if (BtnCriarAtividade != null) BtnCriarAtividade.Visibility = Visibility.Visible;
            }
        }


        private void BtnCriarAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado is Aluno)
            {
                MessageBox.Show("Acesso Negado: Alunos não podem criar atividades.");
            }
            else
            {
                MainGerentSessao TelaSessao = new MainGerentSessao(usuarioLogado);
                this.Close();
                TelaSessao.Show();
            }
        }

        private void BtnGerenciarClasse_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado is Aluno)
            {
                MessageBox.Show("Acesso Negado: Alunos não podem gerenciar a classe.");
            }
            else
            {
                MainGerentClasse tela = new MainGerentClasse(usuarioLogado);
                this.Close();
                tela.Show();
            }
        }

        private void BtnAtividades_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Atividades");
        private void BtnMeuPerfil_Click(object sender, RoutedEventArgs e) { PerfilWindow p = new PerfilWindow(usuarioLogado); p.Show(); }
        private void BtnNotificacoes_Click(object sender, RoutedEventArgs e) { var p = new NotificacoesWindow(usuarioLogado); p.ShowDialog(); }
        private void BtnSair_Click(object sender, RoutedEventArgs e) => this.Close();

        private void BtnEnviarPost_Click(object sender, RoutedEventArgs e)
        {
            string conteudo = CaixaTextoPost.Text.Trim();
            if (string.IsNullOrEmpty(conteudo) || conteudo == placeholderText) { var p = new PopUpsGerais(16); p.ShowDialog(); return; }
            Post novoPost = new Post { Autor = usuarioLogado, Conteudo = conteudo, Data = DateTime.Now };
            GerenciadorDePosts.AdicionarPost(novoPost);
            CaixaTextoPost.Text = string.Empty;
            MostrarFeed();
        }

        private void MostrarFeed()
        {
            PainelDePosts.Children.Clear();
            var posts = GerenciadorDePosts.ObterPosts();
            foreach (var post in posts)
            {
                bool isUser = post.Autor.Id == usuarioLogado.Id;
                Border b = new Border { Background = Brushes.White, CornerRadius = new CornerRadius(15), Padding = new Thickness(20), Margin = new Thickness(0, 0, 0, 15), HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Stretch };
                Grid g = new Grid(); g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Ellipse el = new Ellipse { Width = 50, Height = 50, Margin = new Thickness(0, 0, 15, 0) };
                if (!string.IsNullOrEmpty(post.Autor.CaminhoFotoPerfil) && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", post.Autor.CaminhoFotoPerfil)))
                    el.Fill = new ImageBrush(new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", post.Autor.CaminhoFotoPerfil))));
                else el.Fill = Brushes.LightGray;
                StackPanel sp = new StackPanel();
                sp.Children.Add(new TextBlock { Text = post.Autor.Nome, FontWeight = FontWeights.Bold });
                sp.Children.Add(new TextBlock { Text = post.Conteudo, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 5, 0, 0) });
                sp.Children.Add(new TextBlock { Text = post.Data.ToString("dd/MM HH:mm"), FontSize = 12, Foreground = Brushes.Gray });
                Grid.SetColumn(el, 0); Grid.SetColumn(sp, 1); g.Children.Add(el); g.Children.Add(sp); b.Child = g; PainelDePosts.Children.Add(b);
            }
        }

        private void CaixaTextoPost_GotFocus(object sender, RoutedEventArgs e) { if (CaixaTextoPost.Text == placeholderText) { CaixaTextoPost.Text = ""; CaixaTextoPost.Foreground = Brushes.Black; } }
        private void CaixaTextoPost_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrEmpty(CaixaTextoPost.Text)) { CaixaTextoPost.Text = placeholderText; CaixaTextoPost.Foreground = Brushes.Gray; } }
        private void CarregarUsuarios() { ListaDeUsuarios.ItemsSource = GerenciadorDeUsuarios.GetTodosUsuarios(); }
        private void PreencherPainelUsuarioLogado(Usuario u) { txtNomeUsuario.Text = u.Nome; try { if (!string.IsNullOrEmpty(u.CaminhoFotoPerfil)) { string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", u.CaminhoFotoPerfil); if (File.Exists(p)) ellipseFoto.Fill = new ImageBrush(new BitmapImage(new Uri(p))); } } catch { } }
    }
}