using System;

namespace TestGame {
    [AttributeUsage(AttributeTargets.Class)]
    public class ExampleAttribute : Attribute {
        public string Title { get; private set; }

        public ExampleAttribute(string title) {
            Title = title;
        }
    }
}