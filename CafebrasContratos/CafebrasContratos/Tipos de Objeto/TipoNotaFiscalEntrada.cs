using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoNotaFiscalEntrada : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
        public string Nome => "Nota Fiscal de Entrada";
        public int IndexParaCombo => 1;
        public string Tabela => "OPCH";
        public TipoDeObjetoDeContrato ObjetoBase => null;
        public FormDocumentoMarketing Form => new FormNotaFiscalEntrada();
    }
}
