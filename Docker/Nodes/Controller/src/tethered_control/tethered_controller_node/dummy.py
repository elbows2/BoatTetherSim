import rclpy
from rclpy.node import Node
from tethered_controller_node.accumulator import Accumulator
class Dummy(Node):
    def __init__(self):
        super().__init__('test')
        self.acc = Accumulator(hist_time=1, node=self)
        self.timer = self.create_timer(2,self.sample)

    def sample(self):
        
        s = self.acc.get_state()
        if s is not None:
            print(f'x size: {s[0].shape}')
            print(f'y size: {s[1].shape}')



def main(args=None):
    rclpy.init(args=args)
    dummy = Dummy()
    rclpy.spin(dummy)

if __name__ == '__main__':
    main()