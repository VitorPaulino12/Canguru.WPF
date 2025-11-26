namespace Canguru.Core
{
    public class ResultadoAluno
    {
        public Aluno Aluno { get; set; }
        public int Pontos { get; set; }
        public int Acertos { get; set; }
        public int Erros { get; set; }
        public double Porcentagem => (double)Acertos / (Acertos + Erros) * 100;
    }
}
