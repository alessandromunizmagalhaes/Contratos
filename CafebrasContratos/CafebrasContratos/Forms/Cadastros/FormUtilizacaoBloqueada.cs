using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormUtilizacaoBloqueada : SAPHelper.Form
    {
        public override string FormType { get { return "FormUtilizacaoBloqueada"; } }
        public string mainDbDataSource = new TabelaUtilizacaoBloqueada().NomeComArroba;

        #region :: Campos

        public Matriz _matriz = new Matriz()
        {
            ItemUID = "matrix",
            Datasource = new TabelaUtilizacaoBloqueada().NomeComArroba
        };
        public ButtonForm _adicionar = new ButtonForm()
        {
            ItemUID = "btnAdd"
        };
        public ButtonForm _remover = new ButtonForm()
        {
            ItemUID = "btnRmv"
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
                _matriz._utilizacao.Popular(mtx);

                using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                {
                    var dbdts = dbdtsCOM.Dbdts;
                    dbdts.Clear();

                    using (var recordset = new RecordSet())
                    {
                        var rs = recordset.DoQuery($"SELECT U_Usage FROM [{mainDbDataSource}] ORDER BY CONVERT(INT,Code)");
                        if (rs.RecordCount > 0)
                        {
                            while (!rs.EoF)
                            {
                                var utilizacao = rs.Fields.Item(_matriz._utilizacao.Datasource).Value;
                                dbdts.InsertRecord(dbdts.Size);
                                dbdts.SetValue(_matriz._utilizacao.Datasource, dbdts.Size - 1, utilizacao);
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
            }
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _salvar.ItemUID)
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                    mtx.FlushToDataSource();
                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;

                        if (!CamposMatrizEstaoValidos(form, dbdts, _matriz))
                        {
                            BubbleEvent = false;
                        }
                    }
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _adicionar.ItemUID)
            {
                OnAdicionarLinha(FormUID);
            }
            else if (pVal.ItemUID == _remover.ItemUID)
            {
                OnRemoverLinha(FormUID);
            }
            else if (pVal.ItemUID == _salvar.ItemUID)
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
                    Global.Company.StartTransaction();
                    bool ok = true;

                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        var dbdts = dbdtsCOM.Dbdts;

                        using (var recordset = new RecordSet())
                        {
                            var rs = recordset.DoQuery($"DELETE FROM [{dbdts.TableName}];");
                            var userTable = Global.Company.UserTables.Item(dbdts.TableName.Remove(0, 1));

                            var mtx = GetMatrix(form, _matriz.ItemUID);
                            mtx.FlushToDataSource();
                            var utilizacaoDataSource = _matriz._utilizacao.Datasource;

                            for (int i = 0; i < dbdts.Size; i++)
                            {
                                var utilizacao = dbdts.GetValue(utilizacaoDataSource, i);
                                if (!string.IsNullOrEmpty(utilizacao))
                                {
                                    var codigo = (i + 1).ToString();
                                    userTable.Code = codigo;
                                    userTable.Name = codigo;
                                    userTable.UserFields.Fields.Item(utilizacaoDataSource).Value = utilizacao;

                                    if (userTable.Add() != 0)
                                    {
                                        ok = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!ok)
                    {
                        Dialogs.PopupError("Erro ao salvar dados.\nErro: " + Global.Company.GetLastErrorDescription());
                    }
                    else
                    {
                        Global.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                        Dialogs.PopupSuccess("Dados salvos com sucesso.");
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
            public ComboFormObrigatorio _utilizacao = new ComboFormObrigatorio()
            {
                ItemUID = "Usage",
                Datasource = "U_Usage",
                SQL = "SELECT ID, Usage FROM OUSG ORDER BY Usage",
                Mensagem = "A Utilização é obrigatória."
            };
        }

        #endregion
    }
}
