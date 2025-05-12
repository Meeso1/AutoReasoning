using Logic.Model.Models;
using Logic.Queries.Models;

namespace Logic.Queries;

public sealed class QueryParser(ModelDefinition model)
{
    public bool TryParse(string input, out Query query, out IReadOnlyList<string> errors)
    {
        throw new NotImplementedException();
    }   
}
