import rclpy
from rclpy.node import Node
from sensor_msgs.msg import Imu


class Controller(Node):
    def __init__(self):
        super().__init__('tethered_controller')
        self.boat_imu_sub = self.create_subscription(Imu, "/boat/imu", self.boat_imu_callback, 1)

    def boat_imu_callback(self, msg):
        self.get_logger().info(f"boat  angular_vel: {msg.angular_velocity} ")

def main(args=None):
    rclpy.init(args=args)
    controller = Controller()
    rclpy.spin(controller)

if __name__ == '__main__':
    main()
