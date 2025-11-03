namespace Domain.Exceptions
{
    public class GenericNotFoundException<Tentity, Tkey>(object Value) : NotFoundExceptionHandeling($"The {typeof(Tentity).Name} with {typeof(Tkey).Name} : {Value} not found")
    {
    }
}
