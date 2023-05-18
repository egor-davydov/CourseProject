using Random = UnityEngine.Random;

namespace CodeBase.Services.Randomizer
{
  public class RandomService : IRandomService
  {
    public int Next(int minInclusive, int maxExclusive) =>
      Random.Range(minInclusive, maxExclusive);

    public float Next(float minInclusive, float maxInclusive) => 
      Random.Range(minInclusive, maxInclusive);
  }
}