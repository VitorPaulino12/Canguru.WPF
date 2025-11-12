using Canguru.Core;
using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Business
{
    public static class GerenciadorSessao
    {
        private static List<Sessao> _norteSessao = new List<Sessao>();

        public static void AddSessao(string nomeSessao, string descricaoSessaobase)
        {
           // int novoId = _norteSessao.Count == 0 ? 0 : _norteSessao.Max(s => s.Id) + 1;
            int novoId = _norteSessao.Count == 0 ? 1 : _norteSessao.Max(s => s.Id) + 1;
            Sessao novaSessao = new Sessao
            {
                Id = novoId,
                NomeSessao = nomeSessao,
                descricaoSessao = descricaoSessaobase
            };

            _norteSessao.Add(novaSessao);
        }

        public static List<Sessao> GetSessoes()
        {
            return _norteSessao;
        }
        public static void AtualizarSessoes(int idsessao, string NovoNomeSessao, string descricao)
        {
            
            var sessaoExistente = _norteSessao.FirstOrDefault(p => p.Id == idsessao);

            if (sessaoExistente == null)
            {
                throw new Exception($"Pergunta com ID {idsessao} não encontrada.");
            }
            sessaoExistente.NomeSessao = NovoNomeSessao;
            sessaoExistente.descricaoSessao = descricao;
            
            //perguntaExistente.Enunciado = enunciado;
            //perguntaExistente.Alternativas = alternativas;
            //perguntaExistente.IdRespostaCorreta = idRespostaCorreta;
            //ideal ter um pop Up aki para notificar o usuário de que a eprgunta dele foi alterada com os novos atributos

        }
        public static Sessao GetSessoesPorId(int idsessao)
        {
            //vai até onde esta salvo a pergunta e procura uma pergunta com um id especifico
            return _norteSessao.FirstOrDefault(p => p.Id == idsessao);
        }
    }

}
