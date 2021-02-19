using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public abstract class VrDevice
    {
        public uint DeviceIndex;//All steamvr devices are in a single array

        public Matrix DeviceMatrix = Matrix.Identity;
    }



    public class HMD : VrDevice { }

    public class VrController : VrDevice
    {
        public VrController(uint index) => DeviceIndex = index;
    }

    public class VrTracker : VrDevice
    {
        public VrTracker(uint index) => DeviceIndex = index;
    }
}