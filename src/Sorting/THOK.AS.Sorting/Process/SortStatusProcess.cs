using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.MCP;
using THOK.AS.Sorting.Util;
using THOK.AS.Sorting.Dao;
using THOK.AS.Sorting.View;
using THOK.Util;

namespace THOK.AS.Sorting.Process
{
    public class SortStatusProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object o = ObjectUtil.GetObject(stateItem.State);
                if (o != null)
                {
                    string sortStatusTag = o.ToString();

                    using (PersistentManager pm = new PersistentManager())
                    {
                        SortStatusDao sortStatusDao = new SortStatusDao();
                        sortStatusDao.UpdateSortStatus(sortStatusTag);
                    }
                }
            }
            catch (Exception e)
            {                
                Logger.Error("�ּ��豸����״̬��Ϣ����ʧ�ܣ�ԭ��" + e.Message);
            }
        }
    }
}
