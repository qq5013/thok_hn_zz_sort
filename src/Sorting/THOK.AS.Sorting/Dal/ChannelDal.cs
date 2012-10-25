using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Sorting.Dao;
using THOK.Util;

namespace THOK.AS.Sorting.Dal
{
    public class ChannelDal
    {
        public DataTable GetChannel()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ChannelDao channelDao = new ChannelDao();
                return channelDao.FindChannel();
            }
        }

        public DataTable GetChannel(string sortNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ChannelDao channelDao = new ChannelDao();
                return channelDao.FindChannelQuantity(sortNo);
            }
        }

        public DataTable GetEmptyChannel()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ChannelDao channelDao = new ChannelDao();
                return channelDao.FindEmptyChannel();
            }
        }

        public DataTable GetChannelQuantity()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                string sortNo = orderDao.FindLastSortNo();

                ChannelDao channelDao = new ChannelDao();
                return channelDao.FindChannelQuantity(sortNo);
            }
        }

        public bool ExechangeChannel(string sourceChannel, string targetChannel, out int sourceChannelAddress, out int targetChannelAddress)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                ChannelDao channelDao = new ChannelDao();
                OrderDao orderDao = new OrderDao();
                try
                {
                    pm.BeginTransaction();
                    DataTable channelTable = channelDao.FindChannel(sourceChannel);
                    DataTable targetChannelTable = channelDao.FindChannel(targetChannel);

                    sourceChannelAddress = Convert.ToInt32(channelTable.Rows[0]["CHANNELADDRESS"]);
                    targetChannelAddress = Convert.ToInt32(targetChannelTable.Rows[0]["CHANNELADDRESS"]);

                    channelDao.UpdateChannel(targetChannel, channelTable.Rows[0]["CIGARETTECODE"].ToString(),
                        channelTable.Rows[0]["CIGARETTENAME"].ToString(), 
                        Convert.ToInt32(channelTable.Rows[0]["QUANTITY"]),
                        channelTable.Rows[0]["SORTNO"].ToString());

                    channelDao.UpdateChannel(sourceChannel, "", "", 0, "0");

                    orderDao.UpdateChannel(sourceChannel, targetChannel);

                    pm.Commit();
                    return true;
                }
                catch
                {                    
                    pm.Rollback();
                    sourceChannelAddress = 0;
                    targetChannelAddress = 0;
                    return false;
                }
            }
        }
    }
}
