#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class AkTransform : global::System.IDisposable {
  private global::System.IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal AkTransform(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static global::System.IntPtr getCPtr(AkTransform obj) {
    return (obj == null) ? global::System.IntPtr.Zero : obj.swigCPtr;
  }

  ~AkTransform() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkTransform(swigCPtr);
        }
        swigCPtr = global::System.IntPtr.Zero;
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public AkVector Position() {
    AkVector ret = new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_Position(swigCPtr), false);
    return ret;
  }

  public AkVector OrientationFront() {
    AkVector ret = new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationFront(swigCPtr), false);
    return ret;
  }

  public AkVector OrientationTop() {
    AkVector ret = new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationTop(swigCPtr), false);
    return ret;
  }

  public void Set(AkVector in_position, AkVector in_orientationFront, AkVector in_orientationTop) { AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_0(swigCPtr, AkVector.getCPtr(in_position), AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop)); }

  public void Set(float in_positionX, float in_positionY, float in_positionZ, float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ) { AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_1(swigCPtr, in_positionX, in_positionY, in_positionZ, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ); }

  public void SetPosition(AkVector in_position) { AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_0(swigCPtr, AkVector.getCPtr(in_position)); }

  public void SetPosition(float in_x, float in_y, float in_z) { AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_1(swigCPtr, in_x, in_y, in_z); }

  public void SetOrientation(AkVector in_orientationFront, AkVector in_orientationTop) { AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_0(swigCPtr, AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop)); }

  public void SetOrientation(float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ) { AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_1(swigCPtr, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ); }

  public AkTransform() : this(AkSoundEnginePINVOKE.CSharp_new_AkTransform(), true) {
  }

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.