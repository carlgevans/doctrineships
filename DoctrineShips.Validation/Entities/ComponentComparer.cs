namespace DoctrineShips.Validation.Entities
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public sealed class ComponentComparer : IEqualityComparer<ShipFitComponent>
    {
        public bool Equals(ShipFitComponent x, ShipFitComponent y)
        {
            if (x == null || y == null) return false;

            bool equals = x.ComponentId == y.ComponentId && x.Quantity == y.Quantity;

            return equals;
        }

        public int GetHashCode(ShipFitComponent obj)
        {
            if (obj == null) return int.MinValue;

            int hash = 33;
            hash = hash + obj.ComponentId.GetHashCode();
            hash = hash + obj.Quantity.GetHashCode();

            return hash;
        }
    }
}
