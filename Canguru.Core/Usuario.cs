using Canguru.Core;

public abstract class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public string Email { get; set; }
    public string RA { get; set; }
    public TipoUsuario Tipo { get; set; } = TipoUsuario.Professor;

    public string CaminhoFotoPerfil { get; set; } = @"\assets\img\default.png";

    public DateTime DataEntrada { get; set; }
    public string Status { get; set; }
}
