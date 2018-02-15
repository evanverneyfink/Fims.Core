namespace Fims.Core.Model
{
    public abstract class Locator : Resource
    {
        protected Locator()
            : base(nameof(Locator))
        {
        }
    }
}