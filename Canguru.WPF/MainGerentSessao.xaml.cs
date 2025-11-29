using Canguru.Business;
using Canguru.Core;
using Canguru.WPF.Pop_Ups;
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
                    //MessageBox.Show($"Erro: não foi possível localizar a pergunta de ID {PerguntaSelecionadaId}.",
                    //  "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    var PopUp = new PopUpsGerais(21);
                    PopUp.ShowDialog();
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
                // MessageBox.Show($"Por favor, selecione uma sessão antes de criar uma pergunta. (SessaoSelecionadaId: {SessaoSelecionadaId})",
                //   "Sessão Não Selecionada", MessageBoxButton.OK, MessageBoxImage.Warning);
                var PopUp = new PopUpsGerais(23);
                PopUp.ShowDialog();
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
                    // MessageBox.Show("Por favor, insira um número válido para a alternativa correta (0-3).",
                    //     "Alternativa Correta Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    var PopUp = new PopUpsGerais(24);
                    PopUp.ShowDialog();
                    return;
                }

                if (string.IsNullOrWhiteSpace(enunciado))
                {
                   // MessageBox.Show("Por favor, insira o enunciado da pergunta.",
                   //     "Enunciado Vazio", MessageBoxButton.OK, MessageBoxImage.Warning);
                    var PopUp = new PopUpsGerais(25);
                    PopUp.ShowDialog();
                    return;
                }

                if (alternativas.Any(string.IsNullOrWhiteSpace))
                {
                    //MessageBox.Show("Por favor, preencha todas as alternativas.",
                    //    "Alternativas Incompletas", MessageBoxButton.OK, MessageBoxImage.Warning);
                    var PopUp = new PopUpsGerais(26);
                    PopUp.ShowDialog();
                    return;
                }

                if (idRespostaCorreta < 0 || idRespostaCorreta > 3)
                {
                    var PopUp = new PopUpsGerais(27);
                    PopUp.ShowDialog();
                    //MessageBox.Show("A alternativa correta deve ser um número entre 0 e 3.",
                       // "Alternativa Correta Fora do Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int novaPerguntaId = GerenciadorPerguntas.AdicionarPergunta(SessaoSelecionadaId, enunciado, alternativas, idRespostaCorreta);
                var popup = new Pop_Ups.PopUpInfoemações("Sucesso",$"Pergunta salva com sucesso!\n\nID gerado: {novaPerguntaId}");

                LimparCamposPergunta();
                _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                var PopUp = new PopUpsGerais(28);
                PopUp.ShowDialog();
            }
        }

        private void BtnExcluirPergunta_Click(object sender, RoutedEventArgs e)
        {
            if (_perguntaSelecionada == null)
            {
                var PopUp = new PopUpsGerais(29);
                PopUp.ShowDialog();
                return;
            }

            var popupConfirmacao = new Canguru.WPF.Pop_Ups.PopUpInfoemações(
                "Confirmar Exclusão da Pergunta",
                $"Tem certeza que deseja excluir esta pergunta?\n\n" +
                $"Enunciado: {_perguntaSelecionada.Enunciado}\n" +
                $"ID: {_perguntaSelecionada.Id}",

                () =>
                {
                    try
                    {
                        GerenciadorPerguntas.RemoverPergunta(_perguntaSelecionada.Id);
                        new Canguru.WPF.Pop_Ups.PopUpInfoemações("Sucesso","Pergunta excluída com sucesso!").ShowDialog();

                        _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                        CarregarPerguntasDaSessao(SessaoSelecionadaId);
                        LimparCamposPergunta();
                    }
                    catch (Exception ex)
                    {
                        new Canguru.WPF.Pop_Ups.PopUpInfoemações("Erro",$"Erro ao excluir pergunta:\n{ex.Message}").ShowDialog();
                    }
                }
            );

            popupConfirmacao.ShowDialog();
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
            GerenciadorSessao.RemoverSessao(SessaoSelecionadaId);
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (SessaoSelecionadaId <= 0)
            {
                var popUp = new PopUpsGerais(30);
                popUp.ShowDialog();
                return;
            }

            var sessaoParaExcluir = GerenciadorSessao.GetSessoes()
                .FirstOrDefault(s => s.Id == SessaoSelecionadaId);

            if (sessaoParaExcluir == null)
            {
                var popUp = new PopUpsGerais(31);
                popUp.ShowDialog();
                return;
            }

            var popConfirmacao = new PopUpsGerais(17, () =>
            {
                try
                {
                    bool sucesso = GerenciadorSessao.RemoverSessao(SessaoSelecionadaId);

                    if (sucesso)
                    {
                        new PopUpsGerais(15).ShowDialog();

                        SessaoSelecionadaId = 0;
                        lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
                        ListaPerguntas.Children.Clear();
                        _perguntasUsuario = GerenciadorPerguntas.GetTodasPerguntas();
                        LimparCamposPergunta();
                    }
                    else
                    {
                        new PopUpsGerais(31).ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir sessão: {ex.Message}");
                }
            });

            popConfirmacao.ShowDialog();
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
                    //MessageBox.Show("Selecione uma pergunta antes de atualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    var PopUp = new PopUpsGerais(32);
                    PopUp.ShowDialog();
                    return;
                }

                string enunciado = txtEnunciado.Text.Trim();
                string[] alternativas = new string[] { txtAlternativa1.Text.Trim(), txtAlternativa2.Text.Trim(), txtAlternativa3.Text.Trim(), txtAlternativa4.Text.Trim() };
                if (!int.TryParse(txtAlternativaCorreta.Text.Trim(), out int idRespostaCorreta))
                {
                    //MessageBox.Show("Digite um número válido para a alternativa correta (0–3).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                    var PopUp = new PopUpsGerais(27);
                    PopUp.ShowDialog();
                    return;
                }
                GerenciadorPerguntas.AtualizarPergunta(_perguntaSelecionada.Id, enunciado, alternativas, idRespostaCorreta);
                var popUp = new PopUpsGerais(33);
                popUp.ShowDialog();

                CarregarPerguntasDaSessao(SessaoSelecionadaId);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao atualizar pergunta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                var popUp = new PopUpsGerais(34);
                popUp.ShowDialog();
            }
        }

        private void AtivarQuiz_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"um quiz está ativo! {GerenciadorNotificacoes.Notificacoes.Count + 1}", "Notificação", MessageBoxButton.OK, MessageBoxImage.Information);
            var popup = new PopUpComImagem("Sucesso","Pergunta atualizada com sucesso!","Assets/5.png");
            GerenciadorNotificacoes.Adicionar("Novo quiz disponível!", () => { TelaPerguntas tela = new TelaPerguntas(usuarioLogado); tela.Show(); });
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCamposPergunta();

            // Remove destaque visual das perguntas
            foreach (var child in ListaPerguntas.Children)
            {
                if (child is Border b)
                {
                    b.Background = new SolidColorBrush(Colors.WhiteSmoke);
                }
            }

            PerguntaSelecionadaId = 0;
        }

        private void AtivarQuizFinal_Click(object sender, RoutedEventArgs e)
        {
            // caminho relativo — veja nota abaixo sobre Build Action
            string caminhoImagem = "Assets/5.png";

            // checar se arquivo existe (ajuda a depurar)
            try
            {
                if (!System.IO.File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, caminhoImagem)))
                {
                    // tenta usar pack uri como fallback (se imagem for Resource)
                    // não interrompe: a janela abrirá sem imagem
                }

                var popup = new PopUpComImagem(
                    "Sucesso",
                    "Quiz ativado com sucesso!",
                    caminhoImagem
                );

                // opcional: define owner para centralizar e manter foco
                popup.Owner = this;

                // mostra a janela (bloqueante)
                popup.ShowDialog();

                // adiciona notificação para abrir o quiz (a ação não depende do popup fechar)
                GerenciadorNotificacoes.Adicionar("Novo quiz disponível!", () =>
                {
                    var tela = new TelaPerguntas(usuarioLogado);
                    tela.Show();
                });
            }
            catch (Exception ex)
            {
                // debug rápido
                MessageBox.Show($"Erro ao abrir popup: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}