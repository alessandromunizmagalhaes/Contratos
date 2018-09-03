namespace CafebrasContratos
{
    public interface TipoDeObjetoDeContrato
    {
        int Tipo { get; }
        string Nome { get; }
        int IndexParaCombo { get; }
        string Tabela { get; }
        TipoDeObjetoDeContrato ObjetoBase { get; }
        FormDocumentoMarketing Form { get; }
    }
}
