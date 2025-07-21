namespace DM.LocalServices.Device.Abstractions
{
    /// <summary>
    /// Face camera service abstraction
    /// </summary>
    public interface IFaceCameraService
    {
        /// <summary>
        /// Capture face image
        /// </summary>
        byte[]? CaptureImage();

        /// <summary>
        /// Initialize camera
        /// </summary>
        bool Initialize();

        /// <summary>
        /// Close camera
        /// </summary>
        void Close();
    }
}