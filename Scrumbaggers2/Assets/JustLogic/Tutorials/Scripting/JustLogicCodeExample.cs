using System.Collections.Generic;
using JustLogic.Core;
using UnityEngine;
using System.Collections;

public class JustLogicCodeExample : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        // creating new JL Script dynamically
        var script = gameObject.AddComponent<JustLogicScript>();
        /*
        UnityEngine.Object ptLight = null;
        script.Engine.SetActions(
            JLAction.Build<JLToggle>(t => t.Object = JLExpression.Build<JLObjectValue>(v => v.Value = ptLight)),
            JLAction.Build<JLEvalute>(t => t.Expression = JLExpression.Build<JLPrintRet>(v => v.Value = JLExpression.Build<JLStringValue>(w => w.Value = "test"))),
            JLAction.Build<JLEvalute>(t => t.Expression = JLExpression.Build<JLPrintRet>(v => v.Value = JLExpression.Build<JLStringValue>(w => w.Value = "test2")))
            );
        */
        var engine = script.Engine;
        if (engine.Tree)
            Destroy(engine.Tree);
        engine.Tree = JLAction.Build<JLIf>(
        v =>
        {
            v.Value = JLExpression.Build<JLAnd>(a => a.Operands = new JLExpression[0]);
            v.Then = JLAction.Build<JLSequence>(
                b => b.Actions = new JLAction[]
                    {
                        JLAction.Build<JLEvalute>(
                        ev => ev.Expression=JLExpression.Build<JLPrintRet>
                            (p => p.Value = 
                                JLExpression.Build<JLStringValue>(s => s.Value = "Hello world2!")))
                    });
        });



        // launching the script from code
        script.StartExecution();
        // or simulate event Awake
        script.StartExecutionByEvent(new EventData(eventInfo: new JustLogic.Core.Events.Awake(),
            arguments: new object[0], handler: null, counter: 0));



        // creating new event handler from code
        // JustLogicEventHandlerLite - fast version
        // JustLogicEventHandlerMouse - slower, with mouse events 
        // JustLogicEventHandler - with mouse events, OnGUI, OnTap and OnKeyUp/Down
        var handler = gameObject.AddComponent<JustLogicEventHandlerLite>();
        var data = handler.EventHandlerData;
        // specify event FixedUpdate
        data.EventDataClassFullName = typeof(JustLogic.Core.Events.FixedUpdate).FullName;
        // specify target script
        data.JLScriptsLookup = new[] { JLExpression.Build<JLObjectValue>(v => v.Value = script) };
        // initialize conditions
        data.Condition = JLExpression.Build<JLBoolValue>(v => v.Value = true);


        // modifying the script
        // you can modify any JL script the same way
        // note that default internal structure JLIf-JLSequence can be changed
        // in the later versions
        JLSequence scriptSequence = ((JLSequence)((JLIf)engine.Tree).Then);
        var list = new List<JLAction>(scriptSequence.Actions);
        // we can't add expressions to the list
        // so we wrap the expression PrintRet with
        // the Evaluate action
        list.Add(JLAction.Build<JLEvalute>(
            ev => ev.Expression = JLExpression.Build<JLPrintRet>
                            (p => p.Value = JLExpression.Build<JLStringValue>(s => s.Value = "Hello world3!"))));

        list.Add(JLAction.Build<JLEvalute>(
            ev => ev.Expression = JLExpression.Build<JLPrintRet>
                            (p => p.Value = JLExpression.Build<JLStringValue>(s => s.Value = "Hello world4!"))));


        // don't forget to put the list back!
        scriptSequence.Actions = list.ToArray();

        script.StartExecution();
    }
}
