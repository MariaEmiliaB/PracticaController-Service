namespace PracticaController_Service.Exceptions
{
    // La lanza el service cuando se intenta guardar un producto con un nombre que ya existe.
    // El service no sabe nada de HTTP: es el controller quien la traduce a 409 Conflict.
    public class DuplicateProductNameException : Exception
    {
        public DuplicateProductNameException(string name)
            : base($"Ya existe un producto con el nombre '{name}'.")
        {
        }
    }
}
