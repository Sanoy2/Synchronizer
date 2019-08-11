using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Library
{
    public class Step
    {
        private static int id = 0;

        public int Id { get; set; }
        public string SourceDirectory { get; set; }
        public string DestinationDirectory { get; set; }
        public string SourceName { get; set; }
        public string DestinationName{ get; set; }
        public string Options { get; set; }
        public bool IsEnabled { get; set; }

        public Step()
        {
            Id = ++id;
        }

        public override string ToString()
        {
            return $"Id: {Id}, {SourceName} -> {DestinationName}, {SourceDirectory}, Destination: {DestinationDirectory}, Options: {Options}";
        }
    }
}
