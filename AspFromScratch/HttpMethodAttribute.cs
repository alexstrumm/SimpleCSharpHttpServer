using System;


namespace AspFromScratch {
    public class HttpMethodAttribute : Attribute {
        public string Method { get; set; }

        public HttpMethodAttribute(string method) {
            if (String.IsNullOrWhiteSpace(method)) {
                throw new ArgumentException("message", nameof(method));
            }

            this.Method = method;
        }
    }
}
