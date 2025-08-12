public class AndGate : LogicGate
{
    protected override bool CalculateOutput(bool[] inputs)
    {
        foreach (bool b in inputs)
        {
            if (!b) return false;
        }
        return true;
    }
}