using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoRecebimentoDeMercadorias : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes;
        public string Nome => "Recebimento De Mercadorias";
        public int IndexParaCombo => 2;
        public string Tabela => "OPDN";
        public TipoDeObjetoDeContrato ObjetoBase => new TipoPedidoDeCompra();
        public FormDocumentoMarketing Form => new FormRecebimentoMercadoria();
    }
}
