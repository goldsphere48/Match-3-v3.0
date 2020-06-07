using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct Text
    {
        private string _text;

        public Text(string text)
        {
            _text = text;
        }

        public string GetText()
        {
            return _text;
        }
    }
}
