using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using Canguru.Core;
using Canguru.Business;
using System.IO;

namespace Canguru.WPF
{
    public partial class PerfilWindow : Window
    {
        private Usuario _usuarioLogado;
        public ObservableCollection<FeedPost> MeusPosts { get; set; }

        public PerfilWindow()
        {
            InitializeComponent();
            _usuarioLogado = new Aluno { Id = 1, Nome = "Teste", Email = "teste@canguru.com", RA = "11111111111" };
            ConfigurarJanela();
        }

        public PerfilWindow(Usuario usuario)
        {
            InitializeComponent();
            this._usuarioLogado = usuario;
            ConfigurarJanela();
        }

        private void ConfigurarJanela()
        {
            btnVoltar.Click += BtnVoltar_Click;
            CarregarDadosDoUsuario();
            CarregarFeed();
        }

        private void CarregarDadosDoUsuario()
        {
            if (_usuarioLogado != null)
            {
                txtNomeUsuario.Text = _usuarioLogado.Nome;
                txtEmail.Text = "E-mail: " + _usuarioLogado.Email;

                // Exibe o RA
                string ra = _usuarioLogado.RA ?? "";
                txtRA.Text = "RA: " + ra;

                if (ra.StartsWith("1"))
                {
                    txtTipoUsuario.Text = "Tipo usuário: Aluno";
                }
                else
                {
                    // Admin (RA 2000...) ou Professor (RA 2...) caem aqui
                    txtTipoUsuario.Text = "Tipo usuário: Professor";
                }
            }

            try { if (!string.IsNullOrEmpty(_usuarioLogado.CaminhoFotoPerfil)) { string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", _usuarioLogado.CaminhoFotoPerfil); if (File.Exists(p)) imgPerfil.ImageSource = new BitmapImage(new Uri(p)); else imgPerfil.ImageSource = new BitmapImage(new Uri("https://cdn-icons-png.flaticon.com/512/3135/3135715.png")); } else imgPerfil.ImageSource = new BitmapImage(new Uri("https://cdn-icons-png.flaticon.com/512/3135/3135715.png")); } catch { }
        }

        private void CarregarFeed() { MeusPosts = new ObservableCollection<FeedPost>(); var posts = GerenciadorDePosts.ObterPosts().Where(p => p.Autor.Id == _usuarioLogado.Id).OrderByDescending(p => p.Data); foreach (var p in posts) MeusPosts.Add(new FeedPost { Titulo = p.Conteudo, FoiConcluido = true, DataPostagem = p.Data }); lstFeed.ItemsSource = MeusPosts; }
        private void BtnVoltar_Click(object sender, RoutedEventArgs e) => Close();
        private void btnEditarSobreMim_Click(object sender, RoutedEventArgs e) { if (txtSobreMim.Visibility == Visibility.Visible) { txtEditSobreMim.Text = txtSobreMim.Text; txtSobreMim.Visibility = Visibility.Collapsed; txtEditSobreMim.Visibility = Visibility.Visible; btnEditarSobreMim.Content = "Salvar"; btnEditarSobreMim.Background = Brushes.OrangeRed; } else { txtSobreMim.Text = txtEditSobreMim.Text; txtSobreMim.Visibility = Visibility.Visible; txtEditSobreMim.Visibility = Visibility.Collapsed; btnEditarSobreMim.Content = "Editar"; btnEditarSobreMim.Background = (Brush)new BrushConverter().ConvertFrom("#50E3C2"); } }
        private void btnEditarInformacoes_Click(object sender, RoutedEventArgs e) { var w = new EditarCadastroWindow(txtNomeUsuario.Text, txtEmail.Text); if (w.ShowDialog() == true) { txtNomeUsuario.Text = w.NovoNome; txtEmail.Text = "E-mail: " + w.NovoEmail; if (_usuarioLogado != null) { _usuarioLogado.Nome = w.NovoNome; _usuarioLogado.Email = w.NovoEmail; } MessageBox.Show("Atualizado!"); } }
    }
}