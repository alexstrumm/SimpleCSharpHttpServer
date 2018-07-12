using System;
using System.Linq;
using Ardalis.GuardClauses;

namespace AspFromScratch.Guards {
    public static class GuardConditionExtensionMethods {
        public static void TypeWithoutConstructorForTypes<T>(this IGuardClause guard, params Type[] types) {
            var argType = typeof(T);
            if (argType.GetConstructor(types) == null) {
                var name = argType.Name;
                var fullName = argType.FullName;
                var ctorParams = String.Join(", ", types.Select(t => t.Name));
                var exText = $"{fullName} shoud have constructor: public {name}({ctorParams}).";
                throw new ArgumentException(exText);
            }
        }
    }
}
