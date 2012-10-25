using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing;

namespace THOK.AS.Sorting.Util
{
    public class LED2008
    {
        #region DLL 方法定义

        #region 参数说明

        //通讯方式常量
        public const int DEVICE_TYPE_COM = 0;// 串口通讯
        public const int DEVICE_TYPE_NET = 1;// 网络通讯

        //串行通讯速度常量
        public const int SBR_9600 = 9600; 
        public const int SBR_19200 = 19200; 
        public const int SBR_57600 = 57600;
        public const int SBR_115200 = 115200;

        //EQ3002/2008控制卡类型
        public const int EQ3002_I = 0; 
        public const int EQ3002_II = 1; 
        public const int EQ3002_III = 2; 
        public const int EQ2008_I = 3; 
        public const int EQ2008_II = 3;  

        //颜色常量
        public const int RED = 0x0000FF; 
        public const int GREEN = 0x00FF00; 
        public const int YELLOW = 0x00FFFF; 

        //返回值常量
        public const int EQ_FALSE = 0; 
        public const int EQ_TRUE = 1; 

        #endregion        

        #region 节目管理
        /// <summary>
        /// 添加节目
        /// </summary>
        /// <param name="CardNum">控制卡地址</param>
        /// <param name="bWaitToEnd"></param>
        /// <param name="iPlayTime">播放时间</param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern int User_AddProgram(
            int CardNum,
            int bWaitToEnd,
            int iPlayTime
        );
        /// <summary>
        /// 删除节目
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_DelProgram(
            int CardNum,
            int iProgramIndex
        );
        /// <summary>
        /// 删除所有节目
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_DelAllProgram(
            int CardNum
        );

        #endregion
        
        #region 图像处理

        /// <summary>
        /// 添加图文区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pBmp"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern int User_AddBmpZone(
            int CardNum,
            ref User_Bmp pBmp,
            int iProgramIndex
        );
        /// <summary>
        /// 向图文区添加图片
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="iBmpPartNum"></param>
        /// <param name="hBitmap"></param>
        /// <param name="pMoveSet"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_AddBmp(
            int CardNum,
            int iBmpPartNum,
            IntPtr hBitmap,
            ref User_MoveSet pMoveSet,
            int iProgramIndex
        );

        #endregion

        #region 文本处理
        /// <summary>
        /// 添加文本区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pText"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern int User_AddText (
            int CardNum, ref User_Text pText, int iProgramIndex
        );

        /// <summary>
        /// 添加RTF区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pRTF"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern int User_AddRTF (
            int CardNum, ref User_RTF pRTF, int iProgramIndex  
        );

        /// <summary>
        /// 添加单行文本区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pSingleText"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern int User_AddSingleText(
            int CardNum, ref User_SingleText pSingleText, int iProgramIndex 
        );

        #endregion

        #region 开关机
        /// <summary>
        /// 开机
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_OpenScreen(
            int  CardNum
        );
        /// <summary>
        /// 关机
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_CloseScreen(
            int  CardNum
        );

        #endregion       

        #region 发送数据

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool User_SendToScreen(
            int CardNum
        );

        #endregion

        #region 同步连接管理
        /// <summary>
        /// （1）、建立连接
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern  bool User_RealtimeConnect(int CardNum);
        /// <summary>
        /// （2）、发送数据
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="hBitmap"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_RealtimeSendData(int CardNum, int x, int y, int iWidth, int iHeight, int hBitmap);
        /// <summary>
        /// （3、）断开连接
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll")]
        public static extern bool User_RealtimeDisConnect(int CardNum);

        #endregion

        #endregion
    }
}
