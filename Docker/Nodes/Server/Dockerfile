FROM boat_common
EXPOSE 10000
RUN git clone https://github.com/Unity-Technologies/ROS-TCP-Endpoint /home/ros_ws/src/ros_tcp_endpoint -b ROS2v0.7.0


WORKDIR /home/ros_ws
RUN  . /opt/ros/humble/setup.sh && colcon build
COPY run.sh .

# CMD ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0
CMD ["./run.sh"]