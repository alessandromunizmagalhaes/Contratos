using SAPHelper;
namespace CafebrasContratos
{
    class PostTransactionNotice : IStoredProcedureImplementation
    {
        public string Codigo()
        {
            return
                $@"IF (SELECT count(*) FROM [@UPD_OBJ_TYPES] WHERE U_objtype = @object_type) > 0 AND @transaction_type = 'A'
                    BEGIN
		                DECLARE @Tabela VARCHAR(50) = (SELECT DBO.NOMETABELA(@object_type))
		                EXEC AtualizaCampoFalandoQueVeioDoContrato @DocEntry = @list_of_cols_val_tab_del, @Tabela = @Tabela
                        EXEC AtualizaSaldoContratoConformeDocMkt @DocEntry = @list_of_cols_val_tab_del, @ObjectType = @object_type
                    END";
        }
    }
}
