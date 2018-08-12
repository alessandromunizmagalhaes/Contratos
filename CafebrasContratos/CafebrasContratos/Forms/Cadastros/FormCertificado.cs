using SAPHelper;

namespace CafebrasContratos
{
    public class FormCertificado : FormCadastroBasico
    {
        public override string FormType { get { return "FormCertificado"; } }
        public override string mainDbDataSource { get { return new TabelaCertificado().NomeComArroba; } }
    }
}
