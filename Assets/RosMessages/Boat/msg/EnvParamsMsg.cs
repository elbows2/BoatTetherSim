//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Boat
{
    [Serializable]
    public class EnvParamsMsg : Message
    {
        public const string k_RosMessageName = "boat_msgs/EnvParams";
        public override string RosMessageName => k_RosMessageName;

        public int n_tether_segments;
        public float boat_drag;
        public float boat_ang_drag;
        public float sub_drag;
        public float sub_ang_drag;
        public float end_mass;
        public float boat_mass;
        public float sim_speed;

        public EnvParamsMsg()
        {
            this.n_tether_segments = 0;
            this.boat_drag = 0.0f;
            this.boat_ang_drag = 0.0f;
            this.sub_drag = 0.0f;
            this.sub_ang_drag = 0.0f;
            this.end_mass = 0.0f;
            this.boat_mass = 0.0f;
            this.sim_speed = 0.0f;
        }

        public EnvParamsMsg(int n_tether_segments, float boat_drag, float boat_ang_drag, float sub_drag, float sub_ang_drag, float end_mass, float boat_mass, float sim_speed)
        {
            this.n_tether_segments = n_tether_segments;
            this.boat_drag = boat_drag;
            this.boat_ang_drag = boat_ang_drag;
            this.sub_drag = sub_drag;
            this.sub_ang_drag = sub_ang_drag;
            this.end_mass = end_mass;
            this.boat_mass = boat_mass;
            this.sim_speed = sim_speed;
        }

        public static EnvParamsMsg Deserialize(MessageDeserializer deserializer) => new EnvParamsMsg(deserializer);

        private EnvParamsMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.n_tether_segments);
            deserializer.Read(out this.boat_drag);
            deserializer.Read(out this.boat_ang_drag);
            deserializer.Read(out this.sub_drag);
            deserializer.Read(out this.sub_ang_drag);
            deserializer.Read(out this.end_mass);
            deserializer.Read(out this.boat_mass);
            deserializer.Read(out this.sim_speed);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.n_tether_segments);
            serializer.Write(this.boat_drag);
            serializer.Write(this.boat_ang_drag);
            serializer.Write(this.sub_drag);
            serializer.Write(this.sub_ang_drag);
            serializer.Write(this.end_mass);
            serializer.Write(this.boat_mass);
            serializer.Write(this.sim_speed);
        }

        public override string ToString()
        {
            return "EnvParamsMsg: " +
            "\nn_tether_segments: " + n_tether_segments.ToString() +
            "\nboat_drag: " + boat_drag.ToString() +
            "\nboat_ang_drag: " + boat_ang_drag.ToString() +
            "\nsub_drag: " + sub_drag.ToString() +
            "\nsub_ang_drag: " + sub_ang_drag.ToString() +
            "\nend_mass: " + end_mass.ToString() +
            "\nboat_mass: " + boat_mass.ToString() +
            "\nsim_speed: " + sim_speed.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}