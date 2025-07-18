using UnityEngine;

public class Connection : MonoBehaviour
{
    public LogicGate fromGate;
    public LogicGate toGate;

    public void Connect()
    {
        if (toGate != null && fromGate != null)
        {
            toGate.AddInput(fromGate);
        }
    }
}
