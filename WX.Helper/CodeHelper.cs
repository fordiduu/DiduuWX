using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace WX.Helper
{
    public class CodeHelper
    {

        public static string RandomCode(int VcodeNum = 4)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p" +
             ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P" +
             ",Q,R,S,T,U,V,W,X,Y,Z,";
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            int temp = -1;
            Random rand = new Random();

            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(35);
                if (temp != -1 && temp == t)
                {
                    return RandomCode(VcodeNum);
                }
                temp = t;
                VNum += VcArray[t] + " ";
            }
            CookieHelper.SetCookie("CodeType", "Char");
            CookieHelper.SetCookie("CodeValue", VNum);
            return VNum;
        }

        public static string RandomNum()
        {
            Random ran = new Random();
            int num1 = ran.Next(10, 99);
            int num2 = ran.Next(1, 50);
            string VNum = string.Empty;
            string type = ran.Next(0, 10).ToString();
            switch (type)
            {
                case "1":
                case "5":
                case "7":
                case "8":
                default:
                    VNum = num1 + " + " + num2;
                    break;
                case "2":
                case "4":
                case "9":
                case "3":
                case "6":
                    VNum = num1 + " - " + num2;
                    break;
                case "0":
                    VNum = num1 + " x " + num2;
                    break;
            }
            CookieHelper.SetCookie("CodeType", "Num");
            CookieHelper.SetCookie("CodeValue", VNum);
            return VNum;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateNum">验证码</param>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap(120, 50);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 50; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("youyuan", 20, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                ///画什么、字体、画刷、纵坐标、横坐标
                g.DrawString(validateCode, font, brush, 5, 10);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

    }
}
