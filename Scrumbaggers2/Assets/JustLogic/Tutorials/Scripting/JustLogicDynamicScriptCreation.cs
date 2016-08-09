using JustLogic.Core;
using UnityEngine;

public class JustLogicDynamicScriptCreation : MonoBehaviour
{
    public Light Target;

    void Awake()
    {
        Library.Initialize(Application.isPlaying);
        // creating new JL Script dynamically
        var script = gameObject.AddComponent<JustLogicScript>();

        var engine = script.Engine;
        if (engine.Tree)
            Destroy(engine.Tree);

        // building actions fluently
        script.Engine.SetActions
        (
          JLAction.Build<JLToggle>(t => t.Object = JLExpression.Build<JLObjectValue>(v => v.Value = Target)),
          JLAction.Build<JLEvalute>(t => t.Expression = JLExpression.Build<JLPrintRet>(
              v => v.Value = JLExpression.Build<JLStringValue>(w => w.Value = "text1"))),
          JLAction.Build<JLEvalute>(t => t.Expression = JLExpression.Build<JLPrintRet>(
              v => v.Value = JLExpression.Build<JLStringValue>(w => w.Value = "text2")))
        );

        // launching the script from code
        //script.StartExecution();
        // or simulate event Awake
        //script.StartExecutionByEvent(new EventData(eventInfo: new JustLogic.Core.Events.Awake(),
        //    arguments: new object[0], handler: null, counter: 0));

        // or create event handler
        var handler = gameObject.AddComponent<JustLogicEventHandlerLite>();
        var data = handler.EventHandlerData;
        // specify event FixedUpdate
        data.EventDataClassFullName = typeof(JustLogic.Core.Events.FixedUpdate).FullName;
        // specify target script
        data.JLScriptsLookup = new[] { JLExpression.Build<JLObjectValue>(v => v.Value = script) };
        // initialize conditions
        data.Condition = JLExpression.Build<JLBoolValue>(v => v.Value = true);
    }
}
