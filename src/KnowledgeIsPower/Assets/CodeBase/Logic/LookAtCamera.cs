using UnityEngine;

namespace CodeBase.Logic
{
  public class LookAtCamera : MonoBehaviour
  {
    private Camera _mainCamera;

    private void Start() => 
      _mainCamera = Camera.main;

    private void Update()
    {
      Quaternion cameraRotation = _mainCamera.transform.rotation;
      transform.LookAt(transform.position + cameraRotation * Vector3.back, cameraRotation * Vector3.up);
    }
  }
}
