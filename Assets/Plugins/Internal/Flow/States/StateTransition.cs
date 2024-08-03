using System;
using System.Collections.Generic;

namespace Internal.Flow.States
{
    public class StateTransition
    {
        public List<Func<bool>> Conditions { get; set; }
        public Type TargetType { get; set; }
    }
}