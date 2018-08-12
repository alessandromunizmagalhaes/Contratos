using SAPHelper;

namespace CafebrasContratos
{
    public class FormMetodoFinanceiro : FormCadastroBasico
    {
        public override string FormType { get { return "FormMetodoFinanceiro"; } }
        public override string mainDbDataSource { get { return new TabelaMetodoFinanceiro().NomeComArroba; } }
    }
}
