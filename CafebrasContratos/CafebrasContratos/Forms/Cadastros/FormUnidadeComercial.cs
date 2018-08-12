using SAPHelper;

namespace CafebrasContratos
{
    public class FormUnidadeComercial : FormCadastroBasico
    {
        public override string FormType { get { return "FormUnidadeComercial"; } }
        public override string mainDbDataSource { get { return new TabelaUnidadeComercial().NomeComArroba; } }
    }
}
