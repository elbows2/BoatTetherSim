import dynamics
import buffer
import rclpy
from sensor_msgs.msg import Imu
from geometry_msgs.msg import Vector3, PoseStamped


    
class ControlPolicy(rclpy.Node):

    def __init__(self):
        self.model = dynamics.DynamicsModel()
        self.data = buffer.ReplayBuffer()