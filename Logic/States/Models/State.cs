using System.Diagnostics;

namespace Logic.States.Models;

/// <summary>
/// 	Single state specifying all fluent values
/// </summary>
/// <param name="FluentValues">Dictionary containing all fluent values</param>
public sealed record State(IReadOnlyDictionary<Fluent, bool> FluentValues);

/// <summary>
/// 	Group of states, specified by a list of possible parial fluent value specifications
/// </summary>
/// <param name="SpecifiedFluentGroups">List of dictionaries, each specifying some of the fluent values</param>
/// <remarks>
/// 	The state is contained in the group, if there exists a dictionary in the list, 
/// 	for which all fluent values that it specifies match fluent values in the state
/// </remarks>
public sealed record StateGroup(IReadOnlyList<IReadOnlyDictionary<Fluent, bool>> SpecifiedFluentGroups)
{
	public bool Contains(State state)
	{
		return SpecifiedFluentGroups.Any(group => IsSubsetOf(group, state.FluentValues));
	}

	private static bool IsSubsetOf(
		IReadOnlyDictionary<Fluent, bool> specifiedFluents, 
		IReadOnlyDictionary<Fluent, bool> state)
	{
		foreach (var key in specifiedFluents.Keys)
		{
			if (!state.ContainsKey(key))
			{
				throw new UnreachableException("State does not specify one of the fluents");
			}

			if (specifiedFluents[key] != state[key])
			{
				return false;
			}
		}

		return true;
	}
}
