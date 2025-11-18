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
        private int PerguntaSelecionadaId;
        private List<Pergunta> _perguntasUsuario = new List<Pergunta>();
        private Pergunta _perguntaSelecionada;

        public MainGerentSessao(Usuario usuario)
        {
            InitializeComponent();
            CarregarSessoes();
            usuarioLogado = usuario;
            SessaoSelecionadaId = 0;
            _perguntasUsuario = GerenciadorGlobal.ObterTodasPerguntas();
            lblSessaoSelecionada.Text = "Nenhuma sessão selecionada";
            btnExcluirPergunta.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TelaHome telaHome = new TelaHome(usuarioLogado);
            telaHome.Show();
            this.Close();
        }

        public void CarregarSessoes()
        {
            var sessoes = GerenciadorGlobal.ObterSessoes();

            if (sessoes != null && sessoes.Count > 0)
            {
                ListaDeSessoes.ItemsSource = sessoes;
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
                _perguntasUsuario = GerenciadorGlobal.ObterTodasPerguntas();
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
