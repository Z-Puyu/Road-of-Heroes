using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.util.errors {
    public class PartialOrderException<S, T> : Exception {
        public PartialOrderException(S s, T t) : base($"{s} and {t} are not comparable!") {}
    }
}