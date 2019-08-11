using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Library
{
    public static class Script
    {
        private static string tool = "robocopy";

        public static string BuildScript(StepsCollection steps)
        {
            var builder = new StringBuilder();
            foreach(var step in steps.Steps)
            {
                builder.AppendLine(BuildCommand(step));
            }
            return builder.ToString();
        }

        private static string BuildCommand(Step step)
        {
            if(step.IsEnabled)
            {
                return $"{tool} {step.SourceDirectory} {step.DestinationDirectory} {step.Options}";
            }
            else
            {
                return $"echo Step {step.SourceName} -> {step.DestinationName} is disabled";
            }
        }
    }
}
