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
            int novoId = _norteSessao.Count == 0 ? 0 : _norteSessao.Max(s => s.Id) + 1;

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
    }

}
