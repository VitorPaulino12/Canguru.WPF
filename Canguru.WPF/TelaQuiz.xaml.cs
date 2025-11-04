using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizTeste
{
    public partial class TelaPerguntas : Window
    {
        private List<Pergunta> _quiz;
        private int _indiceAtual = 0;
        private int _pontuacao = 0;

        public TelaPerguntas()
        {
            InitializeComponent();
            IniciarQuiz();
        }

        private void IniciarQuiz()
        {
            _quiz = GerenciadorPerguntas.GerarQuizAleatorio();
            _indiceAtual = 0;
            _pontuacao = 0;
            ExibirPergunta();
        }

        private void ExibirPergunta()
        {
            if (_indiceAtual >= _quiz.Count)
            {
                FinalizarQuiz();
                return;
            }

            var p = _quiz[_indiceAtual];
            txtPergunta.Text = p.Enunciado;
            btnAlt1.Content = p.Alternativas[0];
            btnAlt2.Content = p.Alternativas[1];
            btnAlt3.Content = p.Alternativas[2];
            btnAlt4.Content = p.Alternativas[3];

            foreach (var b in new[] { btnAlt1, btnAlt2, btnAlt3, btnAlt4 })
            {
                b.IsEnabled = true;
                b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
            }
        }

        private async void Responder_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            botao.IsEnabled = false;
            int indiceEscolhido = Convert.ToInt32(botao.Tag);
            var pergunta = _quiz[_indiceAtual];

            bool acertou = pergunta.ValidarResposta(indiceEscolhido);
            if (acertou)
            {
                _pontuacao++;
                botao.Background = Brushes.LightGreen;
            }
            else
            {
                botao.Background = Brushes.LightCoral;
                var botaoCorreto = FindName($"btnAlt{pergunta.IdRespostaCorreta + 1}") as Button;
                if (botaoCorreto != null)
                    botaoCorreto.Background = Brushes.LightGreen;
            }

            await Task.Delay(1000);

            _indiceAtual++;
            ExibirPergunta();
        }

        private void FinalizarQuiz()
        {
            double percentual = (double)_pontuacao / _quiz.Count * 100;
            MessageBox.Show($"Quiz finalizado!\n\nAcertos: {_pontuacao} de {_quiz.Count}\nDesempenho: {percentual:F1}%",
                "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
        }
    }
}
