using Canguru.Business;
using Canguru.Core;
using QuizTeste;
using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Canguru.WPF
{
    public partial class MainGerentSessao : Window
    {
        private Usuario usuarioLogado;
        private int SessaoSelecionadaId;
        private List<Pergunta> _perguntasUsuario = new List<Pergunta>();
        private int PerguntaSelecionadaId = 0;
        public MainGerentSessao(Usuario usuario)
        {
            InitializeComponent();
            CarregarSessoes();
            usuarioLogado = usuario;
            SessaoSelecionadaId = 0;
            _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
            lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }

        public void CarregarSessoes()
        {
            var sessoes = GerenciadorSessao.GetSessoes();

            if (sessoes != null && sessoes.Count > 0)
            {
                ListaDeSessoes.ItemsSource = sessoes;

                
                System.Diagnostics.Debug.WriteLine($"Sessões carregadas: {sessoes.Count}");
                foreach (var sessao in sessoes)
                {
                    System.Diagnostics.Debug.WriteLine($"Sessão: {sessao.NomeSessao} (ID: {sessao.Id})");
                }
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
                CarregarPerguntasDaSessao(SessaoSelecionadaId);

                
                lblSessaoSelecionada.Text = $"Sessão selecionada: {sessaoSelecionada.NomeSessao} (ID: {sessaoSelecionada.Id})";

                MessageBox.Show($"Sessão selecionada: {sessaoSelecionada.NomeSessao} (ID: {sessaoSelecionada.Id})",
                    "Sessão Selecionada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void CarregarPerguntasDaSessao(int idSessao)
        {
            
            var perguntas = _perguntasUsuario.Where(p => p.IdSessao == idSessao).ToList();
            ListaPerguntas.Children.Clear();

            if (perguntas.Count == 0)
            {
                
                var textBlock = new TextBlock
                {
                    Text = "Nenhuma pergunta cadastrada para esta sessão.",
                    FontStyle = FontStyles.Italic,
                    Foreground = new SolidColorBrush(Colors.Gray),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                ListaPerguntas.Children.Add(textBlock);
                return;
            }

            foreach (var pergunta in perguntas)
            {
                var border = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.LightGray),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(0, 0, 0, 5),
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush(Colors.WhiteSmoke),
                    Tag = pergunta.Id
                };
                border.MouseLeftButtonDown += PerguntaSelecionada_Click;
                var stackPanel = new StackPanel();
                var enunciadoText = new TextBlock
                {
                    Text = pergunta.Enunciado,
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                };
                var alternativasText = new TextBlock
                {
                    Text = $"Alternativas: {string.Join(" | ", pergunta.Alternativas)}",
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Colors.Gray)
                };
                var respostaText = new TextBlock
                {
                    Text = $"Resposta Correta: {pergunta.Alternativas[pergunta.IdRespostaCorreta]}",
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Colors.Green),
                    FontWeight = FontWeights.SemiBold
                };

                stackPanel.Children.Add(enunciadoText);
                stackPanel.Children.Add(alternativasText);
                stackPanel.Children.Add(respostaText);
                border.Child = stackPanel;
                ListaPerguntas.Children.Add(border);
            }
        }
        private void PerguntaSelecionada_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is int idPergunta)
            {
                PerguntaSelecionadaId = idPergunta;
                //trocar por um popUp personalizado depois
                MessageBox.Show($"Pergunta selecionada com ID: {PerguntaSelecionadaId}","Pergunta Selecionada", MessageBoxButton.OK, MessageBoxImage.Information);

                var perguntaSelecionada = GerenciadorPerguntas.GetPerguntaPorId(PerguntaSelecionadaId);

                if (perguntaSelecionada != null)
                {
                    // 📝 Preenche os campos do painel com os dados da pergunta
                    txtEnunciado.Text = perguntaSelecionada.Enunciado;
                    txtAlternativa1.Text = perguntaSelecionada.Alternativas.Length > 0 ? perguntaSelecionada.Alternativas[0] : "";
                    txtAlternativa2.Text = perguntaSelecionada.Alternativas.Length > 1 ? perguntaSelecionada.Alternativas[1] : "";
                    txtAlternativa3.Text = perguntaSelecionada.Alternativas.Length > 2 ? perguntaSelecionada.Alternativas[2] : "";
                    txtAlternativa4.Text = perguntaSelecionada.Alternativas.Length > 3 ? perguntaSelecionada.Alternativas[3] : "";
                    txtAlternativaCorreta.Text = perguntaSelecionada.IdRespostaCorreta.ToString();
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SalvarPergunta();
        }

        private void SalvarPergunta()
        {
            if (SessaoSelecionadaId <= 0)
            {
                MessageBox.Show($"Por favor, selecione uma sessão antes de criar uma pergunta. (SessaoSelecionadaId: {SessaoSelecionadaId})",
                    "Sessão Não Selecionada", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string enunciado = txtEnunciado.Text.Trim();
                string[] alternativas = new string[]
                {
                    txtAlternativa1.Text.Trim(),
                    txtAlternativa2.Text.Trim(),
                    txtAlternativa3.Text.Trim(),
                    txtAlternativa4.Text.Trim()
                };

                if (!int.TryParse(txtAlternativaCorreta.Text.Trim(), out int idRespostaCorreta))
                {
                    MessageBox.Show("Por favor, insira um número válido para a alternativa correta (0-3).",
                        "Alternativa Correta Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(enunciado))
                {
                    MessageBox.Show("Por favor, insira o enunciado da pergunta.",
                        "Enunciado Vazio", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (alternativas.Any(string.IsNullOrWhiteSpace))
                {
                    MessageBox.Show("Por favor, preencha todas as alternativas.",
                        "Alternativas Incompletas", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (idRespostaCorreta < 0 || idRespostaCorreta > 3)
                {
                    MessageBox.Show("A alternativa correta deve ser um número entre 0 e 3.",
                        "Alternativa Correta Fora do Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                //int novaPerguntaId = AdicionarPerguntaUsuario(SessaoSelecionadaId, enunciado, alternativas, idRespostaCorreta);
                GerenciadorPerguntas.AdicionarPergunta(SessaoSelecionadaId, enunciado, alternativas, idRespostaCorreta);
                MessageBox.Show($"Pergunta salva com sucesso!", // eu retirei o texto que mencionava o id da pergunta que acabou de ser criada, por favor coloque aki de novo :D esqueci como faz!
                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                LimparCamposPergunta();
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar pergunta: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*eu retirei essa parte porque dessa forma as perguntas aparentemente não são de id exclusivo
            da forma com que eu fiz o metodo 'AdicionarPergunta' ele eprmite na teoria deixar a pergunta com id exclusivo
        private int AdicionarPerguntaUsuario(int idSessao, string enunciado, string[] alternativas, int idRespostaCorreta)
        {
            var pergunta = new Pergunta
            {
                Id = _proximoIdPergunta,
                IdSessao = idSessao,
                Enunciado = enunciado,
                Alternativas = alternativas,
                IdRespostaCorreta = idRespostaCorreta
            };

            _perguntasUsuario.Add(pergunta);
            _proximoIdPergunta++;
            return pergunta.Id;
        }
        */

        private List<Pergunta> ObterPerguntasUsuarioPorSessao(int idSessao)
        {
            return _perguntasUsuario.Where(p => p.IdSessao == idSessao).ToList();
        }

        
        private bool RemoverPerguntaUsuario(int id)
        {
            var pergunta = _perguntasUsuario.FirstOrDefault(p => p.Id == id);
            if (pergunta != null)
            {
                return _perguntasUsuario.Remove(pergunta);
            }
            return false;
        }

        private void LimparCamposPergunta()
        {
            txtEnunciado.Clear();
            txtAlternativa1.Clear();
            txtAlternativa2.Clear();
            txtAlternativa3.Clear();
            txtAlternativa4.Clear();
            txtAlternativaCorreta.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Funcionalidade de excluir sessão será implementada em breve.",
                "Funcionalidade Futura", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            LimparCamposPergunta();
            MessageBox.Show("Campos limpos. Preencha os dados para uma nova pergunta.",
                "Campos Limpos", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CriacaoSessao abrirTela = new CriacaoSessao(usuarioLogado);
            abrirTela.ShowDialog();
            CarregarSessoes();
        }
        private void btnAtualziar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnAttSessao_Click(object sender, RoutedEventArgs e)
        {
            AtualizarSessao novatela = new AtualizarSessao(SessaoSelecionadaId);
            novatela.ShowDialog();
        }

        private void btnAtualizarPergunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PerguntaSelecionadaId <= 0)
                {
                    MessageBox.Show("Selecione uma pergunta antes de atualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string enunciado = txtEnunciado.Text.Trim();
                string[] alternativas = new string[] { txtAlternativa1.Text.Trim(), txtAlternativa2.Text.Trim(), txtAlternativa3.Text.Trim(), txtAlternativa4.Text.Trim() };
                if (!int.TryParse(txtAlternativaCorreta.Text.Trim(), out int idRespostaCorreta))
                {
                    MessageBox.Show("Digite um número válido para a alternativa correta (0–3).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                GerenciadorPerguntas.AtualizarPergunta(PerguntaSelecionadaId, enunciado, alternativas, idRespostaCorreta);
                MessageBox.Show($"Pergunta ID {PerguntaSelecionadaId} atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar pergunta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}