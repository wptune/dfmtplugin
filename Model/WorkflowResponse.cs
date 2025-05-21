using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftTranslatorProvider.Model
{
    public class WorkflowResponse
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        public Outputs Outputs { get; set; }
    }
    public class Outputs
    {
        public string Text { get; set; }
    }
}
