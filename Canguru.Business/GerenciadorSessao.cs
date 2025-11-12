using Canguru.Core;
using QuizTeste;
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
        public static void AtualizarSessao(int idSessao, string novoNome, string novaDescricao)
        {
            var sessao = _norteSessao.FirstOrDefault(s => s.Id == idSessao);
            if (sessao != null)
            {
                sessao.NomeSessao = novoNome;
                sessao.descricaoSessao = novaDescricao;
            }
        }
        public static bool RemoverSessao(int idSessao)
        {
            // Procura a sessão pelo ID
            var sessao = _norteSessao.FirstOrDefault(s => s.Id == idSessao);

            if (sessao == null)
                return false; // Sessão não encontrada

            // 🧹 Remove também as perguntas associadas a essa sessão
            GerenciadorPerguntas.RemoverPerguntasPorSessao(idSessao);

            // Remove a sessão da lista principal
            _norteSessao.Remove(sessao);

            return true; // Remoção bem-sucedida
        }

    }

}
