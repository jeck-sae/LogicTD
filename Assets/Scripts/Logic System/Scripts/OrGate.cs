public class OrGate : LogicGate
{
    protected override bool CalculateOutput(bool[] inputs)
    {
        foreach (bool b in inputs)
        {
            if (b) return true;
        }
        return false;
    }
}
