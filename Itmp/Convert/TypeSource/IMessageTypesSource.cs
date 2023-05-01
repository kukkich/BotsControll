namespace Itmp.Convert.TypeSource;

public interface IMessageTypesSource
{
    public IEnumerable<Type> GetTypes(); 
}