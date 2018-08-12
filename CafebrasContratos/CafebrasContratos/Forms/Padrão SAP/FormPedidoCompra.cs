using SAPHelper;

namespace CafebrasContratos
{
    public class FormPedidoCompra : FormDocumentoMarketing
    {
        public override string FormType { get { return ((int)FormTypes.PedidoDeCompra).ToString(); } }
        public override string mainDbDataSource { get { return "OPOR"; } }
    }
}
