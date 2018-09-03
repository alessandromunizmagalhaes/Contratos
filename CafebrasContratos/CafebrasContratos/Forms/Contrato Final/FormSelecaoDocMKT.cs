
using SAPbouiCOM;
using SAPHelper;
using System;
using System.Xml;

namespace CafebrasContratos
{
    public class FormSelecaoDocMKT : SAPHelper.Form
    {
        public override string FormType { get { return "FormSelecaoDocMKT"; } }
        private const string SRF = "FormSelecaoDocMKT.srf";
        public static string _fatherFormUID = "";

        public ItemForm _numeroContratoFinal = new ItemForm() {
            Datasource = "U_DocNumCF",
            ItemUID = "U_DocNumCF"
        };
        public MatrizDTDocumentos _matrizDocumentos = new MatrizDTDocumentos()
        {
            ItemUID = "matrix",
            Datasource = "Documentos"
        };
        public ButtonForm _botaoCopiar = new ButtonForm()
        {
            ItemUID = "btnCopy"
        };

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var numContratoFinal = string.Empty;
            using (var formFatherCOM = new FormCOM(_fatherFormUID))
            {
                var formFather = formFatherCOM.Form;
                var dbdtsFather = GetDBDatasource(formFather, formFather.DataSources.DBDataSources.Item(0).TableName);
                numContratoFinal = dbdtsFather.GetValue("U_DocNumCF", 0).Trim();
            }
            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;

                if (!string.IsNullOrEmpty(numContratoFinal))
                {
                    _numeroContratoFinal.SetValorUserDataSource(form, numContratoFinal);
                    var mtx = GetMatrix(form, _matrizDocumentos.ItemUID);
                    _matrizDocumentos.Bind(mtx);

                    AtualizarMatriz(form);
                }
                else
                {
                    Dialogs.PopupError("Erro interno. Não foi encontrado o código do contrato final. Este form será fechado.");
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ItemUID == _botaoCopiar.ItemUID)
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;

                    var mtx = GetMatrix(form, _matrizDocumentos.ItemUID);
                    int row = mtx.GetNextSelectedRow();
                    if (row > 0)
                    {
                        var objBase = GetObjetoDeContrato(form);
                        var codigoDocSelecionado = mtx.GetCellSpecific(_matrizDocumentos.Codigo.ItemUID, row).Value;
                        var formBase = objBase.Form.Abrir(codigoDocSelecionado);
                        Dialogs.Info("Buscando informações...");
                        objBase.Form.CopiarPara(formBase);
                        form.Close();
                    }
                    else
                    {
                        Dialogs.PopupError("Selecione uma linha da matriz para escolher um documento base.");
                    }
                } 
            }
        }

        public static void AbrirForm(string fatherFormUID, TipoDeObjetoDeContrato tipoDeObjetoDeContrato)
        {
            _fatherFormUID = fatherFormUID;
            FormCreationParams creationPackage = Global.SBOApplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            var oXMLDoc = new XmlDocument();
            oXMLDoc.Load(AppDomain.CurrentDomain.BaseDirectory + SRF);
            creationPackage.XmlData = oXMLDoc.InnerXml;
            creationPackage.UniqueID = Guid.NewGuid().ToString("N");
            SAPbouiCOM.Form oForm = Global.SBOApplication.Forms.AddEx(creationPackage);

            var userDataSource = oForm.DataSources.UserDataSources.Add("ObjectType", BoDataType.dt_SHORT_NUMBER);
            userDataSource.Value = tipoDeObjetoDeContrato.Tipo.ToString();

            oForm.Visible = true;
        }
        
        public void AtualizarMatriz(SAPbouiCOM.Form form)
        {
            var dt = GetDatatable(form, _matrizDocumentos.Datasource);

            var numeroContrato = form.DataSources.UserDataSources.Item(_numeroContratoFinal.Datasource).Value;

            try
            {
                form.Freeze(true);

                var sql = string.Empty;
                var tabela = GetObjetoDeContrato(form).Tabela;
                
                sql += $@"SELECT 
	                ObjType as Tipo
	                , tb0.DocEntry
	                , tb0.DocDate
	                , tb1.ItemCode
	                , tb1.Dscription
	                , tb1.Quantity
	                , tb0.DocTotal 
                FROM {tabela} tb0
                INNER JOIN (
	                SELECT 
		                DocEntry, ItemCode, Dscription, Quantity, ROW_NUMBER() OVER ( PARTITION BY DocEntry ORDER BY DocEntry ) as count
	                FROM {tabela.Substring(1, 3) + "1"}
                )  tb1 ON (tb1.DocEntry = tb0.DocEntry AND tb1.count = 1)
                WHERE 1 = 1 
	                AND tb0.U_DocNumCF = '{numeroContrato}'
                    AND tb0.DocStatus = 'O'
                ";

                dt.ExecuteQuery(sql);

                var mtx = GetMatrix(form, _matrizDocumentos.ItemUID);
                mtx.LoadFromDataSource();

                mtx.AutoResizeColumns();
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public TipoDeObjetoDeContrato GetObjetoDeContrato(SAPbouiCOM.Form form)
        {
            var objecttype = form.DataSources.UserDataSources.Item("ObjectType").Value;
            return new HandlerTipoDeObjeto().GetByObjectType(Int32.Parse(objecttype));
        }
        
        public class MatrizDTDocumentos : MatrizDatatable
        {
            public ItemForm TipoDocumento = new ItemForm()
            {
                ItemUID = "tipo",
                Datasource = "Tipo"
            };
            public ItemForm Codigo = new ItemForm()
            {
                ItemUID = "docentry",
                Datasource = "DocEntry"
            };
            public ItemForm Data = new ItemForm()
            {
                ItemUID = "docdate",
                Datasource = "DocDate"
            };
            public ItemForm CodigoItem = new ItemForm()
            {
                ItemUID = "itemcode",
                Datasource = "ItemCode"
            };
            public ItemForm NomeItem = new ItemForm()
            {
                ItemUID = "itemname",
                Datasource = "ItemName"
            };
            public ItemForm ValorTotal = new ItemForm()
            {
                ItemUID = "doctotal",
                Datasource = "DocTotal"
            };
        }

    }
}
