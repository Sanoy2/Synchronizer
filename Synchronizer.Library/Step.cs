using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Library
{
    public class Step
    {
        public string Label { get; set; }
        public string SourceDirectory { get; set; }
        public string DestinationDirectory { get; set; }
        public string Options { get; set; }

        public override string ToString()
        {
            return $"{Label}: Source: {SourceDirectory} Destination: {DestinationDirectory} Options: {Options}";
        }
    }
}
