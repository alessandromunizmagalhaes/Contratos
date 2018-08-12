namespace CafebrasContratos
{
    public class FormContratoFinalDetalheCertificado : FormDetalheCertificado
    {
        public override string FormType { get { return "FormContratoFinalDetalheCertificado"; } }
        public override string mainDbDataSource { get { return new TabelaCertificadosDoContratoFinal().NomeComArroba; } }
        public override FormContrato formPai { get { return new FormContratoFinal(); } }


        public override Matriz _matriz
        {
            get
            {
                return new Matriz()
                {
                    ItemUID = MatrixUID,
                    Datasource = mainDbDataSource
                };
            }
        }

        public override bool UsuarioPermitido()
        {
            return formPai.UsuarioPermitido();
        }
    }
}
