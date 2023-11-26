namespace Greggs.Products.Database.DataAccess;

public interface IDataAccess<out T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);
}