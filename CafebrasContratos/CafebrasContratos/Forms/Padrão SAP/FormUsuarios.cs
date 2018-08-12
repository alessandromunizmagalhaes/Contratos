using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormUsuarios : SAPHelper.Form
    {
        public override string FormType { get { return ((int)FormTypes.Usuarios).ToString(); } }
        private const string mainDbDataSource = "OUSR";

        #region :: Campos

        public ComboForm _grupoAprovador = new ComboForm()
        {
            ItemUID = "GrupoAprov",
            Datasource = "U_GrupoAprov"
        };
        public ItemForm _labelGrupoAprovador = new ItemForm()
        {
            ItemUID = "lblGrupo",
        };

        #endregion

        public override void OnBeforeFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;

                var itemRefUID = "18";
                var itemRef = form.Items.Item(itemRefUID);

                var itemLabelRefUID = "8";
                var itemLabelRef = form.Items.Item(itemLabelRefUID);

                var comboGrupoAprovador = form.Items.Add(_grupoAprovador.ItemUID, BoFormItemTypes.it_COMBO_BOX);

                comboGrupoAprovador.FromPane = 0;
                comboGrupoAprovador.ToPane = 0;

                int comboTop = itemRef.Top + 122;
                comboGrupoAprovador.Top = comboTop;
                comboGrupoAprovador.Left = itemRef.Left;
                comboGrupoAprovador.Width = itemRef.Width;
                comboGrupoAprovador.DisplayDesc = true;

                ((ComboBox)comboGrupoAprovador.Specific).DataBind.SetBound(true, mainDbDataSource, _grupoAprovador.Datasource);


                var labelGrupoAprovador = form.Items.Add(_labelGrupoAprovador.ItemUID, BoFormItemTypes.it_STATIC);

                labelGrupoAprovador.FromPane = 0;
                labelGrupoAprovador.ToPane = 0;
                labelGrupoAprovador.Top = comboTop;
                labelGrupoAprovador.Left = itemLabelRef.Left;
                labelGrupoAprovador.Width = itemLabelRef.Width;
                labelGrupoAprovador.LinkTo = comboGrupoAprovador.UniqueID;

                ((StaticText)labelGrupoAprovador.Specific).Caption = "Grupo Aprovador";
            }
        }
    }
}
