using UnityEngine;

namespace CodeBase.Extensions.GameplayExtensions
{
  public static class SmoothLookAtExtension
  {
    public static void SmoothLookAt(this Transform from, Transform to, float speed) =>
      from.SmoothLookAt(to.position, speed);

    public static void SmoothLookAt(this Transform from, Vector3 to, float speed) =>
      from.rotation = SmoothedRotation(from.rotation, PositionToLookAt(from, to), speed);

    private static Vector3 PositionToLookAt(Transform from, Vector3 to)
    {
      Vector3 positionDelta = to - from.position;
      return new Vector3(positionDelta.x, 0, positionDelta.z);
    }

    private static Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook, float speed) =>
      Quaternion.Lerp(rotation, TargetRotation(positionToLook), speed);

    private static Quaternion TargetRotation(Vector3 position) =>
      Quaternion.LookRotation(position);
  }
}