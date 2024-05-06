import rclpy
import numpy as np
from threading import Lock
from sensor_msgs.msg import Imu
from geometry_msgs.msg import Vector3, PoseStamped
from boat_msgs.msg import TetherPose, Command

class ActionAccumulator():
    def __init__(self, hist_time, rate, node, topic):
        self.hist_length = (int) (hist_time * rate)
        print(self.hist_length)
        self.node = node
        self.commands = []
        self.length = 0
        self.node.create_subscription(Command, topic, self.command_callback, 1)
        self.lock = Lock()

    def clear(self):
        self.length = 0
        self.commands = []


    def is_ready(self):
        return self.length >= self.hist_length

    def command_callback(self, msg):
        with self.lock:    
            self.commands.append(np.array([
                msg.vel,
                msg.yaw,
                msg.winch
                ]))

            self.length += 1

    def get_data(self):
        return self.commands[-self.hist_length:]

class TetherAccumulator():
    def __init__(self, hist_time, rate, node, topic):
        self.hist_length = (int) (hist_time * rate)
        print(self.hist_length)
        self.node = node
        self.tether_lengths = []
        self.length = 0
        self.node.create_subscription(TetherPose, topic, self.tether_callback, 1)
        self.lock = Lock()

    def clear(self):
        self.length = 0
        self.tether_lengths = []


    def is_ready(self):
        return self.length >= self.hist_length

    def tether_callback(self, msg):
        with self.lock:    
            self.tether_lengths.append(np.array(msg.length))

            self.length += 1

    def get_data(self):
        return self.tether_lengths[-self.hist_length:]

class VectorAccumulator():
    def __init__(self, hist_time, rate, node, topic):
        self.hist_length = (int) (hist_time * rate)
        print(self.hist_length)
        self.node = node
        self.vectors = []
        self.length = 0
        self.node.create_subscription(Vector3, topic, self.vector_callback, 1)
        self.lock = Lock()

    def clear(self):
        self.length = 0
        self.vectors = []

    def is_ready(self):
        return self.length >= self.hist_length
    
    def vector_callback(self, msg):
        with self.lock:  
            self.vectors.append(np.array([
                msg.x,
                msg.y,
                msg.z
            ]))

            self.length += 1

    def get_data(self):
        return self.vectors[-self.hist_length:]

class PoseAccumulator():
    def __init__(self, hist_time, rate, node, topic):
        self.hist_length = (int) (hist_time * rate)
        print(self.hist_length)
        self.node = node
        self.positions = []
        self.depths = []
        self.length = 0
        self.node.create_subscription(PoseStamped, topic, self.pose_callback, 1)
        self.lock = Lock()
    
    def pose_callback(self, msg):
        with self.lock:    
            self.positions.append(np.array([
                msg.pose.position.x,
                msg.pose.position.y,
                msg.pose.position.z
            ]))
            self.depths.append(np.array(msg.pose.position.z))

            self.length += 1

    def clear(self):
        self.length = 0
        self.depths = []
        self.positions = [] 

    def is_ready(self):
        return self.length >= self.hist_length

    def get_positions(self):
        return self.positions[-self.hist_length:]
        
    def get_depths(self):
        return self.depths[-self.hist_length:]

class ImuAccumulator():
    def __init__(self, hist_time, rate, node, topic):
        self.hist_length = (int) (hist_time * rate)
        print(self.hist_length)
        self.node = node
        self.lin_accels = []
        self.ang_vels = [] 
        self.orientations = []
        self.length = 0
        self.node.create_subscription(Imu, topic, self.imu_callback, 1)
        self.lock = Lock()
    
    def imu_callback(self, msg):
        with self.lock:
            self.lin_accels.append(np.array([
                msg.linear_acceleration.x,
                msg.linear_acceleration.y,
                msg.linear_acceleration.z
            ]))

            self.ang_vels.append(np.array([
                msg.angular_velocity.x,
                msg.angular_velocity.y,
                msg.angular_velocity.z
            ]))

            self.orientations.append(np.array([
                msg.orientation.x,
                msg.orientation.y,
                msg.orientation.z,
                msg.orientation.w
            ]))
            self.length += 1

    def is_ready(self):
        return self.length >= self.hist_length

    def clear(self):
        self.length = 0
        self.lin_accels = []
        self.ang_vels = [] 
        self.orientations = []

    def get_lin_accels(self):
        return self.lin_accels[-self.hist_length:]
    def get_ang_vels(self):
        return self.ang_vels[-self.hist_length:]
    def get_orientations(self):  
        return self.orientations[-self.hist_length:]
        

class Accumulator():

    def __init__(self, hist_time, node):
        #Todo Get Sensors and rates from config.
        self.hist_time = hist_time
        self.boat_imu = ImuAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "boat/imu")
        self.boat_pose = PoseAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "boat/pose")
        self.boat_vel = VectorAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "boat/vel")
        self.end_imu = ImuAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "end/imu")
        self.end_pose = PoseAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "end/pose")
        # self.end_vel = VectorAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic = "end/vel", lock=self.lock)
        #Change this to be just a depth sensor later.
        self.tether_length = TetherAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic= "oracle/tether")
        self.actions = ActionAccumulator(hist_time = self.hist_time, rate = 10, node=node, topic="sim/cmd")
    

        self.accumulators = [
            self.boat_imu, self.boat_pose, self. boat_vel, self.end_imu, self.end_pose, self.tether_length, self.actions
            ]

    def clear(self):
        for acc in self.accumulators:
            acc.clear()

    def is_ready(self):
        ready = True
        for acc in self.accumulators:
            ready = ready and acc.is_ready()
        return ready

    def get_state(self):
        #lock to aquire from all acumulators evenly.
        # print(f' lin accels: {self.boat_imu.get_lin_accels()}')
        # # print(f' lin accel size: {self.boat_imu.get_lin_accels().size()}')
        # print(f' ang vels: {self.boat_imu.get_ang_vels()}')
        # # print(f' ang vel size: {self.boat_imu.get_ang_vels().size()}')
        # # print/(/f' oriemtatons: {self.boat_imu.get_orientations()}')
        # print(f' orientations size: {np.array(self.boat_imu.get_orientations()).shape}')
        # print(f' orientations size: {np.array(self.end_imu.get_orientations()).shape}')
        # print(f' depths size: {np.array(self.end_pose.get_depths()).reshape(-1,1).shape}')
        # print(f' lengths size: {np.array(self.tether_length.get_data()).reshape(-1,1).shape}')
        #This is stupid fucking lock everything.
        with self.boat_imu.lock, self.boat_pose.lock, self.boat_vel.lock, self.end_imu.lock, self.end_pose.lock, self.tether_length.lock, self.actions.lock:
            if (not self.is_ready()):
                return 
        
            accumulated_states = np.concatenate((
                self.boat_imu.get_lin_accels(),
                self.boat_imu.get_ang_vels(),
                self.boat_imu.get_orientations(),
                self.boat_pose.get_positions(),
                self.boat_vel.get_data(),
                self.end_imu.get_lin_accels(),
                self.end_imu.get_ang_vels(),
                self.end_imu.get_orientations(),
                np.array(self.end_pose.get_depths()).reshape(-1,1),
                np.array(self.tether_length.get_data()).reshape(-1,1)
            ), axis = 1)
            labelled_full_states = np.concatenate((accumulated_states, self.end_pose.get_positions()), axis = 1)

            # labelled_full_state = labelled_full_states[-1]

            actions = np.array(self.actions.get_data())

            self.clear()
            return [accumulated_states,actions, labelled_full_states]
