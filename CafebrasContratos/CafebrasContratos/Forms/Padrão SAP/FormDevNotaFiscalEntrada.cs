using SAPHelper;
using SAPbouiCOM;
using System;

namespace CafebrasContratos
{
    public class FormDevNotaFiscalEntrada : FormDocumentoMarketing
    {
        public override string FormType { get { return ((int)FormTypes.DevNotaFiscalEntrada).ToString(); } }
        public override string mainDbDataSource { get { return "ORPC"; } }
        public override BoFormObjectEnum formEnum { get { return BoFormObjectEnum.fo_DeliveryNotesReturns; } }

        public override void OnAfterFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormLoad(FormUID, ref pVal, out BubbleEvent);

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;
                    var cardcode = dbdts.GetValue("CardCode",0).Trim();
                    if (!string.IsNullOrEmpty(cardcode) && form.Mode == BoFormMode.fm_ADD_MODE)
                    {
                        AtualizarCampoFalandoQueVeioDeContrato(form);
                        Dialogs.Success("Ok.");
                    }
                }
            }
        }
        
        public override SAPbouiCOM.Form Abrir(string codigo = "")
        {
            return AbrirNaMao("2309", codigo);
        }
    }
}
