using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AspFromScratch.Guards;

namespace AspFromScratch {

    public delegate Task HttpDelegate(HttpListenerContext context, Dictionary<string, object> data);

    public class MiddlewareBuilder {
        private readonly Stack<Type> items = new Stack<Type>();
        private HttpDelegate firstMiddleware;
        public MiddlewareBuilder Use<T>() where T : IMiddleware {
            Guard.Against.TypeWithoutConstructorForTypes<T>(typeof(HttpDelegate));
            this.items.Push(typeof(T));
            return this;
        }
        public HttpDelegate Build() {
            this.firstMiddleware = async (context, data) => {
                context.Response.Close();
                await Task.CompletedTask;
            };
            while (this.items.Count > 0) {
                this.firstMiddleware = (Activator.CreateInstance(this.items.Pop(), this.firstMiddleware) as IMiddleware).InvokeAsync;
            }
            return this.firstMiddleware;
        }
    }
}
