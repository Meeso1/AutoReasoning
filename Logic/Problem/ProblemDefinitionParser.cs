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
    public static ProblemDefinition CreateProblemDefinition(
        IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyList<ActionStatement> actionStatements,
        IReadOnlyList<Formula> initials,
        IReadOnlyList<Formula> always)
    {
        IReadOnlyDictionary<string, Action> actions = ProcessActionStatements(actionStatements);
        return CreateProblemDefinition(fluents, actions, initials, always);
    }

    public static ProblemDefinition CreateProblemDefinition(
        IReadOnlyDictionary<string, Fluent> fluents,
        IReadOnlyDictionary<string, Action> actions,
        IReadOnlyList<Formula> initials,
        IReadOnlyList<Formula> always)
        
    {
        StateGroup validStates = ProcessStatesFromFormulas(always);
        StateGroup initialStates = StateGroup.And(validStates, ProcessStatesFromFormulas(initials));
        return new ProblemDefinition
        {
            Fluents = fluents,
            Actions = actions,
            InitialStates = initialStates,
            ValidStates = validStates
        };
    }

    private static Dictionary<string, Action> ProcessActionStatements(IReadOnlyList<ActionStatement> actionStatements)
    {
        Dictionary<string, Action> actions = [];

        var groupedStatements = actionStatements
            .GroupBy(statement => statement.ActionName)
            .ToDictionary(group => group.Key, group => group.ToList());

        foreach (var group in groupedStatements)
        {
            string actionName = group.Key;
            var statements = group.Value;

            // Sort elements into appropriate lists
            List<ActionEffect> effects = [];
            List<ActionRelease> releases = [];
            List<ActionCondition> conditions = [];

            foreach (var statement in statements)
            {
                switch (statement.Element)
                {
                    case ActionEffect effect:
                        effects.Add(effect);
                        break;
                    case ActionRelease release:
                        releases.Add(release);
                        break;
                    case ActionCondition condition:
                        conditions.Add(condition);
                        break;
                }
            }

            // Create the Action object and add it to the dictionary
            actions[actionName] = new Action(
                actionName,
                effects,
                releases,
                conditions
            );
        }

        return actions;
    }

    private static StateGroup ProcessStatesFromFormulas(IReadOnlyList<Formula> always)
    {
        FormulaReducer formulaReducer = new();
        Formula finalFormula = new True();
        foreach (Formula formula in always) {
            finalFormula = new And(finalFormula, formula);
        }
        return formulaReducer.Reduce(finalFormula);
    }
}
