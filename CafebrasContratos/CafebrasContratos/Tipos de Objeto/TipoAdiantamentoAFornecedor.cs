using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoAdiantamentoAFornecedor : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseDownPayments;
        public string Nome => "Adiantamento a Fornecedor";
        public int IndexParaCombo => 3;
        public string Tabela => "ODPO";
        public TipoDeObjetoDeContrato ObjetoBase => null;
        public FormDocumentoMarketing Form => new FormAdiantamentoFornecedor();
    }
}
