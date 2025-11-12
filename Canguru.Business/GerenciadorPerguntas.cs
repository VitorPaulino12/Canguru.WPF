using Canguru.Business;
using QuizTeste.Core;
using System;
using System.Collections.Generic;

namespace QuizTeste
{
    public static class GerenciadorPerguntas
    {
        private static List<Pergunta> _todasAsPerguntas = new List<Pergunta>();
        private static Random GirarDaddo = new Random();
        // Troquei o nome para ficar mais fácil imaginar a lógica disso funcionando
        // a ideia para essa parte é que ele tem que pegar o nº de perguntas que tem na sessão -1
        // e girar um dado para pegar uma pergunta aleatória dentre esse tanto de perguntas que tem nessa sessão


        static GerenciadorPerguntas()
        {
           // PopularTodasAsPerguntas();
            //Esse é um método para pegar as perguntas e colcoar no vetor que vai
            //armazenar as perguntas no quiz
        }

        public static List<Pergunta> GerarQuizAleatorio()
        {
            List<Pergunta> quiz = new List<Pergunta>();
            var sessoes = GerenciadorSessao.GetSessoes();
            if (sessoes == null || sessoes.Count == 0)
            {
                //pop-up de erro nãp hjá sessões registradas ainda
                return quiz;
            }

            // Filtra apenas as sessões que têm pelo menos 5 perguntas
            List<int> sessoesValidas = new List<int>();

            foreach (var sessao in sessoes)
            {
                int qtdPerguntas = _todasAsPerguntas.Count(p => p.IdSessao == sessao.Id);

                if (qtdPerguntas >= 5)
                {
                    sessoesValidas.Add(sessao.Id);
                }
            }

            // Se não há sessões válidas, não dá pra gerar quiz
            if (sessoesValidas.Count == 0)
            {
                //Exbibir pop-up de erro aki! não há sessões válidas
                
                return quiz;
            }

            // Gera 10 perguntas aleatórias misturadas entre as sessões válidas
            for (int i = 0; i < 10; i++)
            {
                int indiceSessao = GirarDaddo.Next(sessoesValidas.Count);
                int idSessaoEscolhida = sessoesValidas[indiceSessao];

                // Pega todas as perguntas dessa sessão
                var perguntasDaSessao = _todasAsPerguntas.Where(p => p.IdSessao == idSessaoEscolhida).ToList();

                // Escolhe uma aleatória
                var perguntaEscolhida = perguntasDaSessao[GirarDaddo.Next(perguntasDaSessao.Count)];

                quiz.Add(perguntaEscolhida);
            }

            return quiz;
        }
        public static List<Pergunta> GetTodasPerguntas()
        {
            return _todasAsPerguntas;
        }
        //método para adicionar uma pergunta,q ue eu disse que iria fazer
        public static void AdicionarPergunta(int idSessao, string enunciado, string[] alternativas, int idRespostaCorreta)
        {
            int novoid = _todasAsPerguntas.Count == 0 ? 1 : _todasAsPerguntas.Max(i => i.Id) + 1;
            var pergunta = new Pergunta
            {
                Id = novoid,
                IdSessao = idSessao,
                Enunciado = enunciado,
                Alternativas = alternativas,
                IdRespostaCorreta = idRespostaCorreta
            };

            _todasAsPerguntas.Add(pergunta);
        }
        public static void AtualizarPergunta(int idPergunta, string enunciado, string[] alternativas, int idRespostaCorreta)
        {
            //Para esse método só precisa do id da pergunta, porque  a gente já tem ela salva na memória só tem que alterar os atributos dela
            //usando como base as informações quee estão nos campos de texto que o usuário preencheu e atalizar os atributos com os novos
            // Busca a pergunta existente na lista pelo ID

            //armazena dentro de um a variavel local a pergunta 
            //vai em _todasAsPerguntas procura uma pergunta com o p.id idgual a idPergunta passado no parâmetro
            //logo depois coloca essa pergunta dentro de 'perguntaExistente'
            var perguntaExistente = _todasAsPerguntas.FirstOrDefault(p => p.Id == idPergunta);

            if (perguntaExistente == null)
            {
                throw new Exception($"Pergunta com ID {idPergunta} não encontrada.");
            }
             
            perguntaExistente.Enunciado = enunciado;
            perguntaExistente.Alternativas = alternativas;
            perguntaExistente.IdRespostaCorreta = idRespostaCorreta;
            //ideal ter um pop Up aki para notificar o usuário de que a eprgunta dele foi alterada com os novos atributos

        }
        public static Pergunta GetPerguntaPorId(int idPergunta)
        {
            //vai até onde esta salvo a pergunta e procura uma pergunta com um id especifico
            return _todasAsPerguntas.FirstOrDefault(p => p.Id == idPergunta);
        }
    }
}
