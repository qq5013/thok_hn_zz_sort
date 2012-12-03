using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using THOK.AS.Sorting.Dao;
using THOK.Util;
using THOK.AS.Sorting.Util;

namespace THOK.AS.Sorting.Process
{
    class SortingOrderProcess : AbstractProcess
    {
        private MessageUtil messageUtil = null;
        private bool isInit = true;
        public override void Initialize(Context context)
        {
            try
            {
                base.Initialize(context);
                messageUtil = new MessageUtil(context.Attributes);
                isInit = true;
            }
            catch (Exception e)
            {
                Logger.Error("SortingOrderProcess ��ʼ��ʧ�ܣ�ԭ��" + e.Message);
            }

        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object o = ObjectUtil.GetObject(stateItem.State);
                if (o != null)
                {
                    string sortNo = o.ToString();
                    if (Convert.ToInt32(sortNo) > 0)
                    {
                        if (isInit)
                        {
                            isInit = false;
                        }
                        else 
                        {
                            //messageUtil.SendToSupply(sortNo);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Error("��ʼ�ּ𶩵���Ϣ����ʧ�ܣ�ԭ��" + e.Message);
            }
        }
    }
}
