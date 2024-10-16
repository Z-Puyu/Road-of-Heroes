using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.util.errors {
    public class EventTypeMismatchException : Exception {
        public EventTypeMismatchException(Type expected, Type actual) 
            : base($"Event types mismatch during boxing of {expected}: " + 
                   $"got type parameter {actual} instead") {}
    }
}