using Canguru.Core;
using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace Canguru.Business
{
    public static class GerenciadorGlobal
    {
        // Gerenciamento de Sessões
        private static List<Sessao> _sessoes = new List<Sessao>();

        // Gerenciamento de Perguntas
        private static List<Pergunta> _perguntas = new List<Pergunta>();
        private static Random _random = new Random();

      
        public static void AdicionarSessao(string nomeSessao, string descricaoSessao)
        {
            int novoId = _sessoes.Count == 0 ? 1 : _sessoes.Max(s => s.Id) + 1;
            Sessao novaSessao = new Sessao
            {
                Id = novoId,
                NomeSessao = nomeSessao,
                descricaoSessao = descricaoSessao
            };

            _sessoes.Add(novaSessao);
        }

        public static List<Sessao> ObterSessoes()
        {
            return _sessoes;
        }

        public static Sessao ObterSessaoPorId(int idSessao)
        {
            return _sessoes.FirstOrDefault(s => s.Id == idSessao);
        }

        public static void AtualizarSessao(int idSessao, string novoNome, string novaDescricao)
        {
            var sessao = _sessoes.FirstOrDefault(s => s.Id == idSessao);
            if (sessao != null)
            {
                sessao.NomeSessao = novoNome;
                sessao.descricaoSessao = novaDescricao;
            }
        }

        public static bool RemoverSessao(int idSessao)
        {
            var sessao = _sessoes.FirstOrDefault(s => s.Id == idSessao);
            if (sessao == null)
                return false;

            RemoverPerguntasPorSessao(idSessao);
            _sessoes.Remove(sessao);
            return true;
        }

        // ===== MÉTODOS DE PERGUNTAS =====

        public static List<Pergunta> GerarQuizAleatorio()
        {
            List<Pergunta> quiz = new List<Pergunta>();
            var sessoes = ObterSessoes();

            if (sessoes == null || sessoes.Count == 0)
                return quiz;

            List<int> sessoesValidas = new List<int>();
            foreach (var sessao in sessoes)
            {
                int qtdPerguntas = _perguntas.Count(p => p.IdSessao == sessao.Id);
                if (qtdPerguntas >= 5)
                    sessoesValidas.Add(sessao.Id);
            }

            if (sessoesValidas.Count == 0)
                return quiz;

            for (int i = 0; i < 10; i++)
            {
                int indiceSessao = _random.Next(sessoesValidas.Count);
                int idSessaoEscolhida = sessoesValidas[indiceSessao];

                var perguntasDaSessao = _perguntas.Where(p => p.IdSessao == idSessaoEscolhida).ToList();
                if (perguntasDaSessao.Count > 0)
                {
                    var perguntaEscolhida = perguntasDaSessao[_random.Next(perguntasDaSessao.Count)];
                    quiz.Add(perguntaEscolhida);
                }
            }

            return quiz;
        }

        public static List<Pergunta> ObterTodasPerguntas()
        {
            return _perguntas;
        }

        public static List<Pergunta> ObterPerguntasPorSessao(int idSessao)
        {
            return _perguntas.Where(p => p.IdSessao == idSessao).ToList();
        }

        public static int AdicionarPergunta(int idSessao, string enunciado, string[] alternativas, int idRespostaCorreta)
        {
            int novoId = _perguntas.Count == 0 ? 1 : _perguntas.Max(p => p.Id) + 1;
            var pergunta = new Pergunta
            {
                Id = novoId,
                IdSessao = idSessao,
                Enunciado = enunciado,
                Alternativas = alternativas,
                IdRespostaCorreta = idRespostaCorreta
            };

            _perguntas.Add(pergunta);
            return pergunta.Id;
        }

        public static void RemoverPergunta(int idPergunta)
        {
            var perguntaParaRemover = _perguntas.FirstOrDefault(p => p.Id == idPergunta);
            if (perguntaParaRemover != null)
                _perguntas.Remove(perguntaParaRemover);
        }

        public static void RemoverPerguntasPorSessao(int idSessao)
        {
            var perguntasParaRemover = _perguntas.Where(p => p.IdSessao == idSessao).ToList();
            foreach (var pergunta in perguntasParaRemover)
                _perguntas.Remove(pergunta);
        }

        public static void AtualizarPergunta(int idPergunta, string enunciado, string[] alternativas, int idRespostaCorreta)
        {
            var perguntaExistente = _perguntas.FirstOrDefault(p => p.Id == idPergunta);
            if (perguntaExistente == null)
                throw new Exception($"Pergunta com ID {idPergunta} não encontrada.");

            perguntaExistente.Enunciado = enunciado;
            perguntaExistente.Alternativas = alternativas;
            perguntaExistente.IdRespostaCorreta = idRespostaCorreta;
        }

        public static Pergunta ObterPerguntaPorId(int idPergunta)
        {
            return _perguntas.FirstOrDefault(p => p.Id == idPergunta);
        }

        // ===== MÉTODOS AUXILIARES =====

        public static int ObterQuantidadePerguntasPorSessao(int idSessao)
        {
            return _perguntas.Count(p => p.IdSessao == idSessao);
        }

        public static bool SessaoExiste(int idSessao)
        {
            return _sessoes.Any(s => s.Id == idSessao);
        }
    }
}