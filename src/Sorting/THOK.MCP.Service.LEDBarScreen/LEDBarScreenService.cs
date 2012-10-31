using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.MCP.Service.LEDBarScreen
{
    class LEDBarScreenService:THOK.MCP.IService
    {
        private LedForm ledForm = null;
        #region IService 成员

        public event StateChangedEventHandler OnStateChanged;
        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public void Initialize(string fileName)
        {
            Config.Configuration config = new THOK.MCP.Service.LEDBarScreen.Config.Configuration(fileName);
            ledForm = new LedForm(config);
            ledForm.Show();
        }

        public void Release()
        {
            if (null == ledForm)
                return;
            ledForm.Close();
            ledForm.Dispose();
            ledForm = null;
        }

        public void Start()
        {
            return;
        }

        public void Stop()
        {
            ledForm.StopRoll();
            return;
        }

        public object Read(string itemName)
        {
            return null;
        }

        public bool Write(string itemName, object state)
        {
            System.Data.DataTable dt = (System.Data.DataTable)state;
            bool bRet = true;
            if (ledForm == null)
            {
                Logger.Debug("Led form 未初始化！");
                bRet = false;
            }
            if(itemName == "Check")
                ledForm.Refresh(dt,true);
            else
                ledForm.Refresh(dt, false);
            return bRet;
        }

        public void Simulate(string itemName, object state)
        {
            return ;
        }

        #endregion

        #region IService 成员


        public void Invoke(string itemName, object state)
        {
            return;
        }

        #endregion
    }
}
