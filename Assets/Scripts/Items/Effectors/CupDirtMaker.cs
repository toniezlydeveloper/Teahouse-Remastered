using Items.Implementations;

namespace Items.Effectors
{
    public class CupDirtMaker : AEffector<Cup>
    {
        protected override bool TryEffecting(Cup cup)
        {
            if (cup == null)
                return false;

            if (cup.IsDirty)
                return false;

            cup.TeabagType = TeabagType.None;
            cup.WaterTemperature = 25f;
            cup.HasWater = false;
            cup.IsDirty = true;
            return true;
        }
    }
}