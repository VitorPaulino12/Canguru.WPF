using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Business
{
    public class GerenciadorDePosts
    {
        private static List<Post> posts = new List<Post>();

        public static void AdicionarPost(Post p)
        {
            posts.Add(p);
        }

        public static List<Post> ObterPosts()
        {
            return posts;
        }
    }
}
