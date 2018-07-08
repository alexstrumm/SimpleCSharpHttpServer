using System;
using Ardalis.GuardClauses;

namespace AspFromScratch.Guards {
    public static class GuardConditionExtensionMethods {
        public static void TypeWithoutConstructorForTypes<T>(this IGuardClause guard, params Type[] types) {
            if (typeof(T).GetConstructor(types) == null) {
                //TODO: define and use custom exception class
                throw new Exception();
            }
        }
    }
}
