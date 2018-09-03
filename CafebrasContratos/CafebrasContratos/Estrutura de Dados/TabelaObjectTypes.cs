using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class TabelaObjectTypes : Tabela, ITabelaPopulavel
    {
        public Coluna ObjectType { get { return new ColunaVarchar("ObjType", "Tipo de Objeto", 30); } }
        public Coluna Tabela { get { return new ColunaVarchar("Tabela", "Tabela", 30); } }
        public Coluna Origem { get { return new ColunaInt("Origem", "Origem"); } }

        public List<ObjectTypesValues> data = new List<ObjectTypesValues>() {
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseOrders).ToString(),
              NomeTabela = "OPOR",
              Origem = OrigemDocMarketing.Nenhuma,
                // pedido de compra
            },
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseInvoices).ToString(),
              NomeTabela = "OPCH",
              Origem = OrigemDocMarketing.DebitaFiscal,
                // nota fiscal de entrada
            },
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseDeliveryNotes).ToString(),
              NomeTabela = "OPDN",
              Origem = OrigemDocMarketing.DebitaEstoque,
                // recebimento de mercadorias
            },
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseDownPayments).ToString(),
              NomeTabela = "ODPO",
              Origem = OrigemDocMarketing.CreditaFiscal,
                // adiantamento a fornecedor
            },
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseReturns).ToString(),
              NomeTabela = "ORPD",
              Origem = OrigemDocMarketing.CreditaEstoque,
                // devolução de mercadorias
            },
            new ObjectTypesValues(){
              ObjectType = ((int)BoObjectTypes.oPurchaseCreditNotes).ToString(),
              NomeTabela = "ORPC",
              Origem = OrigemDocMarketing.CreditaFiscal,
                // dev. nota fiscal de entrada
            },
        };

        public TabelaObjectTypes() : base("UPD_OBJ_TYPES", "Tipos de Objetos", BoUTBTableType.bott_NoObject)
        {
        }

        public void Popular()
        {
            using (var recordset = new RecordSet())
            {
                var insert =
                $@"INSERT INTO [{NomeComArroba}] 
                    (Code, Name, {ObjectType.NomeComU_NaFrente}, {Tabela.NomeComU_NaFrente}, {Origem.NomeComU_NaFrente})
                    VALUES ";

                var values = string.Empty;
                foreach (var item in data)
                {
                    values += $@",('{item.ObjectType}', '{item.ObjectType}', '{item.ObjectType}', '{item.NomeTabela}', {((int)item.Origem)})";
                }

                values = values.Remove(0, 1);
                recordset.DoQuery(insert + values);
            }
        }

        public class ObjectTypesValues
        {
            public string ObjectType { get; set; }
            public string NomeTabela { get; set; }
            public OrigemDocMarketing Origem { get; set; }

        }
    }

    public enum OrigemDocMarketing
    {
        DebitaEstoque = -1,
        DebitaFiscal = -2,
        Nenhuma = 0,
        CreditaEstoque = 1,
        CreditaFiscal = 2
    }
}
