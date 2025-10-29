using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls; // Adicione esta linha
using Canguru.Core;

namespace Compilador
{
    public partial class CriarSessaoWindow : Window
    {
        private List<Sessao> sessoes = new List<Sessao>();
        private string arquivoSessoes = "sessoes.txt";

        public CriarSessaoWindow()
        {
            InitializeComponent();
            CarregarSessoes();
            CarregarSessoesNaLista();
        }

        private void btnAdicionarPergunta_Click(object sender, RoutedEventArgs e)
        {
            //adicionar funcionalidade
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSessoes.Text))
            {
                MessageBox.Show("Digite um nome para a sessão!", "Aviso",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cria nova sessão
            Sessao novaSessao = new Sessao();
            novaSessao.Id = sessoes.Count + 1;
            novaSessao.Nome = txtSessoes.Text.Trim();

            sessoes.Add(novaSessao);
            SalvarSessoes();
            CarregarSessoesNaLista();

            txtSessoes.Text = ""; // Limpa o campo
            MessageBox.Show($"Sessão '{novaSessao.Nome}' criada com sucesso!", "Sucesso",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSessoes.SelectedIndex != -1)
            {
                Sessao sessaoSelecionada = sessoes[listBoxSessoes.SelectedIndex];

                // Abre dialog para editar nome
                var inputDialog = new InputDialog("Editar nome da sessão:", "Editar Sessão", sessaoSelecionada.Nome);
                if (inputDialog.ShowDialog() == true)
                {
                    if (!string.IsNullOrWhiteSpace(inputDialog.Answer))
                    {
                        sessaoSelecionada.Nome = inputDialog.Answer.Trim();
                        SalvarSessoes();
                        CarregarSessoesNaLista();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione uma sessão para editar!", "Aviso",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSessoes.SelectedIndex != -1)
            {
                Sessao sessaoSelecionada = sessoes[listBoxSessoes.SelectedIndex];
                var result = MessageBox.Show($"Deseja excluir a sessão '{sessaoSelecionada.Nome}'?",
                                           "Confirmar Exclusão",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    sessoes.Remove(sessaoSelecionada);
                    SalvarSessoes();
                    CarregarSessoesNaLista();
                }
            }
            else
            {
                MessageBox.Show("Selecione uma sessão para excluir!", "Aviso",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CarregarSessoesNaLista()
        {
            listBoxSessoes.Items.Clear();
            foreach (var sessao in sessoes)
            {
                listBoxSessoes.Items.Add($"{sessao.Nome} - {sessao.Perguntas.Count} perguntas");
            }
        }

        private void SalvarSessoes()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoSessoes))
                {
                    foreach (var sessao in sessoes)
                    {
                        sw.WriteLine($"SESSAO:{sessao.Id}|{sessao.Nome}");
                        foreach (var pergunta in sessao.Perguntas)
                        {
                            sw.WriteLine($"PERGUNTA:{pergunta.Id}|{pergunta.Enunciado}|{pergunta.IndiceRespostaCorreta}");
                            foreach (var alternativa in pergunta.Alternativas)
                            {
                                sw.WriteLine($"ALTERNATIVA:{alternativa}");
                            }
                        }
                        sw.WriteLine("FIM_SESSAO");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar sessões: {ex.Message}", "Erro",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CarregarSessoes()
        {
            if (!File.Exists(arquivoSessoes)) return;

            try
            {
                sessoes.Clear();
                string[] linhas = File.ReadAllLines(arquivoSessoes);

                Sessao sessaoAtual = null;
                Pergunta perguntaAtual = null;

                foreach (string linha in linhas)
                {
                    if (linha.StartsWith("SESSAO:"))
                    {
                        string[] dados = linha.Substring(7).Split('|');
                        sessaoAtual = new Sessao
                        {
                            Id = int.Parse(dados[0]),
                            Nome = dados[1]
                        };
                        sessoes.Add(sessaoAtual);
                    }
                    else if (linha.StartsWith("PERGUNTA:"))
                    {
                        string[] dados = linha.Substring(9).Split('|');
                        perguntaAtual = new Pergunta
                        {
                            Id = int.Parse(dados[0]),
                            Enunciado = dados[1],
                            IndiceRespostaCorreta = int.Parse(dados[2])
                        };
                        sessaoAtual.Perguntas.Add(perguntaAtual);
                    }
                    else if (linha.StartsWith("ALTERNATIVA:"))
                    {
                        perguntaAtual.Alternativas.Add(linha.Substring(12));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar sessões: {ex.Message}", "Erro",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Classe auxiliar para InputDialog
    public partial class InputDialog : Window
    {
        public string Answer { get; set; }

        public InputDialog(string question, string title, string defaultAnswer = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
            Title = title;
        }

        private void InitializeComponent()
        {
            this.Height = 150;
            this.Width = 300;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10);

            var lblQuestion = new Label();
            lblQuestion.Content = "Question";
            stackPanel.Children.Add(lblQuestion);

            var txtAnswer = new TextBox();
            txtAnswer.Height = 23;
            txtAnswer.Margin = new Thickness(0, 5, 0, 10);
            stackPanel.Children.Add(txtAnswer);

            var buttonPanel = new StackPanel();
            buttonPanel.Orientation = Orientation.Horizontal;
            buttonPanel.HorizontalAlignment = HorizontalAlignment.Right;

            var btnOk = new Button();
            btnOk.Content = "OK";
            btnOk.Width = 75;
            btnOk.Margin = new Thickness(0, 0, 10, 0);
            btnOk.Click += (s, e) => { Answer = txtAnswer.Text; DialogResult = true; };
            buttonPanel.Children.Add(btnOk);

            var btnCancel = new Button();
            btnCancel.Content = "Cancel";
            btnCancel.Width = 75;
            btnCancel.Click += (s, e) => { DialogResult = false; };
            buttonPanel.Children.Add(btnCancel);

            stackPanel.Children.Add(buttonPanel);
            this.Content = stackPanel;

            // Set references for event handlers
            this.txtAnswer = txtAnswer;
            this.lblQuestion = lblQuestion;
        }

        private TextBox txtAnswer;
        private Label lblQuestion;
    }
}