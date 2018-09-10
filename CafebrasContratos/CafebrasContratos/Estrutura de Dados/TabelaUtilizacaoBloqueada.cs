using SAPbobsCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class TabelaUtilizacaoBloqueada : Tabela
    {
        public Coluna Utilizacao { get { return new ColunaVarchar("Usage", "Código Utilização", 30); } }

        public TabelaUtilizacaoBloqueada() : base("UPD_DENIED_USAGE", "Utilizações Bloqueadas", BoUTBTableType.bott_NoObject)
        {
        }
    }
}
