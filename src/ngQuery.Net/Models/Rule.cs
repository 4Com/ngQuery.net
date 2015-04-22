using System;

namespace ngQuery.Net.Models
{
    internal class Rule : IRule
    {
        public string SelectedEntry { get; set; }

        public string SelectedField { get; set; }

        public string SelectedOperator { get; set; }
    }
}
