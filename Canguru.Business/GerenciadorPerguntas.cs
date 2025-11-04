using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizTeste
{
    public static class GerenciadorPerguntas
    {
        private static List<Pergunta> _todasAsPerguntas = new List<Pergunta>();
        private static Random _rand = new Random();

        static GerenciadorPerguntas()
        {
            PopularTodasAsPerguntas();
        }

        public static List<Pergunta> GerarQuizAleatorio()
        {
            var perguntasPorSessao = _todasAsPerguntas
                .GroupBy(p => p.IdSessao)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<Pergunta> quiz = new List<Pergunta>();

            // O quiz terá 10 perguntas, misturadas de sessões diferentes
            for (int i = 0; i < 10; i++)
            {
                // Escolhe uma sessão aleatória
                int sessaoEscolhida = perguntasPorSessao.Keys.ElementAt(_rand.Next(perguntasPorSessao.Count));
                var perguntasDaSessao = perguntasPorSessao[sessaoEscolhida];

                // Escolhe uma pergunta aleatória dessa sessão
                var perguntaEscolhida = perguntasDaSessao[_rand.Next(perguntasDaSessao.Count)];

                quiz.Add(perguntaEscolhida);
            }

            return quiz;
        }

        private static void PopularTodasAsPerguntas()
        {
            // Sessão 1
            _todasAsPerguntas.Add(new Pergunta { Id = 1, IdSessao = 1, Enunciado = "Quanto é 5 + 3?", Alternativas = new[] { "6", "7", "8", "9" }, IdRespostaCorreta = 2 });
            _todasAsPerguntas.Add(new Pergunta { Id = 2, IdSessao = 1, Enunciado = "Quanto é 9 - 4?", Alternativas = new[] { "4", "5", "6", "3" }, IdRespostaCorreta = 1 });
            _todasAsPerguntas.Add(new Pergunta { Id = 3, IdSessao = 1, Enunciado = "Quanto é 3 x 2?", Alternativas = new[] { "5", "6", "7", "8" }, IdRespostaCorreta = 1 });
            _todasAsPerguntas.Add(new Pergunta { Id = 4, IdSessao = 1, Enunciado = "Quanto é 15 / 3?", Alternativas = new[] { "3", "4", "5", "6" }, IdRespostaCorreta = 2 });

            // Sessão 2
            _todasAsPerguntas.Add(new Pergunta { Id = 11, IdSessao = 2, Enunciado = "Quanto é 8 + 7?", Alternativas = new[] { "13", "14", "15", "16" }, IdRespostaCorreta = 1 });
            _todasAsPerguntas.Add(new Pergunta { Id = 12, IdSessao = 2, Enunciado = "Quanto é 25 - 9?", Alternativas = new[] { "14", "15", "16", "17" }, IdRespostaCorreta = 2 });

            // Sessão 3
            _todasAsPerguntas.Add(new Pergunta { Id = 21, IdSessao = 3, Enunciado = "Quanto é 4 x 4?", Alternativas = new[] { "14", "15", "16", "17" }, IdRespostaCorreta = 2 });
            _todasAsPerguntas.Add(new Pergunta { Id = 22, IdSessao = 3, Enunciado = "Quanto é 30 / 5?", Alternativas = new[] { "5", "6", "7", "8" }, IdRespostaCorreta = 0 });
        }
    }
}
