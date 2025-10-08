using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Processamento
{
    public class Function
    {
        private readonly ILogger _logger;

        public Function(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function>();
        }

        // Visit https://aka.ms/sqltrigger to learn how to use this trigger binding
        [Function("Function")]
        public void Run(
            [SqlTrigger("[dbo].[table1]", "ConnectionStrings:PagamentosDb")] IReadOnlyList<SqlChange<ToDoItem>> changes,
                FunctionContext context)
        {
            _logger.LogInformation("SQL Changes: " + System.Text.Json.JsonSerializer.Serialize(changes));

        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
