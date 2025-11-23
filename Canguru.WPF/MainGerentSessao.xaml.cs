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
        public int _contadorTeste = 0;
        
        private Usuario usuarioLogado;
        private int SessaoSelecionadaId;
        private int PerguntaSelecionadaId;
        private List<Pergunta> _perguntasUsuario = new List<Pergunta>();
        private Pergunta _perguntaSelecionada;

        public MainGerentSessao(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
            SessaoSelecionadaId = 0;
            _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
            lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
            btnExcluirPergunta.IsEnabled = false;

            ListaDeSessoes.ItemsSource = GerenciadorSessao.GetSessoes();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }
        /*
        public void CarregarSessoes()
        {
            var sessoes = GerenciadorSessao.GetSessoes();

            if (sessoes != null && sessoes.Count > 0)
            {
                ListaDeSessoes.ItemsSource = sessoes;
            }
            else
            {
                MessageBox.Show("Nenhuma sessão cadastrada ainda.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }*/

        private void ListaDeSessoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaDeSessoes.SelectedItem is Sessao sessaoSelecionada)
            {
                SessaoSelecionadaId = sessaoSelecionada.Id;
                _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas(); // Atualiza a lista de perguntas
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
                lblSessaoSelecionada.Text = $"Sessão selecionada: {sessaoSelecionada.NomeSessao} (ID: {sessaoSelecionada.Id})";
            }
        }

        private void CarregarPerguntasDaSessao(int idSessao)
        {
            var perguntas = _perguntasUsuario.Where(p => p.IdSessao == idSessao).ToList();
            ListaPerguntas.Children.Clear();
            _perguntaSelecionada = null;
            btnExcluirPergunta.IsEnabled = false;

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
                var idText = new TextBlock
                {
                    Text = $"ID Pergunta: {pergunta.Id}",
                    FontSize = 9,
                    Foreground = new SolidColorBrush(Colors.DarkBlue),
                    FontWeight = FontWeights.Bold
                };

                stackPanel.Children.Add(enunciadoText);
                stackPanel.Children.Add(alternativasText);
                stackPanel.Children.Add(respostaText);
                stackPanel.Children.Add(idText);
                border.Child = stackPanel;
                ListaPerguntas.Children.Add(border);
            }
        }
        private void PerguntaSelecionada_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is int idPergunta)
            {
                PerguntaSelecionadaId = idPergunta;

                
                foreach (var child in ListaPerguntas.Children)
                {
                    if (child is Border b)
                        b.Background = new SolidColorBrush(Colors.WhiteSmoke);
                }

                
                border.Background = new SolidColorBrush(Color.FromRgb(255, 230, 200));

               
                var perguntaEncontrada = GerenciadorPerguntas.GetPerguntaPorId(PerguntaSelecionadaId);

                if (perguntaEncontrada != null)
                {
                    
                    _perguntaSelecionada = perguntaEncontrada;
                    txtEnunciado.Text = _perguntaSelecionada.Enunciado;
                    txtAlternativa1.Text = _perguntaSelecionada.Alternativas.Length > 0 ? _perguntaSelecionada.Alternativas[0] : "";
                    txtAlternativa2.Text = _perguntaSelecionada.Alternativas.Length > 1 ? _perguntaSelecionada.Alternativas[1] : "";
                    txtAlternativa3.Text = _perguntaSelecionada.Alternativas.Length > 2 ? _perguntaSelecionada.Alternativas[2] : "";
                    txtAlternativa4.Text = _perguntaSelecionada.Alternativas.Length > 3 ? _perguntaSelecionada.Alternativas[3] : "";
                    txtAlternativaCorreta.Text = _perguntaSelecionada.IdRespostaCorreta.ToString();

                    // Habilita os botões de ação
                    btnExcluirPergunta.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show($"Erro: não foi possível localizar a pergunta de ID {PerguntaSelecionadaId}.",
                        "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Botão Salvar Pergunta
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

                int novaPerguntaId = GerenciadorPerguntas.AdicionarPergunta(SessaoSelecionadaId, enunciado, alternativas, idRespostaCorreta);
                MessageBox.Show($"Pergunta salva com sucesso! (ID: {novaPerguntaId})",
                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                LimparCamposPergunta();
                _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar pergunta: {ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnExcluirPergunta_Click(object sender, RoutedEventArgs e)
        {
            if (_perguntaSelecionada == null)
            {
                MessageBox.Show("Por favor, selecione uma pergunta para excluir.",
                    "Pergunta Não Selecionada", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir esta pergunta?\n\n" +
                $"Enunciado: {_perguntaSelecionada.Enunciado}\n" +
                $"ID: {_perguntaSelecionada.Id}",
                "Confirmar Exclusão da Pergunta",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    GerenciadorPerguntas.RemoverPergunta(_perguntaSelecionada.Id);
                    MessageBox.Show($"Pergunta excluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                    CarregarPerguntasDaSessao(SessaoSelecionadaId);
                    LimparCamposPergunta();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir pergunta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LimparCamposPergunta()
        {
            txtEnunciado.Clear();
            txtAlternativa1.Clear();
            txtAlternativa2.Clear();
            txtAlternativa3.Clear();
            txtAlternativa4.Clear();
            txtAlternativaCorreta.Clear();
            _perguntaSelecionada = null;
            btnExcluirPergunta.IsEnabled = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) 
        {
            LimparCamposPergunta();
            MessageBox.Show("Campos limpos. Preencha os dados para uma nova pergunta.", "Campos Limpos", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) // Botão Excluir Sessão
        {
            if (SessaoSelecionadaId <= 0)
            {
                MessageBox.Show("Por favor, selecione uma sessão para excluir.", "Sessão Não Selecionada", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sessaoParaExcluir = GerenciadorSessao.GetSessoes().FirstOrDefault(s => s.Id == SessaoSelecionadaId);
            if (sessaoParaExcluir == null)
            {
                MessageBox.Show("Sessão não encontrada.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var resultado = MessageBox.Show($"Tem certeza que deseja excluir a sessão '{sessaoParaExcluir.NomeSessao}'?\n\nTodas as perguntas associadas a esta sessão também serão excluídas.", "Confirmar Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    bool sucesso = GerenciadorSessao.RemoverSessao(SessaoSelecionadaId);
                    if (sucesso)
                    {
                        MessageBox.Show($"Sessão '{sessaoParaExcluir.NomeSessao}' excluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        SessaoSelecionadaId = 0;
                        lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
                        ListaPerguntas.Children.Clear();
                        _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                        LimparCamposPergunta();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao excluir a sessão.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir sessão: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) // Botão Criar Nova Sessão
        {
            // 1. Cria a instância da janela de criação
            CriacaoSessao abrirTela = new CriacaoSessao(usuarioLogado);
            abrirTela.ShowDialog();
        }
        

        private void btnAttSessao_Click(object sender, RoutedEventArgs e)
        {
            AtualizarSessao novatela = new AtualizarSessao(SessaoSelecionadaId);
            novatela.ShowDialog();

            // Força a UI a reavaliar a coleção, caso o nome da sessão tenha mudado.
            ListaDeSessoes.Items.Refresh();
        }

        private void btnAtualizarPergunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_perguntaSelecionada == null)
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
                GerenciadorPerguntas.AtualizarPergunta(_perguntaSelecionada.Id, enunciado, alternativas, idRespostaCorreta);
                MessageBox.Show($"Pergunta ID {_perguntaSelecionada.Id} atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar pergunta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e) //Esse é o botão que habilita o quiz
        {
            MessageBox.Show($"quiz ativado {GerenciadorSessao.contadorSessoes}", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            if (_contadorTeste == 0) { GerenciadorSessao.contadorSessoes = 1; } else if(GerenciadorSessao.contadorSessoes > 1) {
                MessageBox.Show($"Já há um quiz ativo", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}