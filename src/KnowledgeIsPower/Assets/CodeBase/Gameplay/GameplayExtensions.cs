using UnityEngine;

namespace CodeBase.Gameplay
{
  public static class GameplayExtensions
  {
    public static void SmoothLookAt(this Transform from, Transform to, float speed) =>
      from.rotation = SmoothedRotation(from.rotation, PositionToLookAt(from, to), speed);

    private static Vector3 PositionToLookAt(Transform from, Transform to)
    {
      Vector3 positionDelta = to.position - from.position;
      return new Vector3(positionDelta.x, 0, positionDelta.z);
    }

    private static Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook, float speed) =>
      Quaternion.Lerp(rotation, TargetRotation(positionToLook), speed * Time.deltaTime);

    private static Quaternion TargetRotation(Vector3 position) =>
      Quaternion.LookRotation(position);
  }
}