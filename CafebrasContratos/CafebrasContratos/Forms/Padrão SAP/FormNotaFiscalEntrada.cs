using SAPHelper;
using SAPbouiCOM;

namespace CafebrasContratos
{
    public class FormNotaFiscalEntrada : FormDocumentoMarketing
    {
        public override string FormType { get { return ((int)FormTypes.NotaFiscalDeEntrada).ToString(); } }
        public override string mainDbDataSource { get { return "OPCH"; } }
        public override BoFormObjectEnum formEnum { get { return BoFormObjectEnum.fo_PurchaseInvoice; } }
    }
}
