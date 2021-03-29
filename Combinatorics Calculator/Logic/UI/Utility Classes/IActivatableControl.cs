using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics_Calculator.Logic.UI.Utility_Classes
{
    /// <summary>
    /// IActivatableControl - all controls that should be triggered after loading
    /// </summary>
    public interface IActivatableControl
    {
        void Activate();
    }
}
