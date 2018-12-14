using System;

namespace CSharpOddOrEvenHttpTrigger
{
    internal class Greeter : ISingletonGreeter, ITransientGreeter, IScopedGreeter
    {
        private readonly Guid _id = Guid.NewGuid();

        public string Greet() => $"Hello World! Greeter Instance: {_id.ToString()}";
    }
}