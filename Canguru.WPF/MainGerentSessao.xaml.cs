using Canguru.Business;
using Canguru.Core;
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
        private int _proximoIdPergunta = 1000; 

        public MainGerentSessao(Usuario usuario)
        {
            InitializeComponent();
            CarregarSessoes();
            usuarioLogado = usuario;
            SessaoSelecionadaId = 0;

            lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }

        private void CarregarSessoes()
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
                    Background = new SolidColorBrush(Colors.WhiteSmoke)
                };

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

                
                int novaPerguntaId = AdicionarPerguntaUsuario(SessaoSelecionadaId, enunciado, alternativas, idRespostaCorreta);

                MessageBox.Show($"Pergunta salva com sucesso! ID: {novaPerguntaId}",
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
    }
}