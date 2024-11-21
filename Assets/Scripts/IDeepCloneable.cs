public interface IDeepCloneable<out T> where T : class
{
    T DeepClone();
}