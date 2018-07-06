using System;

namespace Sample.Domain
{
    public class Item
    {
        public string Name { get; set; }

        public override string ToString() => this.Name;
    }
}
