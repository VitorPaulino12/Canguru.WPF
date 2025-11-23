using Canguru.Core;
using QuizTeste;
using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Canguru.Business
{
    public static class GerenciadorSessao
    {
        //private static List<Sessao> _norteSessao = new List<Sessao>();
        //trocar isso resolve a questão de atualizar de maneira responsiva depois que o usuárioo clica no botão de adicionar atualizar sessão
        private static ObservableCollection<Sessao> _norteSessao = new ObservableCollection<Sessao>();
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

        public static ObservableCollection<Sessao> GetSessoes()
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
            var sessao = _norteSessao.FirstOrDefault(s => s.Id == idSessao);

            if (sessao == null)
                return false;

            GerenciadorPerguntas.RemoverPerguntasPorSessao(idSessao);

            _norteSessao.Remove(sessao);

            return true;
        }

    }

}
