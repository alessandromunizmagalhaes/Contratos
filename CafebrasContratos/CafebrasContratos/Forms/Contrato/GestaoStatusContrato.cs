namespace CafebrasContratos
{
    public class GestaoStatusContrato
    {
        public string StatusPersistente { get; set; }
        public string StatusVolatil { get; set; }

        public GestaoStatusContrato(string statusPersistente, string statusVolatil)
        {
            StatusPersistente = statusPersistente;
            StatusVolatil = statusVolatil;
        }

        public bool EstaTrocandoStatus()
        {
            return StatusVolatil != StatusPersistente;
        }
    }
}
