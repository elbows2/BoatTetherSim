FROM ros:humble-ros-base 
RUN mkdir -p /home/ros_ws/src

COPY ./src/ /home/ros_ws/src

# Change to workspace on sign in
RUN echo "cd /home/ros_ws" >> ~/.bashrc

# Build the workspace
RUN cd /home/ros_ws && . /opt/ros/humble/setup.sh && colcon build

# Source the workspace on sign in
RUN echo "source /opt/ros/humble/setup.bash" >> ~/.bashrc
RUN echo "source /home/ros_ws/install/local_setup.bash" >> ~/.bashrc

