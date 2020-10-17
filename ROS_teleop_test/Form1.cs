using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using RosSharp.RosBridgeClient;
using geometry = RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace ROS_teleop_test
{
    public partial class Form1 : Form
    {
        public RosSocket rosSocket;
        public string uri;

        public static double robot_x;
        public static double robot_y;
        public static double robot_theta;

        public static double vel_x;
        public static double vel_y;
        public static double vel_th;

        public static double vel_x_pub;
        public static double vel_y_pub;
        public static double vel_theta_pub;

        public string zm_robot_pose;
        public string zm_robot_vel;
        public string zm_robot_vel_pub;

        private geometry.Twist vel_pub;

        private Thread thr_subscriber;
        private Thread thr_publisher;

        public Form1()
        {
            InitializeComponent();
        }

        public void LoopExecute_subscriber()
        {
            while (true)
            {
                Thread.Sleep(100);
                this.Invoke((Action)Subscribe_Position);
            }
        }

        public void LoopExecute_publisher()
        {
            while (true)
            {
                Thread.Sleep(100);
                this.Invoke((Action)Publisher_CmdVel);
            }
        }

        public void Subscribe_Position()
        {
            label10.Text = robot_x.ToString("F3");
            label11.Text = robot_y.ToString("F3");
            label12.Text = robot_theta.ToString("F3");
            
            label13.Text = vel_x.ToString("F3");
            label14.Text = vel_y.ToString("F3");
            label15.Text = vel_th.ToString("F3");
        }

        public void Publisher_CmdVel()
        {
            rosSocket.Publish(zm_robot_vel_pub, vel_pub);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uri = "ws://" + textBox1.Text + ":" + textBox2.Text;
            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(textBox1.Text, Convert.ToInt32(textBox2.Text));

                rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(uri));
                zm_robot_pose = rosSocket.Subscribe<geometry.Pose2D>("/zm_robot_position", SubscriptionPositionHandler);
                zm_robot_vel = rosSocket.Subscribe<geometry.Twist>("/cmd_vel", SubscriptionCmdVelHandler);
                zm_robot_vel_pub = rosSocket.Advertise<geometry.Twist>("cmd_vel");
                thr_subscriber = new Thread(LoopExecute_subscriber);
                thr_publisher = new Thread(LoopExecute_publisher);
                thr_subscriber.Start();
                thr_publisher.Start();
                label20.Text = "Success.";
                button14.Enabled = true;
            }
            catch (SocketException ex)
            {
                label20.Text = "Failed.";
                MessageBox.Show("Connnect failed, please check IP Address.");
            }
            catch (System.FormatException error)
            {
                label20.Text = "Failed.";
                MessageBox.Show("Input IP incorrect, please input correct IP Address.");
            }
        }

        private static void SubscriptionPositionHandler(geometry.Pose2D message)
        {
            robot_x = message.x;
            robot_y = message.y;
            robot_theta = message.theta;
        }

        private static void SubscriptionCmdVelHandler(geometry.Twist message)
        {
            geometry.Vector3 vel_linear = message.linear;
            geometry.Vector3 vel_angular = message.angular;
            vel_x = vel_linear.x;
            vel_y = vel_linear.y;
            vel_th = vel_angular.z;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            rosSocket.Unsubscribe(zm_robot_pose);
            rosSocket.Unsubscribe(zm_robot_vel);
            rosSocket.Unadvertise(zm_robot_vel_pub);

            thr_subscriber.Abort();

            rosSocket.Close();
            label20.Text = "Disconection success.";
            button1.Enabled = true;
            button13.Enabled = false;
            button14.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 0;
            vel_y_pub = 0.05 * trackBar1.Value * 0;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
               linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0) ,
               angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (vel_x_pub > 0)
            {
                vel_x_pub = 0.05 * trackBar1.Value;
            }
            else if (vel_x_pub < 0)
            {
                vel_x_pub = -0.05 * trackBar1.Value;
            }
            else 
            {
                vel_x_pub = 0;
            }

            if (vel_y_pub > 0)
            {
                vel_y_pub = 0.05 * trackBar1.Value;
            }
            else if (vel_y_pub < 0)
            {
                vel_y_pub = -0.05 * trackBar1.Value;
            }
            else
            {
                vel_y_pub = 0;
            }

            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (vel_theta_pub > 0)
            {
                vel_theta_pub = 0.182 * trackBar2.Value;
            }
            else if (vel_theta_pub < 0)
            {
                vel_theta_pub = -0.182 * trackBar2.Value;
            }
            else
            {
                vel_theta_pub = 0;
            }

            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 1;
            vel_y_pub = 0.05 * trackBar1.Value * 0;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button4_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * -1;
            vel_y_pub = 0.05 * trackBar1.Value * 0;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button5_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 0;
            vel_y_pub = 0.05 * trackBar1.Value * 1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button6_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 0;
            vel_y_pub = 0.05 * trackBar1.Value * -1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button7_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 1;
            vel_y_pub = 0.05 * trackBar1.Value * 1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button8_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 1;
            vel_y_pub = 0.05 * trackBar1.Value * -1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button9_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * -1;
            vel_y_pub = 0.05 * trackBar1.Value * 1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button10_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * -1;
            vel_y_pub = 0.05 * trackBar1.Value * -1;
            vel_theta_pub = 0.182 * trackBar2.Value * 0;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button11_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 0;
            vel_y_pub = 0.05 * trackBar1.Value * 0;
            vel_theta_pub = 0.182 * trackBar2.Value * 1;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button12_Click(object sender, EventArgs e)
        {
            vel_x_pub = 0.05 * trackBar1.Value * 0;
            vel_y_pub = 0.05 * trackBar1.Value * 0;
            vel_theta_pub = 0.182 * trackBar2.Value * -1;
            vel_pub = new geometry.Twist
            {
                linear = new geometry.Vector3(vel_x_pub, vel_y_pub, 0),
                angular = new geometry.Vector3(0, 0, vel_theta_pub)
            };
        }

        private void button14_Click(object sender, EventArgs e)
        {
            thr_publisher.Abort();
            button1.Enabled = false;
            button13.Enabled = true;
            button14.Enabled = false;
        }
    }
}