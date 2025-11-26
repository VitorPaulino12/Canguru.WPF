using System;
using System.Collections.Generic;

namespace Canguru.Core
{
    public static class GeradorAlunos
    {
        private static Random _rng = new();

        public static List<ResultadoAluno> Gerar(int quantidade)
        {
            var lista = new List<ResultadoAluno>();

            for (int i = 0; i < quantidade; i++)
            {
                var acertos = _rng.Next(5, 15);
                var erros = _rng.Next(0, 10);

                var aluno = new Aluno
                {
                    Nome = $"Aluno {i + 1}",
                    Email = $"aluno{i + 1}@teste.com"
                };

                lista.Add(new ResultadoAluno
                {
                    Aluno = aluno,
                    Acertos = acertos,
                    Erros = erros,
                    Pontos = acertos * 10
                });
            }

            return lista;
        }
    }
}
