using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Canguru.Core; // <--- Certifique-se que sua classe Usuario está aqui

namespace Canguru.WPF
{
    public partial class PerfilWindow : Window
    {
        private Usuario _usuarioLogado;

        public ObservableCollection<FeedPost> MeusPosts { get; set; }

        public PerfilWindow()
        {
            InitializeComponent();

            _usuarioLogado = new Aluno
            {
                Nome = "Usuário Teste",
                Email = "teste@canguru.com",
             
            };

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
            }

            try
            {
            
                var uri = new Uri("https://cdn-icons-png.flaticon.com/512/3135/3135715.png");
                imgPerfil.ImageSource = new BitmapImage(uri);
            }
            catch { }
        }

        private void CarregarFeed()
        {
            MeusPosts = new ObservableCollection<FeedPost>
            {
                new FeedPost { Titulo = "Atualizou o Perfil", FoiConcluido = true, DataPostagem = DateTime.Now.AddDays(-1) },
                new FeedPost { Titulo = "Interagiu com a Turma", FoiConcluido = true, DataPostagem = DateTime.Now.AddDays(-5) },
                new FeedPost { Titulo = "Realizou as Atividades", FoiConcluido = false, DataPostagem = DateTime.Now }
            };
            lstFeed.ItemsSource = MeusPosts;
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e) => Close();

        private void btnEditarSobreMim_Click(object sender, RoutedEventArgs e)
        {
            if (txtSobreMim.Visibility == Visibility.Visible)
            {
                txtEditSobreMim.Text = txtSobreMim.Text;
                txtSobreMim.Visibility = Visibility.Collapsed;
                txtEditSobreMim.Visibility = Visibility.Visible;

                btnEditarSobreMim.Content = "Salvar Alterações";
                btnEditarSobreMim.Background = Brushes.OrangeRed;
            }
            else
            {
                txtSobreMim.Text = txtEditSobreMim.Text;
                txtSobreMim.Visibility = Visibility.Visible;
                txtEditSobreMim.Visibility = Visibility.Collapsed;

                btnEditarSobreMim.Content = "Editar 'Sobre mim'";
                btnEditarSobreMim.Background = (Brush)new BrushConverter().ConvertFrom("#50E3C2");
            }
        }

        private void btnEditarInformacoes_Click(object sender, RoutedEventArgs e)
        {
            var janelaEdicao = new EditarCadastroWindow(txtNomeUsuario.Text, txtEmail.Text);

            if (janelaEdicao.ShowDialog() == true)
            {
                txtNomeUsuario.Text = janelaEdicao.NovoNome;
                txtEmail.Text = "E-mail: " + janelaEdicao.NovoEmail;

                if (_usuarioLogado != null)
                {
                    _usuarioLogado.Nome = janelaEdicao.NovoNome;
                    _usuarioLogado.Email = janelaEdicao.NovoEmail;
                }

                MessageBox.Show("Dados atualizados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}