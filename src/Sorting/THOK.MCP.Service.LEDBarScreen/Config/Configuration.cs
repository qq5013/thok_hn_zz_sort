using System;
using System.Xml;

namespace THOK.MCP.Service.LEDBarScreen.Config
{
	/// <summary>
	/// OPC配置文件处理类
	/// </summary>
	public class Configuration
	{
		private XmlDocument doc;

        private System.Collections.Generic.Dictionary<int, System.Drawing.Point> group = null;

        public int rollSpeed = 1;
        public int rollTime = 40;
        public int stopTime = 10000;

        public System.Collections.Generic.Dictionary<int, System.Drawing.Point> Group
        {
            get { return group; }
        }
		public Configuration(string configFile)
		{
			doc = new XmlDocument();
			doc.Load(configFile);
            group = new System.Collections.Generic.Dictionary<int, System.Drawing.Point>();
			Initialize();
		}

		private void Initialize()
		{
			XmlNodeList nodeList = doc.GetElementsByTagName("LEDGroup");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode xn = nodeList[i];
                int groupNo = Convert.ToInt32(xn.Attributes["groupNo"].Value);
                int left = Convert.ToInt32(xn.Attributes["left"].Value);
                int top = Convert.ToInt32(xn.Attributes["top"].Value);
                if(!group.ContainsKey(groupNo))
                    group.Add(groupNo, new System.Drawing.Point(left,top));
            }

            nodeList = doc.GetElementsByTagName("Set");
            rollSpeed = Convert.ToInt32(nodeList[0].Attributes["rollSpeed"].Value);
            rollTime = Convert.ToInt32(nodeList[0].Attributes["rollTime"].Value);
            stopTime = Convert.ToInt32(nodeList[0].Attributes["stopTime"].Value);
		}
    }
}
