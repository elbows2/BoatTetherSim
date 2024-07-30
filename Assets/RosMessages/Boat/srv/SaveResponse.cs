//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Boat
{
    [Serializable]
    public class SaveResponse : Message
    {
        public const string k_RosMessageName = "boat_msgs/Save";
        public override string RosMessageName => k_RosMessageName;


        public SaveResponse()
        {
        }
        public static SaveResponse Deserialize(MessageDeserializer deserializer) => new SaveResponse(deserializer);

        private SaveResponse(MessageDeserializer deserializer)
        {
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
        }

        public override string ToString()
        {
            return "SaveResponse: ";
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Response);
        }
    }
}
