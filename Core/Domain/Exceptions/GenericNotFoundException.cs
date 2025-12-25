namespace Domain.Exceptions
{
    public class GenericNotFoundException<Tentity, Tkey>(object Value, string PropertyName) : NotFoundExceptionHandeling($"The {typeof(Tentity).Name} with {PropertyName} : {Value} not found")
    {
    }
}
