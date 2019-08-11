using Synchronizer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Start");
            var steps = new StepsCollection(true);
            var script = Script.BuildScript(steps);
            System.Console.WriteLine(script);

            var runner = new ScriptRunner();
            var output = runner.RunScript(script);
            System.Console.WriteLine(output);

            System.Console.WriteLine("Finished");
            System.Console.WriteLine("Press any key to continue..");
            System.Console.ReadKey();
        }
    }
}
