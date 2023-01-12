///////////////////////////////////////////////////////////
//  GForceListener.cs
//  Implementation of the Class GForceListener
//  Generated by Enterprise Architect
//  Created on:      02-2月-2021 16:07:33
//  Original author: hebin
///////////////////////////////////////////////////////////

using gf;
using System.Collections.Generic;
using static gf.Device;


namespace GForce
{
    public class GForceListener : HubListener
    {
        private List<GForceDevice> gForceDevices = new List<GForceDevice>();
        private List<UIListener> uiListeners = new List<UIListener>();

        /// 
        /// <param name="GForceListener"></param>
        public GForceListener()
        {

        }

        ~GForceListener()
        {

        }


        public override void onScanFinished()
        {
            GForceLogger.Log("[GForceListener] onScanFinished ");

            lock (this) // TODO: use lock (uiListeners)
            {
                foreach (var item in uiListeners)
                {
                    item.onScanFinished();
                }
            }
        }

        /// 
        /// <param name="state"></param>
        public override void onStateChanged(Hub.HubState state)
        {
            GForceLogger.Log("[GForceListener] onStateChanged ");

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onStateChanged(state);
                }
            }
        }

        /// <summary>
        /// 连接完成的回调
        /// </summary>
        /// <param name="device"></param>
        public override void onDeviceConnected(Device device)
        {
            GForceLogger.Log("[GForceListener] onDeviceConnected ");

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onDeviceConnected(device);
                    for (int i = 0; i < gForceDevices.Count; i++)
                    {
                        if (gForceDevices[i].GetDevice() == null)
                        {
                            if (device.getName().StartsWith("gForceDual"))
                            {
                                device.SetDeviceType(DeviceType.GforceDual);
                            }
                            if (device.getName().StartsWith("gForceReh"))
                            {
                                device.SetDeviceType(DeviceType.GforceReh);
                            }
                            if (device.getName().StartsWith("gForcePro"))
                            {
                                device.SetDeviceType(DeviceType.GforceProPlus);
                            }
                            if (device.getName().StartsWith("gForce200"))
                            {
                                device.SetDeviceType(DeviceType.Gforce200);
                            }
                            if (device.getName().StartsWith("gForceDuo"))
                            {
                                device.SetDeviceType(DeviceType.GforceDuo);
                            }
                            if (device.getName().StartsWith("gForceMotion"))
                            {
                                device.SetDeviceType(DeviceType.GforceMotion);
                            }
                            if (device.getName().StartsWith("ORehabArm"))
                            {
                                device.SetDeviceType(DeviceType.ORehabArm);
                            }
                            if (device.getName().StartsWith("ORehabLeg"))
                            {
                                device.SetDeviceType(DeviceType.ORehabLeg);
                            }
                            if (device.getName().StartsWith("GfroceOct"))
                            {
                                device.SetDeviceType(DeviceType.GfroceOct);
                            }

                            gForceDevices[i].onDeviceConnected(device);
                            break;
                        }

                    }
                }
            }
        }

        /// 
        /// <param name="device"></param>
        public override void onDeviceDiscard(Device device)
        {
            GForceLogger.Log("[GForceListener] onDeviceDiscard ");

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onDeviceDiscard(device);
                }

                foreach (var dev in gForceDevices)
                {
                    dev.onDeviceDiscard(device);
                }
            }
        }

        /// 
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public override void onDeviceDisconnected(Device device, int reason)
        {
            GForceLogger.Log("[GForceListener] onDeviceDisconnected ");

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onDeviceDisconnected(device, reason);
                }

                foreach (var dev in gForceDevices)
                {
                    dev.onDeviceDisconnected(device, reason);
                }
            }
        }

        /// 
        /// <param name="device"></param>
        public override void onDeviceFound(Device device)
        {
            GForceLogger.Log("[GForceListener] onDeviceFound " + device.getAddress());

            if (!device.getName().StartsWith("gForce") &&
                !device.getName().StartsWith("OHand") &&
                !device.getName().StartsWith("ORehab"))
            {
                return;
            }

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onDeviceFound(device);
                }
            }
        }

        /// 
        /// <param name="device"></param>
        /// <param name="status"></param>
        public override void onDeviceStatusChanged(Device device, Device.Status status)
        {
            GForceLogger.Log("[GForceListener] onDeviceStatusChanged ");

            lock (this)
            {
                foreach (var item in uiListeners)
                {
                    item.onDeviceStatusChanged(device, status);
                }

                foreach (var dev in gForceDevices)
                {
                    dev.onDeviceStatusChanged(device, status);
                }
            }
        }

        /// 
        /// <param name="device"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public override void onExtendedDeviceData(Device device, Device.DataType type, byte[] data)
        {
            //GForceLogger.Log("[GForceListener] onExtendedDeviceData ");

            foreach (var dev in gForceDevices)
            {
                if(dev.GetDevice()!=null)
                {
                    if (device.getAddress().Equals(dev.GetDevice().getAddress()))
                    {
                        if (type == DataType.Emgraw)
                        {
                            dev.UpdateEMGData(data);
                        }
                    }
                }
            }
        }

        /// 
        /// <param name="device"></param>
        /// <param name="gesture"></param>
        public override void onGestureData(Device device, uint gesture)
        {
            //GForceLogger.Log("[GForceListener] onGestureData ");

            //Debug.Log("[GForceListener] onGestureData, Device: " + device + " gForceDevices: " + gForceDevices);

            foreach (var dev in gForceDevices)
            {
                if (dev.GetDevice()!=null)
                {
                    if (device.getAddress().Equals(dev.GetDevice().getAddress()))
                    {
                        if (device.getAddress().Equals(dev.GetDevice().getAddress()))
                        {
                            dev.UpdateGesture(gesture);
                        }
                    }
                }
            }
        }

        /// 
        /// <param name="device"></param>
        /// <param name="w"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public override void onOrientationData(Device device, float w, float x, float y, float z)
        {
            //GForceLogger.Log("[GForceListener] onOrientationData ");

            foreach (var dev in gForceDevices)
            {
                if(dev.GetDevice()!=null)
                {
                    if (device.getAddress().Equals(dev.GetDevice().getAddress()))
                    {
                        if (device.getAddress().Equals(dev.GetDevice().getAddress()))
                        {
                            dev.UpdateOrientationData(w, x, y, z);
                        }
                    }
                }
            }
        }

        /// 
        /// <param name="uiListener"></param>
        public void RegisterUIListener(UIListener uiListener)
        {
            GForceLogger.Log("[GForceListener] RegisterUIListener ");

            lock (this)
            {
                uiListeners.Add(uiListener);
            }
        }

        /// 
        /// <param name="uiListener"></param>
        public void UnregisterUIListener(UIListener uiListener)
        {
            GForceLogger.Log("[GForceListener] UnregisterUIListener ");

            lock (this)
            {
                uiListeners.Remove(uiListener);
            }
        }

        /// 
        /// <param name="dev"></param>
        public void RegisterGForceDevice(GForceDevice dev)
        {
            GForceLogger.Log("[GForceListener] RegisterGForceDevice ");

            lock (this)
            {
                gForceDevices.Add(dev);
            }
        }

        /// 
        /// <param name="dev"></param>
        public void UnregisterGForceDevice(GForceDevice dev)
        {
            GForceLogger.Log("[GForceListener] UnregisterGForceDevice ");

            lock (this)
            {
                gForceDevices.Remove(dev);
            }
        }

    }//end GForceListener

}//end namespace GameCtrler