using SAPbobsCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class TabelaGrupoCafe : Tabela
    {
        public Coluna GrupoDeItem { get { return new ColunaVarchar("ItmsGrpCod", "Código Grupo de Item", 30); } }

        public TabelaGrupoCafe() : base("UPD_OCTC", "Grupos de Café", BoUTBTableType.bott_NoObject)
        {
        }
    }
}
