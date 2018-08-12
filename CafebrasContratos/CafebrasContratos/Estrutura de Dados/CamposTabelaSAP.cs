using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class CamposTabelaSAP
    {
        public Coluna grupoAprovador = new ColunaVarchar("GrupoAprov", "Grupo Aprovador", 2, false, "V", new List<ValorValido>() {
                            new ValorValido(GrupoAprovador.Planejador, "Planejador"),
                            new ValorValido(GrupoAprovador.Executor, "Executor"),
                            new ValorValido(GrupoAprovador.Autorizador, "Autorizador"),
                            new ValorValido(GrupoAprovador.Gestor, "Gestor"),
                            new ValorValido(GrupoAprovador.Visualizador, "Visualizador")
                        });

        public Coluna numeroContratoFilho = new ColunaVarchar("DocNumCF", "Numero Contrato Final", 50);
        public Coluna filhoDeContrato = new ColunaVarchar("SonOfContract", "Son of Contract", 1, false, "N");
    }
}
