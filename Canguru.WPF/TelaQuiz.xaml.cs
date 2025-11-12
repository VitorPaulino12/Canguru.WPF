using Canguru.Core;
using Canguru.Business;
using Canguru.WPF;
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
        private Usuario usuarioLogado;
        private List<Pergunta> _quiz;
        private int _indiceAtual = 0;
        private int _pontuacao = 0;

        // variaveis do gerenciador interações

        public TelaPerguntas(Usuario usuario)
        {
            InitializeComponent();
            usuarioLogado = usuario;
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
                // se a pessoa acertou tem que adiconar em um vetor o conteudo (id pergunta + resp) e ao final adicionar n gerenciador Interaçoes junto ao id do usuario
                // tem que fazer o mesmo se a pessoa errou só que a resp = incorreto ou algo assim.
            }
            else
            {
                botao.Background = Brushes.LightCoral;
                var botaoCorreto = FindName($"btnAlt{pergunta.IdRespostaCorreta + 1}") as Button;
                if (botaoCorreto != null)
                {
                    botaoCorreto.Background = Brushes.LightGreen;
                }
            }
            //int novoId = GerenciadorInteracao.Count == 0 ? 0 : GerenciadorInteracao.Max(s => s.Id) + 1;


            // perguntar pro grupo depois de faz sentido isso ser um bool
            //porque ou ta certo ou errado...

            // Aqui salva no histórico de interações
            int resultado = acertou ? 1 : 0; // você pode guardar 1=acerto, 0=erro (ou bool, se preferir)
            GerenciadorInteracao.AddInteracao(usuarioLogado.Id, pergunta.Id, resultado);

            await Task.Delay(1000);

            _indiceAtual++;
            ExibirPergunta();
        }

        private void FinalizarQuiz()
        {
            // ao finalizar o código deve pegar as informações e gravalas dentro de um repositório na memória
            // que diz respeito as interações de cada usuário, salvando o Id da interação, o id do usuário (para ser rastreado depois) e o id da pergunta para..
            // saber qual foi e não precisar realizar um salvamento de pergunta no repositório, uma vez que se tiver o Id da eprgunta é só buscar no repositório
            //onde está salvo uma eprgunta com o memso id, e depois mais um atributo para identificar se foi um erro ou acerto
            double percentual = (double)_pontuacao / _quiz.Count * 100;
            MessageBox.Show($"Quiz finalizado!\n\nAcertos: {_pontuacao} de {_quiz.Count}\nDesempenho: {percentual:F1}%",
                "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);

            //Registro de informação no 'Historico_GlobalResultados'
            //N precisa passar o identificadorQuiz porq na criação do objeto dentro do método add
            //ele já cria uma variavel local e adiciona um id ao atributo IdentificadorQuiz
            GerenciadorResultFinal.addResultado_Lista(usuarioLogado.Id,percentual);
            //


            //Volta para tela principal
            TelaHome telaHome = new TelaHome(usuarioLogado);
            //telaHome.Show();
            this.Close();
        }
    }
}
