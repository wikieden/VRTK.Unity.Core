namespace VRTK.Core.Rule
{
    public interface IRule
    {
        bool Accepts(object target);
    }
}