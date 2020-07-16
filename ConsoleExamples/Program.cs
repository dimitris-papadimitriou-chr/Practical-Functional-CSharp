using System;

namespace PracticalCSharp
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var demo = new EitherTask.Monad.Example.Switch.Demo();
            await demo.Run();
        }
    }
}
