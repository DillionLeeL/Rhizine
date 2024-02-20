using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhizine.Tests.Models
{
    public interface ITestScript
    {
        string Name { get; }
        string Description { get; }
        List<string> Prerequisites { get; }
        TestScriptResult Execute();

        bool CanExecute();
    }
}
