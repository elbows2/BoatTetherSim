import rclpy
import numpy as np
from rclpy.node import Node
from tethered_controller_node.accumulator import Accumulator
from tethered_controller_node.buffer import ReplayBuffer
from boat_msgs.srv import Save
class DatasetBuilder(Node):
    def __init__(self):
        super().__init__('dataset')
        self.acc = Accumulator(hist_time=1, node=self)
        self.buffer = ReplayBuffer(state_dim=(280-28),action_dim=30, estimate_dim=31)
        self.timer = self.create_timer(2,self.sample)
        self.srv = self.create_service(Save, 'save', self.save)

    def sample(self):
        s = self.acc.get_state()
        if s is not None:
            # print("add!")
            states = s[0]
            actions = s[1]
            estimates = s[2]
            self.buffer.add(estimates[-2], states[:-1].flatten(), actions[:].flatten(), estimates[-1])


    def save(self, request, response):
        self.buffer.save(request.folder)

        return response

def main(args=None):
    rclpy.init(args=args)
    builder = DatasetBuilder()
    rclpy.spin(builder)


if __name__ == '__main__':
    main()