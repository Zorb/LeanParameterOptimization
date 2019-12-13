using Jtc.Optimization.Objects.Interfaces;
using System.Collections.Generic;

namespace Jtc.Optimization.LeanOptimizer
{
    public interface IRunner
    {
        Dictionary<string, decimal> Run(Dictionary<string, object> items, IOptimizerConfiguration config);
    }
}