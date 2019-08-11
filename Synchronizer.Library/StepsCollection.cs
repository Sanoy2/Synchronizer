using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Library
{
    public class StepsCollection
    {
        private List<Step> steps;
        public IList<Step> Steps { get => steps; }

        public StepsCollection(bool mock = false)
        {
            steps = new List<Step>();
            if(mock)
            {
                var step = new Step()
                {
                    Label = "mock",
                    SourceDirectory = "C:\\tmp\\loc1",
                    DestinationDirectory = "C:\\tmp\\loc2",
                    Options = "*.txt *.xml /xo /s /xl"
                };
                steps.Add(step) ;
            }
        }

        public void AddNew(Step step)
        {
            steps.Add(step);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Steps:"); ;
            foreach(var step in steps)
            {
                builder.AppendLine(step.ToString());
            }
            return builder.ToString();
        }
    }
}
