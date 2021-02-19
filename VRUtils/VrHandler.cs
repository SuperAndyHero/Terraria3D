using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Num = System.Numerics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using Valve.VR;
using System.Text;
using Terraria;
using Terraria3D;

namespace Terraria3D
{
    public class VrHandler
    {
        public bool HMDConnected = false;
        public bool VrInitalized = false;

        public CVRSystem cvrsystem;
        public CVRCompositor compositor;

        public uint maxDevicesCount;

        public HMD Hmd;
        public List<VrController> controllers;
        public List<VrTracker> trackers;


        private GraphicsDevice GraphicsDevice => Main.graphics.GraphicsDevice;

        public uint viewWidth = 0;
        public uint viewHeight = 0;
        public Vector2 ViewSize => new Vector2(viewWidth, viewHeight);

        public TrackedDevicePose_t[] currentPoses;
        public TrackedDevicePose_t[] nextPoses;

        #region matrixes and renderTargets
        public RenderTarget2D leftEyeTarget;
        public RenderTarget2D rightEyeTarget;

        public Texture_t leftEyeTexture;
        public Texture_t rightEyeTexture;

        public Matrix leftEyeProjection;//left eye fov
        public Matrix rightEyeProjection;//right eye fov

        public Matrix leftEyeView;//left eye transform
        public Matrix rightEyeView;//right eye transform
        #endregion

        public void ActivateVrMode()
        {
            Terraria3D.VrEnabled = true;
            if (!VrInitalized)
                InitVr();
        }

        public void DisableVrMode()
        {
            Terraria3D.VrEnabled = false;
        }


        public void Initalize()//called only once
        {
            if (Terraria3D.VrEnabled)//doesn't initalize this if the game starts with vr mode disabled
            {
                InitVr();//sets a bool to true if it succeeds, if it doesn't it can be called again
            }
        }

        public void InitVr()
        {
            EVRInitError initError = EVRInitError.None;
            cvrsystem = OpenVR.Init(ref initError);

            //vrGui.errorText = initError.ToString();

            if (initError != EVRInitError.None)
            {
                Main.NewText(initError.ToString());
                OpenVR.Shutdown();
                return;
            }

            HMDConnected = true;





            Hmd = new HMD();
            controllers = new List<VrController>();
            trackers = new List<VrTracker>();

            maxDevicesCount = OpenVR.k_unMaxTrackedDeviceCount;

            //gets the poses for every possible device
            currentPoses = new TrackedDevicePose_t[maxDevicesCount];
            nextPoses = new TrackedDevicePose_t[maxDevicesCount];

            for (uint device = 0; device < maxDevicesCount; device++)//discover each connected device
            {
                ETrackedDeviceClass deviceClass = cvrsystem.GetTrackedDeviceClass(device);//get device type

                switch (deviceClass)//if type is HMD or Controller cache the index
                {
                    case ETrackedDeviceClass.HMD:
                        Hmd.DeviceIndex = device;
                        break;
                    case ETrackedDeviceClass.TrackingReference:
                        AddTracker(device);
                        break;
                    case ETrackedDeviceClass.Controller:
                        AddController(device);
                        break;
                }
            }

            InitVisual();//init the visual objects like rendertargets, 'Texture_t's, etc. And sets the compositor

            VrInitalized = true;
        }

        private void InitVisual()
        {
            cvrsystem.GetRecommendedRenderTargetSize(ref viewWidth, ref viewHeight);

            leftEyeTarget = new RenderTarget2D(GraphicsDevice, (int)viewWidth, (int)viewHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            rightEyeTarget = new RenderTarget2D(GraphicsDevice, (int)viewWidth, (int)viewHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            compositor = OpenVR.Compositor;
            compositor.CompositorBringToFront();

            //set view/projection matrixes
            UpdatePoses();
            UpdateMatrix();


            FieldInfo fieldInfo = typeof(Texture2D).GetField("glTexture", BindingFlags.Instance | BindingFlags.NonPublic);

            //left eye
            leftEyeTexture = new Texture_t()
            {
                handle = new IntPtr((int)fieldInfo.GetValue(leftEyeTarget)),
                eType = ETextureType.OpenGL,
                eColorSpace = EColorSpace.Auto
            };

            //right eye
            rightEyeTexture = new Texture_t()
            {
                handle = new IntPtr((int)fieldInfo.GetValue(rightEyeTarget)),
                eType = ETextureType.OpenGL,
                eColorSpace = EColorSpace.Auto
            };

            compositor.FadeGrid(5.0f, false);//fade from the steamVR void to the gameview
        }

        #region device management
        private void AddController(uint device) =>
            controllers.Add(new VrController(device));
        private void RemoveController(uint device) => 
            controllers.RemoveAll(c => c.DeviceIndex == device);

        private void AddTracker(uint device) =>
            trackers.Add(new VrTracker(device));
        private void RemoveTracker(uint device) =>
            trackers.RemoveAll(c => c.DeviceIndex == device);
        #endregion

        public void Draw()
        {
            if (Terraria3D.VrRendering)
            {
                PollEvents();
                UpdatePoses();
                UpdateMatrix();

                //GameMain.Instance.DrawGeometry(leftEyeProjection, leftEyeViewFinal, translate, leftEyeTarget);
                //GameMain.Instance.DrawGeometry(rightEyeProjection, rightEyeViewFinal, translate, rightEyeTarget);

                GraphicsDevice.SetRenderTarget(null);

                SendFrameToHMD();
            }
        }

        private unsafe void PollEvents()
        {
            VREvent_t vrEvent = new VREvent_t();
            int eventSize = sizeof(VREvent_t);//this line needs unsafe
            while (cvrsystem.PollNextEvent(ref vrEvent, (uint)eventSize))
            {
                switch ((EVREventType)vrEvent.eventType)
                {
                    case EVREventType.VREvent_TrackedDeviceActivated:
                        uint addedDevice = vrEvent.trackedDeviceIndex;
                        ETrackedDeviceClass addedDeviceClass = cvrsystem.GetTrackedDeviceClass(addedDevice);
                        switch (addedDeviceClass)
                        {
                            case ETrackedDeviceClass.HMD:
                                Hmd.DeviceIndex = addedDevice;
                                HMDConnected = true;
                                InitVr();
                                System.Diagnostics.Debug.WriteLine("Heatset Connected");
                                break;
                            case ETrackedDeviceClass.TrackingReference:
                                AddTracker(addedDevice);
                                break;
                            case ETrackedDeviceClass.Controller:
                                AddController(addedDevice);
                                break;
                        }
                        break;
                    case EVREventType.VREvent_TrackedDeviceDeactivated:
                        uint removedDevice = vrEvent.trackedDeviceIndex;
                        ETrackedDeviceClass removedDeviceClass = cvrsystem.GetTrackedDeviceClass(removedDevice);
                        switch (removedDeviceClass)
                        {
                            case ETrackedDeviceClass.HMD:
                                HMDConnected = false;
                                break;
                            case ETrackedDeviceClass.Controller:
                                RemoveController(removedDevice);
                                break;
                            case ETrackedDeviceClass.TrackingReference:
                                RemoveTracker(removedDevice);
                                break;
                        }
                        break;
                    //default:
                    //    System.Diagnostics.Debug.WriteLine((EVREventType)vrEvent.eventType);
                    //    break;
                }
            }
        }
        private void UpdatePoses()
        {
            compositor.WaitGetPoses(currentPoses, nextPoses);//gets the poses for every possible device
        }
        private void UpdateMatrix()
        {
            if (currentPoses[Hmd.DeviceIndex].bPoseIsValid)
                Hmd.DeviceMatrix = currentPoses[Hmd.DeviceIndex].mDeviceToAbsoluteTracking.ToMatrix();

            foreach (VrController controller in controllers)
            {
                if (currentPoses[controller.DeviceIndex].bPoseIsValid)
                    controller.DeviceMatrix = currentPoses[controller.DeviceIndex].mDeviceToAbsoluteTracking.ToMatrix();
            }

            foreach (VrTracker tracker in trackers)
            {
                if (currentPoses[tracker.DeviceIndex].bPoseIsValid)
                    tracker.DeviceMatrix = currentPoses[tracker.DeviceIndex].mDeviceToAbsoluteTracking.ToMatrix();
            }

            //TODO move to just init
            leftEyeProjection = cvrsystem.GetProjectionMatrix(EVREye.Eye_Left, 0.01f, 1000.0f).ToMatrix();
            rightEyeProjection = cvrsystem.GetProjectionMatrix(EVREye.Eye_Right, 0.01f, 1000.0f).ToMatrix();

            leftEyeView = cvrsystem.GetEyeToHeadTransform(EVREye.Eye_Left).ToMatrix();
            rightEyeView = cvrsystem.GetEyeToHeadTransform(EVREye.Eye_Right).ToMatrix();
        }

        private VRTextureBounds_t texBounds = new VRTextureBounds_t
        {
            uMin = 0,
            uMax = 1,
            vMin = 0,
            vMax = 1
        };

        private void SendFrameToHMD() 
        {
            //damn distortion effect is ALWAYS there
            //after several hours of debugging and adding a distortion shader, I conclude that its NOT a issue with this (after opening steamvr home for once)
            OpenVR.Compositor.Submit(EVREye.Eye_Left, ref leftEyeTexture, ref texBounds, EVRSubmitFlags.Submit_Default);

            OpenVR.Compositor.Submit(EVREye.Eye_Right, ref rightEyeTexture, ref texBounds, EVRSubmitFlags.Submit_Default);
        }
    }
}