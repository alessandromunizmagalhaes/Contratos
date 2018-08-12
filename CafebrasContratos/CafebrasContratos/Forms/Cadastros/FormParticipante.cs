using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormParticipante : SAPHelper.Form
    {
        public override string FormType { get { return "FormParticipante"; } }
        private string mainDbDataSource = new TabelaParticipante().NomeComArroba;

        #region :: Campos

        public ItemFormObrigatorio _codigo = new ItemFormObrigatorio()
        {
            ItemUID = "Code",
            Datasource = "Code",
            Mensagem = "O código é obrigatório",
        };
        public ItemFormObrigatorio _tipo = new ItemFormObrigatorio()
        {
            ItemUID = "Tipo",
            Datasource = "U_Tipo",
            Mensagem = "O tipo é obrigatório",
        };
        public ItemFormObrigatorio _nome = new ItemFormObrigatorio()
        {
            ItemUID = "Name",
            Datasource = "Name",
            Mensagem = "O nome é obrigatório",
        };

        #endregion


        #region :: Eventos de Formulário

        public override void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    string nextCode = GetNextCode(mainDbDataSource);
                    dbdts.SetValue(_codigo.ItemUID, 0, nextCode);

                    BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
                }
            }
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(BusinessObjectInfo.FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
                }
            }
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                _OnAdicionarNovo(form);
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            string nextCode = GetNextCode(mainDbDataSource);
            var dbdts = GetDBDatasource(form, mainDbDataSource);
            dbdts.SetValue(_codigo.ItemUID, 0, nextCode);
        }

        #endregion
    }
}
