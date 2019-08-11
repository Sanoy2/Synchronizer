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

        public StepsCollection(int? howMany = null)
        {
            steps = new List<Step>();
            if(howMany != null)
            {
                for (int i = 0; i < howMany; i++)
                {
                    var step = new Step()
                    {
                        SourceDirectory = $"C:\\tmp\\loc{i}",
                        DestinationDirectory = $"C:\\tmp\\loc{i + 1}",
                        SourceName = $"loc{i}",
                        DestinationName = $"loc{i + 1}"
                    };
                    steps.Add(step);
                }
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
