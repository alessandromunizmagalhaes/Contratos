namespace CafebrasContratos
{
    public class FormPreContratoAberturaPorPeneira : FormAberturaPorPeneira
    {
        public override string FormType { get { return "FormPreContratoAberturaPorPeneira"; } }
        public override string mainDbDataSource { get { return new TabelaItensDoPreContrato().NomeComArroba; } }
        public override string fatherFormMainDbDataSource { get { return new TabelaPreContrato().NomeComArroba; } }

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
            return new FormPreContrato().UsuarioPermitido();
        }
    }
}
