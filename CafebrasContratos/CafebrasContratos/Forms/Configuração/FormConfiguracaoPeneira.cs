using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormConfiguracaoPeneira : SAPHelper.Form
    {
        public override string FormType { get { return "FormConfiguracaoPeneira"; } }
        public string mainDbDataSource = new TabelaConfiguracaoPeneira().NomeComArroba;

        #region :: Campos

        public Matriz _matriz = new Matriz()
        {
            ItemUID = "matrix",
            Datasource = new TabelaConfiguracaoPeneira().NomeComArroba
        };
        public ButtonForm _salvar = new ButtonForm()
        {
            ItemUID = "btnSalvar"
        };

        #endregion


        #region :: Eventos de Itens

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                var mtx = GetMatrix(form, _matriz.ItemUID);
                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;

                    PreencherDadosMatriz(form, mtx, dbdts);
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _salvar.ItemUID)
            {
                OnSalvar(FormUID);
            }
        }

        private void OnSalvar(string formUID)
        {
            using (var formCOM = new FormCOM(formUID))
            {
                var form = formCOM.Form;
                try
                {
                    form.Freeze(true);
                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;
                        var mtx = GetMatrix(form, _matriz.ItemUID);

                        Salvar(form, dbdts, mtx);

                        Program.CarregarPeneirasVindoDaConfiguracao();
                    }
                }
                catch (Exception e)
                {
                    Dialogs.PopupError("Erro interno. Erro ao salvar dados.\nErro: " + e.Message);
                    Global.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                finally
                {
                    form.Freeze(false);
                }
            }
        }

        private void OnRemoverLinha(string FormUID)
        {
            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                _matriz.RemoverLinha(form);
            }
        }

        private void OnAdicionarLinha(string FormUID)
        {
            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;
                _matriz.AdicionarLinha(form);
            }
        }

        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizMasterDataForm
        {
            public ItemForm _peneira = new ItemForm()
            {
                ItemUID = "Peneira",
                Datasource = "U_Peneira",
            };
            public ItemForm _nomepeneira = new ItemForm()
            {
                ItemUID = "NomeP",
                Datasource = "U_NomeP",
            };
            public ComboAtivoForm _ativo = new ComboAtivoForm();
        }

        #endregion


        #region :: Regras de Negócio

        private void PreencherDadosMatriz(SAPbouiCOM.Form form, Matrix mtx, DBDataSource dbdts)
        {
            dbdts.Clear();

            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery("SELECT * FROM [@UPD_CONF_PENEIRA] ORDER BY CONVERT(INT,Code)");
                if (rs.RecordCount > 0)
                {
                    while (!rs.EoF)
                    {
                        var code = rs.Fields.Item("Code").Value;
                        var peneira = rs.Fields.Item(_matriz._peneira.Datasource).Value;
                        var nomePeneira = rs.Fields.Item(_matriz._nomepeneira.Datasource).Value;
                        var ativo = rs.Fields.Item(_matriz._ativo.Datasource).Value;

                        dbdts.InsertRecord(dbdts.Size);

                        int row = dbdts.Size - 1;

                        dbdts.SetValue("Code", row, code);
                        dbdts.SetValue(_matriz._peneira.Datasource, row, peneira);
                        dbdts.SetValue(_matriz._nomepeneira.Datasource, row, nomePeneira);
                        dbdts.SetValue(_matriz._ativo.Datasource, row, ativo);

                        rs.MoveNext();
                    }

                    mtx.LoadFromDataSourceEx();
                }
                else
                {
                    _matriz.AdicionarLinha(form);
                }
            }
        }

        private void Salvar(SAPbouiCOM.Form form, DBDataSource dbdts, Matrix mtx)
        {
            Global.Company.StartTransaction();

            var userTable = Global.Company.UserTables.Item(dbdts.TableName.Remove(0, 1));

            mtx.FlushToDataSource();
            var peneiraDataSource = _matriz._peneira.Datasource;
            var nomePeneiraDataSource = _matriz._nomepeneira.Datasource;
            var ativoDataSource = _matriz._ativo.Datasource;
            bool ok = true;
            for (int i = 0; i < dbdts.Size; i++)
            {
                var code = dbdts.GetValue("Code", i);

                if (userTable.GetByKey(code))
                {
                    var peneira = dbdts.GetValue(peneiraDataSource, i);
                    var nomePeneira = dbdts.GetValue(nomePeneiraDataSource, i);
                    var ativo = dbdts.GetValue(ativoDataSource, i);

                    userTable.UserFields.Fields.Item(peneiraDataSource).Value = peneira;
                    userTable.UserFields.Fields.Item(nomePeneiraDataSource).Value = nomePeneira;
                    userTable.UserFields.Fields.Item(ativoDataSource).Value = ativo;

                    if (userTable.Update() != 0)
                    {
                        ok = false;
                        break;
                    }
                }
                else
                {
                    throw new Exception($"Erro interno.\nCódigo {code} não encontrado.");
                }
            }

            if (ok)
            {
                Global.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                Dialogs.PopupSuccess("Dados salvos com sucesso.");
            }
            else
            {
                Dialogs.PopupError("Erro ao salvar dados.\nErro: " + Global.Company.GetLastErrorDescription());
            }
        }

        #endregion
    }
}
