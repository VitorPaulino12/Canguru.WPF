using Canguru.Business; // PARA ENCONTRAR A CLASSE (PROFESSOR)
using Canguru.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Canguru.WPF
{
    public partial class TelaHome : Window
    {
        //PRECISO ORGANIZAR ESSA PARTE PARA PEGAR O NOME DO USUARIO LOGADO E COLOCAR NO PAINEL DE IDENTIFICAÇÃO
        // ALEM DISOS PEGAR A IMAGEM DE PERFIL DESSE USUARIO E CARREGA-LA NO PAINEL
        private Professor usuarioLogado = new Professor { Nome = "Nome Usuário :D", Id = 1 };
        private string pathImagemSelecionada = string.Empty;

        public TelaHome()
        {
            InitializeComponent();
            MostrarFeed(); //  CARREGA O FEED E OQ TA DENTRO DO REPOSITÓRIO
        }
        //EVENTO DE AO CLICAR NO BOTÃO ELE PEGA AS COISAISAS
        //E COLOCA NO REPOSITÓRIO DE POST
        private void BtnEnviarPost_Click(object sender, RoutedEventArgs e)
        {
            string conteudo = CaixaTextoPost.Text.Trim();

            if (string.IsNullOrEmpty(conteudo))//CASO ESTIVER VAZIO 
            {
                MessageBox.Show("Digite algo antes de postar!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //PEGANDO O CAMINHO DA IMAGEM PARA CARREGAR DEPOIS NO POST
            string imagemPost = pathImagemSelecionada;
            if (string.IsNullOrEmpty(imagemPost) || !File.Exists(imagemPost))
            {
                imagemPost = "Resources/user_Postdefault.png";
            }

            Post novoPost = new Post
            {
                Autor = usuarioLogado,
                Conteudo = conteudo,
                Data = DateTime.Now,
                Imagem = imagemPost
            };
            // PEGA O 'NOVOPOST' E ADICIONA NO REPOSITPORIO
            GerenciadorDePosts.AdicionarPost(novoPost);

            //ESVAZIA O CONTEUDO DAS CAIXAS DE TEXTO
            CaixaTextoPost.Text = string.Empty;
            pathImagemSelecionada = string.Empty;
            // USA A FUNÇÃO DE 'MOSTRAR FEED' E ATUALIZA O FEED COM O NOVO OBJETO 'POST'
            MostrarFeed();
        }
        //FUNÇÃO PARA MOSTRAR O FEED (PEGA CADA OBJETO POST E COLOCA NO FEED)
        private void MostrarFeed()
        {
            PainelDePosts.Children.Clear();
            var postsParaMostrar = GerenciadorDePosts.ObterPosts();
            
            // AS POSTAGENS
            // PARA CADA OBJETO NA MEMORIA 'POST' ELE SEGUE UM PADRÃO
            foreach (var post in postsParaMostrar)
            {
                bool isDoUsuarioLogado = post.Autor.Id == usuarioLogado.Id;

                //PEGA OS DADOS DO 'POST' E COLOCA DE UMA FORMA VISUAL NO PAINEL FEED
                //SEQUENCIA DE PREPARAÇÃO PARA A CRIAÇÃO DO POST
                Border postBorder = new Border
                {
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(15),
                    Padding = new Thickness(20),
                    Margin = new Thickness(0, 0, 0, 15),
                    HorizontalAlignment = isDoUsuarioLogado ? HorizontalAlignment.Right : HorizontalAlignment.Stretch
                };

                Grid gridPost = new Grid();
                gridPost.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                gridPost.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Ellipse autorEllipse = new Ellipse { Width = 50, Height = 50, Fill = Brushes.LightGray, Margin = new Thickness(0, 0, 15, 0) };
                Grid.SetColumn(autorEllipse, 0);

                StackPanel postStackPanel = new StackPanel();
                TextBlock autorTextBlock = new TextBlock { Text = post.Autor.Nome, FontWeight = FontWeights.Bold };

               
                TextBlock conteudoTextBlock = new TextBlock
                {
                    Text = post.Conteudo,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                TextBlock dataTextBlock = new TextBlock { Text = post.Data.ToString("dd/MM/yyyy HH:mm"), FontSize = 12, Foreground = Brushes.Gray };

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
        private void BtnCriarAtividade_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Criar Atividade");
        private void BtnGerenciarClasse_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Abrir Gerenciar Classe");
        private void BtnSair_Click(object sender, RoutedEventArgs e) => this.Close();
        private void BtnNotificacoes_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Você tem 3 novas notificações!");
    }
}