using Canguru.Core;
using System.Collections.Generic;
using System.Linq;

namespace Canguru.Business
{
    public static class GerenciadorDePosts
    {
        // A lista de posts agora é estática e persiste durante a execução da aplicação
        private static List<Post> _posts = new List<Post>();

        public static void AdicionarPost(Post p)
        {
            _posts.Insert(0, p); // Adiciona no topo da lista
        }

        public static List<Post> ObterPosts()
        {
            // Retorna a lista ordenada por data, do mais novo para o mais antigo
            return _posts.OrderByDescending(p => p.Data).ToList();
        }
    }
}