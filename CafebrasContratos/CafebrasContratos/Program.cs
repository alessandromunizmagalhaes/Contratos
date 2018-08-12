using SAPbobsCOM;
using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{

    static class Program
    {
        private static string _addonName = "Cafébras Contratos";
        public static string _grupoAprovador;
        public static double _versaoAddon = 0.1;

        public static readonly List<Peneira> _peneiras = new List<Peneira>() { };
        public static readonly List<string> _gruposDeItensPermitidos = new List<string>() { };

        [STAThread]
        static void Main()
        {
            ConectarComSAP();

            CriarEstruturaDeDados();

            CriarMenus();

            DeclararEventos();

            Dialogs.Success(".:: " + _addonName + " ::. Iniciado", BoMessageTime.bmt_Medium);

            SetGrupoAprovador();

            CarregarPeneirasVindoDaConfiguracao();
            CarregarGruposDeItensPermitidos();

            // deixa a aplicação ativa
            System.Windows.Forms.Application.Run();
        }

        private static void ConectarComSAP()
        {
            try
            {
                SAPConnection.GetDICompany();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                System.Windows.Forms.Application.Exit();
            }
        }

        private static void CriarEstruturaDeDados()
        {
            Dialogs.Info(":: " + _addonName + " :: Verificando tabelas e estruturas de dados ...", BoMessageTime.bmt_Long);

            try
            {
                Global.Company.StartTransaction();

                using (Database db = new Database())
                {
                    var versoes = new List<Versionamento>() {
                        new Versao_Zero_Um(),
                    };

                    GerenciadorVersoes.Aplicar(db, versoes, _versaoAddon);
                }

                Global.Company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (DatabaseException e)
            {
                Dialogs.PopupError(e.Message);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao criar estrutura de dados.\nErro: " + e.Message);
                if (Global.Company.InTransaction)
                {
                    Global.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
        }

        private static void CriarMenus()
        {
            Dialogs.Info(":: " + _addonName + " :: Criando menus ...");

            try
            {
                RemoverMenu();

                Menu.CriarMenus(AppDomain.CurrentDomain.BaseDirectory + @"/criar_menus.xml");
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao inserir menus.\nErro: " + e.Message);
            }
        }

        private static void RemoverMenu()
        {
            Menu.RemoverMenus(AppDomain.CurrentDomain.BaseDirectory + @"/remover_menus.xml");
        }

        private static void DeclararEventos()
        {
            var eventFilters = new EventFilters();
            eventFilters.Add(BoEventTypes.et_MENU_CLICK);

            try
            {
                #region :: Forms Cadastro Básico

                var formGrupoDeItens = new FormGrupoDeItens();
                var formConfiguracaoPeneira = new FormConfiguracaoPeneira();
                var formCertificado = new FormCertificado();
                var formMetodoFinanceiro = new FormMetodoFinanceiro();
                var formModalidade = new FormModalidade();
                var formSafra = new FormSafra();
                var formTipoOperacao = new FormTipoOperacao();
                var formUnidadeComercial = new FormUnidadeComercial();
                var formParticipante = new FormParticipante();

                var formsCadastroBasico = new List<SAPHelper.Form>() {
                     formGrupoDeItens,
                     formConfiguracaoPeneira,
                     formCertificado,
                     formMetodoFinanceiro,
                     formModalidade,
                     formSafra,
                     formTipoOperacao,
                     formUnidadeComercial,
                     formParticipante
                };

                #endregion


                #region :: Forms Detalhe de Contrato

                var formPreContrato = new FormPreContrato();
                var formContratoFinal = new FormContratoFinal();

                var formPreContratoAberturaPorPeneira = new FormPreContratoAberturaPorPeneira();
                var formContratoFinalAberturaPorPeneira = new FormContratoFinalAberturaPorPeneira();

                var formPreContratoDetalheCertificado = new FormPreContratoDetalheCertificado();
                var formContratoFinalDetalheCertificado = new FormContratoFinalDetalheCertificado();

                var formPreContratoComissoes = new FormPreContratoComissoes();
                var formContratoFinalComissoes = new FormContratoFinalComissoes();

                var formsDetalheContrato = new List<SAPHelper.Form>() {
                     formPreContratoAberturaPorPeneira,
                     formContratoFinalAberturaPorPeneira,

                     formPreContratoDetalheCertificado,
                     formContratoFinalDetalheCertificado,

                     formPreContratoComissoes,
                     formContratoFinalComissoes
                };

                #endregion


                #region :: Form SAP
                var formUsuarios = new FormUsuarios();
                var formPedidoCompra = new FormPedidoCompra();

                var formsPadraoSAP = new List<SAPHelper.Form>()
                {
                    formUsuarios,
                    formPedidoCompra
                };

                #endregion


                #region :: Grupos de Forms

                var formsVisible = new List<SAPHelper.Form>() { formPreContrato, formContratoFinal };
                formsVisible.AddRange(formsCadastroBasico);
                formsVisible.AddRange(formsDetalheContrato);

                #endregion


                FormEvents.DeclararEventos(eventFilters, new List<MapEventsToForms>() {
                    new MapEventsToForms(BoEventTypes.et_FORM_VISIBLE, formsVisible),
                    new MapEventsToForms(BoEventTypes.et_FORM_LOAD, formsPadraoSAP),
                    new MapEventsToForms(BoEventTypes.et_COMBO_SELECT, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal,
                        formPreContratoComissoes,
                        formContratoFinalComissoes
                    }),
                    new MapEventsToForms(BoEventTypes.et_VALIDATE, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal
                    }),
                    new MapEventsToForms(BoEventTypes.et_CHOOSE_FROM_LIST, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formPreContratoAberturaPorPeneira,
                        formContratoFinal,
                        formContratoFinalAberturaPorPeneira
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_ADD, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal,
                        formCertificado,
                        formMetodoFinanceiro,
                        formModalidade,
                        formSafra,
                        formTipoOperacao,
                        formUnidadeComercial,
                        formParticipante
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_UPDATE, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal,
                        formCertificado,
                        formMetodoFinanceiro,
                        formModalidade,
                        formSafra,
                        formTipoOperacao,
                        formUnidadeComercial,
                        formParticipante
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_LOAD, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_CLOSE, new List<SAPHelper.Form>(){
                        formPreContratoDetalheCertificado,
                        formPreContratoComissoes,
                        formContratoFinalDetalheCertificado,
                        formContratoFinalComissoes,
                        formContratoFinal
                    }),
                    new MapEventsToForms(BoEventTypes.et_ITEM_PRESSED, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal,
                        formPreContratoAberturaPorPeneira,
                        formPreContratoDetalheCertificado,
                        formPreContratoComissoes,
                        formContratoFinalAberturaPorPeneira,
                        formContratoFinalDetalheCertificado,
                        formContratoFinalComissoes,
                        formGrupoDeItens,
                        formConfiguracaoPeneira,
                        formPedidoCompra
                    }),
                    new MapEventsToForms(BoEventTypes.et_MATRIX_LINK_PRESSED, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal
                    }),
                    new MapEventsToForms(BoEventTypes.et_DOUBLE_CLICK, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formContratoFinal
                    }),
                });

                var formsAdicionarNovo = new List<SAPHelper.Form>() { formPreContrato, formContratoFinal };
                formsAdicionarNovo.AddRange(formsCadastroBasico);

                FormEvents.DeclararEventosInternos(EventosInternos.AdicionarNovo, formsAdicionarNovo);
                FormEvents.DeclararEventosInternos(EventosInternos.Pesquisar, new List<SAPHelper.Form>(){
                    formPreContrato,
                    formContratoFinal
                });
                FormEvents.DeclararEventosInternos(EventosInternos.Duplicar, formPedidoCompra);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao declarar eventos de formulário.\nErro: " + e.Message);
            }

            try
            {
                Global.SBOApplication.SetFilter(eventFilters);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao setar eventos declarados da aplicação.\nErro: " + e.Message);
            }

            Global.SBOApplication.AppEvent += AppEvent;
            Global.SBOApplication.ItemEvent += FormEvents.ItemEvent;
            Global.SBOApplication.FormDataEvent += FormEvents.FormDataEvent;
            Global.SBOApplication.MenuEvent += Menu.MenuEvent;
        }


        #region :: Declaração Eventos

        private static void AppEvent(BoAppEventTypes EventType)
        {
            if (EventType == BoAppEventTypes.aet_ShutDown)
            {
                RemoverMenu();

                // remove a aplicação da memória
                System.Windows.Forms.Application.Exit();
            }
        }

        #endregion


        #region :: Regras de Negócio

        public static void SetGrupoAprovador()
        {
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery($"SELECT U_GrupoAprov FROM OUSR WHERE USER_CODE = '{Global.Company.UserName}'");
                _grupoAprovador = rs.Fields.Item("U_GrupoAprov").Value;
            }
        }

        public static void CarregarPeneirasVindoDaConfiguracao()
        {
            _peneiras.Clear();
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery("SELECT U_Peneira, U_NomeP, U_Ativo FROM [@UPD_CONF_PENEIRA]");
                while (!rs.EoF)
                {
                    string peneiraUID = rs.Fields.Item("U_Peneira").Value;
                    _peneiras.Add(new Peneira()
                    {
                        UID = rs.Fields.Item("U_Peneira").Value,
                        Nome = rs.Fields.Item("U_NomeP").Value,
                        Ativo = rs.Fields.Item("U_Ativo").Value == "Y",
                    });

                    rs.MoveNext();
                }
            }
        }

        public static void CarregarGruposDeItensPermitidos()
        {
            _gruposDeItensPermitidos.Clear();
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery("SELECT DISTINCT U_ItmsGrpCod FROM [@UPD_OCTC]");
                while (!rs.EoF)
                {
                    string grupoDeItem = rs.Fields.Item("U_ItmsGrpCod").Value;
                    _gruposDeItensPermitidos.Add(grupoDeItem);
                    rs.MoveNext();
                }
            }
        }

        #endregion

    }
}


