using Canguru.Business;
using Canguru.Core;
using Canguru.WPF.Pop_Ups;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            // Pega o RA salvo
            string ra = usuarioLogado.RA ?? "";

            // REGRA: Começa com '1' -> É ALUNO -> BLOQUEIA
            if (ra.StartsWith("1"))
            {
                if (BtnGerenciarClasse != null) BtnGerenciarClasse.Visibility = Visibility.Collapsed;
                if (BtnCriarAtividade != null) BtnCriarAtividade.Visibility = Visibility.Collapsed;
            }
            // REGRA: Começa com '2' -> LIBERA
            else
            {
                if (BtnGerenciarClasse != null) BtnGerenciarClasse.Visibility = Visibility.Visible;
                if (BtnCriarAtividade != null) BtnCriarAtividade.Visibility = Visibility.Visible;
            }
        }

        // --- BOTÕES E CLIQUES ---
        private void BtnCriarAtividade_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.RA.StartsWith("1")) MessageBox.Show("Acesso Negado.");
            else { MainGerentSessao t = new MainGerentSessao(usuarioLogado); Close(); t.Show(); }
        }

        private void BtnGerenciarClasse_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.RA.StartsWith("1")) MessageBox.Show("Acesso Negado.");
            else { MainGerentClasse t = new MainGerentClasse(usuarioLogado); Close(); t.Show(); }
        }

        // Métodos
        private void BtnAtividades_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Atividades");
        private void BtnMeuPerfil_Click(object sender, RoutedEventArgs e) { PerfilWindow p = new PerfilWindow(usuarioLogado); p.Show(); }
        private void BtnNotificacoes_Click(object sender, RoutedEventArgs e) { var p = new NotificacoesWindow(usuarioLogado); p.ShowDialog(); }
        private void BtnSair_Click(object sender, RoutedEventArgs e) { LoginWindow l = new LoginWindow(); l.Show(); this.Close(); }

        // Feed
        private void BtnEnviarPost_Click(object sender, RoutedEventArgs e)
        {
            string c = CaixaTextoPost.Text.Trim();
            if (string.IsNullOrEmpty(c) || c == placeholderText) { new PopUpsGerais(16).ShowDialog(); return; }
            GerenciadorDePosts.AdicionarPost(new Post { Autor = usuarioLogado, Conteudo = c, Data = DateTime.Now });
            CaixaTextoPost.Text = ""; MostrarFeed();
        }
        private void MostrarFeed()
        {
            PainelDePosts.Children.Clear();
            foreach (var post in GerenciadorDePosts.ObterPosts())
            {
                bool isUser = post.Autor.Id == usuarioLogado.Id;
                Border b = new Border { Background = Brushes.White, CornerRadius = new CornerRadius(15), Padding = new Thickness(20), Margin = new Thickness(0, 0, 0, 15), HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Stretch };
                StackPanel sp = new StackPanel();
                sp.Children.Add(new TextBlock { Text = post.Autor.Nome, FontWeight = FontWeights.Bold });
                sp.Children.Add(new TextBlock { Text = post.Conteudo, TextWrapping = TextWrapping.Wrap });
                b.Child = sp; PainelDePosts.Children.Add(b);
            }
        }
        private void CaixaTextoPost_GotFocus(object sender, RoutedEventArgs e) { if (CaixaTextoPost.Text == placeholderText) { CaixaTextoPost.Text = ""; CaixaTextoPost.Foreground = Brushes.Black; } }
        private void CaixaTextoPost_LostFocus(object sender, RoutedEventArgs e) { if (string.IsNullOrEmpty(CaixaTextoPost.Text)) { CaixaTextoPost.Text = placeholderText; CaixaTextoPost.Foreground = Brushes.Gray; } }
        private void CarregarUsuarios() { ListaDeUsuarios.ItemsSource = GerenciadorDeUsuarios.GetTodosUsuarios(); }
        private void PreencherPainelUsuarioLogado(Usuario u) { txtNomeUsuario.Text = u.Nome; try { if (!string.IsNullOrEmpty(u.CaminhoFotoPerfil) && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", u.CaminhoFotoPerfil))) ellipseFoto.Fill = new ImageBrush(new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", u.CaminhoFotoPerfil)))); } catch { } }
    }
}