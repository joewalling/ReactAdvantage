using System;

namespace ReactAdvantage.Data.EntityFramework.GraphQlQueryModel
{
        public class GraphQlQueryModel
        {
            public string OperationName { get; set; }
            public string NamedQuery { get; set; }
            public string Query { get; set; }
            public Object Variables { get; set; }
        }
}
