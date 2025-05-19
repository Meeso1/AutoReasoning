using System.Diagnostics.CodeAnalysis;
using Logic.Problem.Models;
using Logic.States.Models;
using Logic.States;
using System;

namespace Logic.Problem;

using Action = Models.Action;
/// <summary>
/// 	Class that parses and validates problem definitions
/// </summary>
public sealed class ProblemDefinitionParser
{
    /// <summary>
    /// 	Parses and validates a problem definition
    /// </summary>
    /// <param name="fluents">
    /// 	A dict of fluents, fluent name for key and Fluent object for value
    /// </param>
    /// <param name="actionStrings">
    /// 	A list of actions represented in string form.
    /// </param>
    /// <param name="initials">
    /// 	A dict of fluents, specifying their intital value. If Fluent is not present in dict it means its unspecified.
    /// </param>
    /// <param name="always">
    /// 	A list of formulas that must be met in every state. Will be processed into validStates
    /// </param>
    /// <param name="problem">
    /// 	Parsed problem definition if parsing was successful, null otherwise
    /// </param>
    /// <param name="errors">
    /// 	Errors that occurred during parsing, or null if parsing was successful
    /// </param>
    /// <returns>
    /// 	True if parsing was successful, false otherwise
    /// </returns>
    public bool TryParse(
        IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyList<string> actionStrings,
        IReadOnlyDictionary<Fluent, bool> initials,
        IReadOnlyList<Formula> always,
        [NotNullWhen(true)] out ProblemDefinition? problem,
        [NotNullWhen(false)] out IReadOnlyList<string>? errors)
    {
        ActionsParser actionParser = new();
        if (actionParser.TryParse(actionStrings, out IReadOnlyDictionary<string, Action>? actions, out IReadOnlyList<string>? actionErrors))
        {
            problem = CreateProblemDefinition(fluents, actions, initials, always);
            errors = null;
            return true;
        }
        problem = null;
        errors = actionErrors;
        return false;
    }

    private ProblemDefinition CreateProblemDefinition(
        IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyDictionary<string, Action> actions,
        IReadOnlyDictionary<Fluent, bool> initials,
        IReadOnlyList<Formula> always)
        
    {
        StateGroup validStates = ProcessValidStates(always);
        StateGroup initialStates = ProcessInitialStates(validStates, initials);
        return new ProblemDefinition
        {
            Fluents = fluents,
            Actions = actions,
            InitialStates = initialStates,
            ValidStates = validStates
        };
    }

    private static StateGroup ProcessValidStates(IReadOnlyList<Formula> always)
    {
        FormulaReducer formulaReducer = new();
        Formula finalFormula = new True();
        foreach (Formula formula in always) {
            finalFormula = new And(finalFormula, formula);
        }
        return formulaReducer.Reduce(finalFormula);
    }

    private static StateGroup ProcessInitialStates(StateGroup validStates, IReadOnlyDictionary<Fluent, bool> initials)
    {
        List<Dictionary<Fluent, bool>> initialStates = [];

        foreach (IReadOnlyDictionary<Fluent, bool> state in validStates.SpecifiedFluentGroups)
        {
            initialStates.AddRange(AndMergeStrategy.Merge(state, initials));
        }
        return new StateGroup(initialStates);
    }

}
