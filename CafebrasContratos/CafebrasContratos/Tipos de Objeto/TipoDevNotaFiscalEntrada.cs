using System;
using SAPHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class TipoDevNotaFiscalEntrada : TipoDeObjetoDeContrato
    {
        public int Tipo => (int)SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes;
        public string Nome => "Dev. Nota Fiscal de Entrada";
        public int IndexParaCombo => 5;
        public string Tabela => "ORPC";
        public TipoDeObjetoDeContrato ObjetoBase => new TipoNotaFiscalEntrada();
        public FormDocumentoMarketing Form => new FormDevNotaFiscalEntrada();
    }
}
