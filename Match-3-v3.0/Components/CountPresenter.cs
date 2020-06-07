using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct CountPresenter
    {
        public string Title;
        public int Value;
        public CountPresenter(string title, int value)
        {
            Value = value;
            Title = title;
        }

        public override string ToString()
        {
            return $"{Title}: {Value}";
        }
    }
}
