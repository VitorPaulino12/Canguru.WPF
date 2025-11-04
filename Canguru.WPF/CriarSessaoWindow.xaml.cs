using Canguru.Core;
using Canguru.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Compilador
{
    public partial class CriarSessaoWindow : Window
    {
        private List<Sessao> sessoes = new List<Sessao>();

        public CriarSessaoWindow()
        {
            InitializeComponent();
            CarregarSessoesNaLista();
        }

        private void btnAdicionarPergunta_Click(object sender, RoutedEventArgs e)
        {   
            //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            //CriacaoPerguntaWindow telaSessao = new CriacaoPerguntaWindow();
            //telaSessao.ShowDialog();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSessoes.Text))
            {
                MessageBox.Show("Digite um nome para a sessão!", "Aviso",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            Sessao novaSessao = new Sessao();
            novaSessao.Id = sessoes.Count + 1;
            novaSessao.Nome = txtSessoes.Text.Trim();

            sessoes.Add(novaSessao);
            CarregarSessoesNaLista();

            txtSessoes.Text = "";
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


                var inputDialog = new InputDialog("Editar nome da sessão:", "Editar Sessão", sessaoSelecionada.Nome);
                if (inputDialog.ShowDialog() == true)
                {
                    if (!string.IsNullOrWhiteSpace(inputDialog.Answer))
                    {
                        sessaoSelecionada.Nome = inputDialog.Answer.Trim();
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

        private void txtSessoes_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }


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


            this.txtAnswer = txtAnswer;
            this.lblQuestion = lblQuestion;
        }

        private TextBox txtAnswer;
        private Label lblQuestion;
    }
}