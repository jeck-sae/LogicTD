public class NotGate : LogicGate
{
    protected override bool CalculateOutput(bool[] inputs)
    {
        if (inputs.Length == 0) return true;
        return !inputs[0];
    }
}