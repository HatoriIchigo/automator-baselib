using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace automator_baselib
{
    public class Input
    {
        public class _Input
        {
            public string type { get; set; }
            public string content { get; set; }
        }

        public string type { get; set; }
        public string content { get; set; }


        public Input(string s)
        {
            if (s == null || s == "")
            {
                this.type = "";
                this.content = "";
            } else {
                var jsonObject = JsonNode.Parse(s)?.AsObject();
                if (jsonObject != null)
                {
                    this.type = jsonObject["type"]?.ToString();
                    this.content = jsonObject["content"]?.ToString();
                }
            }
        }
    }
}
