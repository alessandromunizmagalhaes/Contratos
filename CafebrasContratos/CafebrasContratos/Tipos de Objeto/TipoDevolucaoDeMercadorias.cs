using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoDevolucaoDeMercadorias : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseReturns;
        public string Nome => "Devolução de Mercadorias";
        public int IndexParaCombo => 4;
        public string Tabela => "ORPD";
        public TipoDeObjetoDeContrato ObjetoBase => new TipoRecebimentoDeMercadorias();
        public FormDocumentoMarketing Form => new FormDevolucaoMercadoria();
    }
}
