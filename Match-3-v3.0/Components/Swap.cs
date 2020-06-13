using DefaultEcs;

namespace Match_3_v3._0.Components
{
    internal struct Swap
    {
        public Entity First;
        public Entity Second;

        public Swap(Entity first, Entity second)
        {
            First = first;
            Second = second;
        }

        public void Deconstruct(out Entity first, out Entity second)
        {
            first = First;
            second = Second;
        }
    }
}