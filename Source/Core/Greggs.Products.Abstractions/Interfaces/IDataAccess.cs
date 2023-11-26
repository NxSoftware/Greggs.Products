namespace Greggs.Products.Abstractions.Interfaces;

public interface IDataAccess<out T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);
}