using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Business
{
    public class GerenciadorNotificacoes
    {
        public static List<(string Texto, Action Acao)> Notificacoes = new List<(string, Action)>();

        public static event Action<string, Action> NovaNotificacao;

        public static void Adicionar(string texto, Action acao)
        {
            Notificacoes.Add((texto, acao));
            NovaNotificacao?.Invoke(texto, acao);
        }
        public static void LimparNotificacoes()
        {
            Notificacoes.Clear();
        }
    }
}
