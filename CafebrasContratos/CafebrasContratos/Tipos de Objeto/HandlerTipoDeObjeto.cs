using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafebrasContratos
{
    public class HandlerTipoDeObjeto
    {
        public readonly List<TipoDeObjetoDeContrato> _tiposDeObjetoDeContratos = new List<TipoDeObjetoDeContrato>()
        {
            new TipoPedidoDeCompra(),
            new TipoNotaFiscalEntrada(),
            new TipoRecebimentoDeMercadorias(),
            new TipoAdiantamentoAFornecedor(),
            new TipoDevolucaoDeMercadorias(),
            new TipoDevNotaFiscalEntrada(),
        };

        public TipoDeObjetoDeContrato GetByIndex(int index)
        {
            foreach (var tipo in _tiposDeObjetoDeContratos)
            {
                if (tipo.IndexParaCombo == index)
                {
                    return tipo;
                }
            }
            return null;
        }

        public TipoDeObjetoDeContrato GetByObjectType(int objectType)
        {
            foreach (var tipo in _tiposDeObjetoDeContratos)
            {
                if (tipo.Tipo == objectType)
                {
                    return tipo;
                }
            }
            return null;
        }
    }
}
