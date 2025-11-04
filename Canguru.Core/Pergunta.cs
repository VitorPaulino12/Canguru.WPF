namespace QuizTeste.Core
{
    public class Pergunta
    {
        public int Id { get; set; }
        public int IdSessao { get; set; }
        public string Enunciado { get; set; }
        public string[] Alternativas { get; set; }
        public int IdRespostaCorreta { get; set; }

        public bool ValidarResposta(int indiceEscolhido)
        {
            return indiceEscolhido == IdRespostaCorreta;
        }
    }
}
