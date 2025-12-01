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
            _usuarioLogado = new Aluno { Id = 999, Nome = "Teste", Email = "teste@canguru.com" }; 
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

                txtRA.Text = "RA: " + _usuarioLogado.RA;

                // LÓGICA DE EXIBIÇÃO:
                if (_usuarioLogado is Aluno)
                {
                    txtTipoUsuario.Text = "Tipo usuário: Aluno";
                }
                else if (_usuarioLogado is Adm)
                {
                    txtTipoUsuario.Text = "Tipo usuário: Administrador";
                }
                else
                {
                    txtTipoUsuario.Text = "Tipo usuário: Professor";
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(_usuarioLogado.CaminhoFotoPerfil))
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", _usuarioLogado.CaminhoFotoPerfil);
                    if (File.Exists(path)) imgPerfil.ImageSource = new BitmapImage(new Uri(path));
                    else imgPerfil.ImageSource = new BitmapImage(new Uri("https://cdn-icons-png.flaticon.com/512/3135/3135715.png"));
                }
                else imgPerfil.ImageSource = new BitmapImage(new Uri("https://cdn-icons-png.flaticon.com/512/3135/3135715.png"));
            }
            catch { }
        }

        private void CarregarFeed()
        {
            MeusPosts = new ObservableCollection<FeedPost>();
            var posts = GerenciadorDePosts.ObterPosts().Where(p => p.Autor.Id == _usuarioLogado.Id).OrderByDescending(p => p.Data);
            foreach (var post in posts) MeusPosts.Add(new FeedPost { Titulo = post.Conteudo, FoiConcluido = true, DataPostagem = post.Data });
            lstFeed.ItemsSource = MeusPosts;
        }
        private void BtnVoltar_Click(object sender, RoutedEventArgs e) => Close();
        private void btnEditarSobreMim_Click(object sender, RoutedEventArgs e) { if (txtSobreMim.Visibility == Visibility.Visible) { txtEditSobreMim.Text = txtSobreMim.Text; txtSobreMim.Visibility = Visibility.Collapsed; txtEditSobreMim.Visibility = Visibility.Visible; btnEditarSobreMim.Content = "Salvar Alterações"; btnEditarSobreMim.Background = Brushes.OrangeRed; } else { txtSobreMim.Text = txtEditSobreMim.Text; txtSobreMim.Visibility = Visibility.Visible; txtEditSobreMim.Visibility = Visibility.Collapsed; btnEditarSobreMim.Content = "Editar 'Sobre mim'"; btnEditarSobreMim.Background = (Brush)new BrushConverter().ConvertFrom("#50E3C2"); } }
        private void btnEditarInformacoes_Click(object sender, RoutedEventArgs e) { var w = new EditarCadastroWindow(txtNomeUsuario.Text, txtEmail.Text); if (w.ShowDialog() == true) { txtNomeUsuario.Text = w.NovoNome; txtEmail.Text = "E-mail: " + w.NovoEmail; if (_usuarioLogado != null) { _usuarioLogado.Nome = w.NovoNome; _usuarioLogado.Email = w.NovoEmail; } MessageBox.Show("Dados atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information); } }
    }
}