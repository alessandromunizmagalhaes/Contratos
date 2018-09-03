using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoPedidoDeCompra : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseOrders;
        public string Nome => "Pedido de Compra";
        public int IndexParaCombo => 0;
        public string Tabela => "OPOR";
        public TipoDeObjetoDeContrato ObjetoBase => null;

        public FormDocumentoMarketing Form => new FormPedidoCompra();
    }
}
