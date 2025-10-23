
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core

{
    public abstract class Atividade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int IdCriador { get; set; }

        protected Atividade(int id, string nome, string descricao, int idCriador)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            IdCriador = idCriador;
        }

        public abstract void ExibirDetalhes();
    }
}