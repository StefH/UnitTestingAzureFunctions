namespace CSharpOddOrEvenHttpTrigger
{
    public interface ITransientGreeter
    {
        string Greet();
    }

    public interface IScopedGreeter
    {
        string Greet();
    }

    public interface ISingletonGreeter
    {
        string Greet();
    }
}