/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

namespace RosSharp.RosBridgeClient.MessageTypes.Shape
{
    public class Plane : Message
    {
        public const string RosMessageName = "shape_msgs/Plane";

        //  Representation of a plane, using the plane equation ax + by + cz + d = 0
        //  a := coef[0]
        //  b := coef[1]
        //  c := coef[2]
        //  d := coef[3]
        public double[] coef { get; set; }

        public Plane()
        {
            this.coef = new double[4];
        }

        public Plane(double[] coef)
        {
            this.coef = coef;
        }
    }
}
