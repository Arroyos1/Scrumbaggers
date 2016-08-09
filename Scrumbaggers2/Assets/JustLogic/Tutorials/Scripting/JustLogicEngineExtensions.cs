using System.Collections.Generic;
using JustLogic.Core;

public static class JustLogicExecutionEngineExtensions
{
    public static JLAction[] GetActions(this ExecutionEngine engine)
    {
        return ((JLSequence)((JLIf)engine.Tree).Then).Actions;
    }

    public static void SetActions(this ExecutionEngine engine, params JLAction[] actions)
    {
        if (!engine.Tree)
            engine.Tree = JLAction.Build<JLIf>(
                v =>
                {
                    v.Value = JLExpression.Build<JLAnd>(a => a.Operands = new JLExpression[0]);
                    v.Then = JLAction.Build<JLSequence>();
                });

        ((JLSequence)((JLIf)engine.Tree).Then).Actions = actions.ToArray2();
    }

    public static void AddActions(this ExecutionEngine engine, params JLAction[] actions)
    {
        var lst = new List<JLAction>(GetActions(engine));
        lst.AddRange(actions);
        SetActions(engine, lst.ToArray());
    }

    public static void InsertActions(this ExecutionEngine engine, int index, params JLAction[] actions)
    {
        var lst = new List<JLAction>(GetActions(engine));
        lst.InsertRange(index, actions);
        SetActions(engine, lst.ToArray());
    }

    public static JLExpression[] GetConditions(this ExecutionEngine engine)
    {
        return ((JLAnd)(((JLIf)engine.Tree).Value)).Operands;
    }

    public static void SetConditions(this ExecutionEngine engine, params JLExpression[] boolExpressions)
    {
        ((JLAnd)(((JLIf)engine.Tree).Value)).Operands = boolExpressions.ToArray2();
    }

    public static void AddConditions(this ExecutionEngine engine, params JLExpression[] boolExpressions)
    {
        var lst = new List<JLExpression>(GetConditions(engine));
        lst.AddRange(boolExpressions);
        SetConditions(engine, lst.ToArray());
    }

    public static void InsertConditions(this ExecutionEngine engine, int index, params JLExpression[] boolExpressions)
    {
        var lst = new List<JLExpression>(GetConditions(engine));
        lst.InsertRange(index, boolExpressions);
        SetConditions(engine, lst.ToArray());
    }
}
