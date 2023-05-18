namespace CodeBase.Services.Randomizer
{
  public interface IRandomService : IService
  {
    int Next(int minInclusive, int maxExclusive);
    float Next(float minInclusive, float maxValue);
  }
}