using SAPHelper;
using SAPbouiCOM;
using System;

namespace CafebrasContratos
{
    public class FormAdiantamentoFornecedor : FormDocumentoMarketing
    {
        public override string FormType { get { return ((int)FormTypes.AdiantamentoFornecedor).ToString(); } }
        public override string mainDbDataSource { get { return "ODPO"; } }

        // a porcaria do SAP não tem o enum pra adiantamento de fornecedor.
        // setando qualquer um só pq essa propriedade tem abstract e precisa ser informada
        // sobrepondo o método que usa ela pra não atrapalhar quem já usa.
        public override BoFormObjectEnum formEnum { get { return BoFormObjectEnum.fo_CashDiscount; } }
        
        public override SAPbouiCOM.Form Abrir(string codigo = "")
        {
            return AbrirNaMao("2317", codigo);
        }
    }
}
