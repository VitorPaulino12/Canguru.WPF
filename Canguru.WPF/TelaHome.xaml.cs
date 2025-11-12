using Canguru.Business; // PARA ENCONTRAR A CLASSE (PROFESSOR)
using Canguru.Core;
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
        //PRECISO ORGANIZAR ESSA PARTE PARA PEGAR O NOME DO USUARIO LOGADO E COLOCAR NO PAINEL DE IDENTIFICAÇÃO
        // ALEM DISOS PEGAR A IMAGEM DE PERFIL DESSE USUARIO E CARREGA-LA NO PAINEL
        private Usuario usuarioLogado;
        private string pathImagemSelecionada = string.Empty;

        public TelaHome(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            PreencherPainelUsuarioLogado(usuarioLogado);
            CarregarUsuarios();
            MostrarFeed();

        }

        //Construindo o painel onde ficam os usuarios cadastrados no sistema
        private void CarregarUsuarios()
        {
            var usuarios = GerenciadorDeUsuarios.GetTodosUsuarios();
            ListaDeUsuarios.ItemsSource = usuarios;
        }

        //
        //
        private void PreencherPainelUsuarioLogado(Usuario u)
        {
            txtNomeUsuario.Text = u.Nome;

            if (!string.IsNullOrEmpty(u.CaminhoFotoPerfil))
            {
                string caminhoCompleto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", u.CaminhoFotoPerfil);
                if (File.Exists(caminhoCompleto))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(caminhoCompleto, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    ellipseFoto.Fill = new ImageBrush(bitmap);
                }
            }
        }
        //
        //EVENTO DE AO CLICAR NO BOTÃO ELE PEGA AS COISAISAS
        //E COLOCA NO REPOSITÓRIO DE POST
        private void BtnEnviarPost_Click(object sender, RoutedEventArgs e)
        {
            string conteudo = CaixaTextoPost.Text.Trim();

            if (string.IsNullOrEmpty(conteudo) || conteudo == placeholderText)//CASO ESTIVER VAZIO 
            {
                MessageBox.Show("Digite algo antes de postar!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
       
                Post novoPost = new Post
                {
                    Autor = usuarioLogado,
                    Conteudo = conteudo,
                    Data = DateTime.Now,

                };
            // PEGA O 'NOVOPOST' E ADICIONA NO REPOSITPORIO
            GerenciadorDePosts.AdicionarPost(novoPost);

            //ESVAZIA O CONTEUDO DAS CAIXAS DE TEXTO
            CaixaTextoPost.Text = string.Empty;
            // USA A FUNÇÃO DE 'MOSTRAR FEED' E ATUALIZA O FEED COM O NOVO OBJETO 'POST'
            MostrarFeed();
        }
        private string placeholderText = "O que você está pensando?";

        private void CaixaTextoPost_GotFocus(object sender, RoutedEventArgs e)
        {
            if(CaixaTextoPost.Text == placeholderText)
            {
                CaixaTextoPost.Text = "";
                CaixaTextoPost.Foreground = Brushes.Black;
            }
        }

        private void CaixaTextoPost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CaixaTextoPost.Text))
            {
                CaixaTextoPost.Text = placeholderText;
                CaixaTextoPost.Foreground = Brushes.Gray;
            }
        }

       

        //FUNÇÃO PARA MOSTRAR O FEED (PEGA CADA OBJETO POST E COLOCA NO FEED)
        private void MostrarFeed()
        {
            PainelDePosts.Children.Clear();
            var postsParaMostrar = GerenciadorDePosts.ObterPosts();

            foreach (var post in postsParaMostrar)
            {
                bool isDoUsuarioLogado = post.Autor.Id == usuarioLogado.Id;

                // Cria o contêiner visual do post
                Border postBorder = new Border
                {
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(15),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    HorizontalAlignment = isDoUsuarioLogado ? HorizontalAlignment.Right : HorizontalAlignment.Stretch
                };

                // Layout do post: imagem + conteúdo
                Grid gridPost = new Grid();
                gridPost.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                gridPost.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Criar a elipse para a foto do autor
                Ellipse autorEllipse = new Ellipse
                {
                    Width = 50,
                    Height = 50,
                    Margin = new Thickness(0, 0, 15, 0)
                };

                // Define a imagem do autor, se existir
                if (!string.IsNullOrEmpty(post.Autor.CaminhoFotoPerfil))
                {
                    string caminhoCompleto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FotosPerfil", post.Autor.CaminhoFotoPerfil);
                    if (File.Exists(caminhoCompleto))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(caminhoCompleto, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        autorEllipse.Fill = new ImageBrush(bitmap);
                    }
                    else
                    {
                        autorEllipse.Fill = Brushes.LightGray; // se o arquivo não existir
                    }
                }
                else
                {
                    autorEllipse.Fill = Brushes.LightGray; // se o usuário não tiver foto
                }

                Grid.SetColumn(autorEllipse, 0);

                // Cria a pilha de texto (autor, conteúdo, data)
                StackPanel postStackPanel = new StackPanel();

                TextBlock autorTextBlock = new TextBlock
                {
                    Text = post.Autor.Nome,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };

                TextBlock conteudoTextBlock = new TextBlock
                {
                    Text = post.Conteudo,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0),
                    Foreground = Brushes.Black
                };

                TextBlock dataTextBlock = new TextBlock
                {
                    Text = post.Data.ToString("dd/MM/yyyy HH:mm"),
                    FontSize = 12,
                    Foreground = Brushes.Gray
                };

                postStackPanel.Children.Add(autorTextBlock);
                postStackPanel.Children.Add(conteudoTextBlock);
                postStackPanel.Children.Add(dataTextBlock);
                Grid.SetColumn(postStackPanel, 1);

                gridPost.Children.Add(autorEllipse);
                gridPost.Children.Add(postStackPanel);
                postBorder.Child = gridPost;

                PainelDePosts.Children.Add(postBorder);
            }
        }


        // DEPOIS TEM QUE ADICIONAR CADA EVENTO DE ABRIR E FECHAR TELA
        private void BtnMeuPerfil_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Meu Perfil");
        private void BtnAtividades_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Atividades");
        private void BtnCriarAtividade_Click(object sender, RoutedEventArgs e) {
            MainGerentSessao TelaSessao = new MainGerentSessao(usuarioLogado);
            this.Close();
            TelaSessao.Show();
            
        }
        private void BtnGerenciarClasse_Click(object sender, RoutedEventArgs e)
        {
            MainGerentClasse tela = new MainGerentClasse(usuarioLogado);
            this.Close();
            tela.Show();          
            
        }
        private void BtnSair_Click(object sender, RoutedEventArgs e) => this.Close();
        private void BtnNotificacoes_Click(object sender, RoutedEventArgs e)
        {
            var popup = new NotificacaoPopup(usuarioLogado);
            popup.ShowDialog();
        }


        //Tem que adicioinar o evento de checagem se existe um quiz para ser feito
        // e isso é definido pelo professor dono da sala
        //private void PanelNotifica_Click(object sender, RoutedEventArgs e) { }
    }
}