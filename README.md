# ROS_teleop_test
The HMI remote control zm robot using C# program from Windows 10.

Need to install package and Software.

Software: Visual Studio 2019 C# Windows Form and .Net Framework 4.8

Package:

``` bash
$ sudo apt-get install ros-melodic-rosbridge-server
```

``` bash
$ cd <catkin_workspace>/src
```

``` bash
$ git clone https://github.com/qaz9517532846/zm_robot.git
```


``` bash
$ cd ..
```


``` bash
$ catkin_make
```

------

# zm_robot_teleope HMI.

![image](https://github.com/qaz9517532846/ROS_teleop_test/blob/main/image/zm_robot_teleop.png)

# Run the ROS_teleop_test.

Step 1. open zm_robot.

``` bash
$ roslaunch zm_robot_gazebo zm_robot_world.launch
```

``` bash
$ roslaunch zm_robot_control zm_robot_control_rviz.launch
```

Step 2. create zm_robot_rodsbridge launch file and open it.

zm_robot_rodsbridge.launch in zm_robot_control package and input your IP Address and Port.

``` bash
<node name="rosbridge_websocket" pkg="rosbridge_server" type="rosbridge_websocket" output="screen">
    <param name="address" value="your ip"/>
		<param name="port" value="9090"/>
</node>
```
