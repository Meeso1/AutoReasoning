
using System.Diagnostics.CodeAnalysis;
using Logic.Problem.Models;
using Action = Logic.Problem.Models.Action;


namespace Logic.Problem;
public sealed class ActionsParser
{

    /// <summary>
    /// 	Parses and validates actions
    /// </summary>
    /// <param name="actionStrings">
    /// 	A list of actions represented in string form.
    /// </param>
    /// <param name="actions">
    /// 	Parsed actions in a dict format, where key is action name and value is Action object
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        IReadOnlyList<string> actionStrings,
        [NotNullWhen(true)] out IReadOnlyDictionary<string, Action>? actions,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        throw new NotImplementedException();
    }
}
