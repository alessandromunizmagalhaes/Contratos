using SAPbobsCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class TabelaConfiguracaoPeneira : Tabela, ITabelaPopulavel
    {
        public Coluna Peneira { get { return new ColunaVarchar("Peneira", "Peneira", 30); } }
        public ColunaAtivo Ativo { get { return new ColunaAtivo(); } }
        public Coluna NomePeneira { get { return new ColunaVarchar("NomeP", "Descrição Peneira", 30); } }
        public Coluna NomeDiferencial { get { return new ColunaVarchar("NomeD", "Descrição Diferencial", 30); } }

        public TabelaConfiguracaoPeneira() : base("UPD_CONF_PENEIRA", "Configuração Peneira", BoUTBTableType.bott_NoObject)
        {
        }

        public void Popular()
        {
            using (var recordset = new RecordSet())
            {
                var insert =
                $@"INSERT INTO [{NomeComArroba}] 
                    (Code, Name, {Peneira.NomeComU_NaFrente}, {Ativo.NomeComU_NaFrente}, {NomePeneira.NomeComU_NaFrente}, {NomeDiferencial.NomeComU_NaFrente})
                    VALUES ";

                var values = string.Empty;
                for (int i = 1; i <= 15; i++)
                {
                    var count = i.ToString().PadLeft(2, '0');
                    var peneira = "P" + count;
                    var diferencial = "D" + count;
                    values += $@",('{i}', '{i}', '{peneira}', 'Y', '{peneira}', '{diferencial}')";
                }

                values = values.Remove(0, 1);
                recordset.DoQuery(insert + values);
            }
        }
    }
}
