using SAPHelper;
using SAPbouiCOM;
using System;

namespace CafebrasContratos
{
    public class FormPedidoCompra : FormDocumentoMarketing
    {
        public override string FormType { get { return ((int)FormTypes.PedidoDeCompra).ToString(); } }
        public override string mainDbDataSource { get { return "OPOR"; } }
        public override BoFormObjectEnum formEnum { get { return BoFormObjectEnum.fo_PurchaseOrder; } }
    }
}
