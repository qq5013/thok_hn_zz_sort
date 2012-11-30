using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Sorting.Dao;
using THOK.Util;

namespace THOK.AS.Sorting.Dal
{
    public class OrderDal
    {
        public DataTable GetOrderMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindMaster();
            }
        }

        public DataTable GetOrderDetail(string sortNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindDetail(sortNo);
            }
        }

        public DataTable GetCigarettes()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindCigarettes();
            }
        }

        public DataTable GetOrderWithCigarette(string cigaretteCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindOrderWithCigarette(cigaretteCode);
            }
        }

        public void ClearPackage(string orderID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                orderDao.ClearPackQuantity(orderID);
            }
        }

        public void SetPackage(string orderID)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                orderDao.UpdatePackQuantity(orderID);
            }
        }

        public DataTable GetPackMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindPackMaster();
            }
        }

        /// <summary>
        ///  �����������ʾDAL���� 
        /// </summary>
        /// <param name="sortNoStart">ǰ����ˮ��</param>
        /// <returns></returns>
        public DataTable GetAllOrderDetailForCacheOrderQuery(int sortNoStart)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindDetailForCacheOrderQuery(sortNoStart);
            }
        }

        //��ȡ������ˮ�ŵĶ���
        public DataTable GetSortNoOrderDetail(int sortNoStart)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindSortNoDetail(sortNoStart);
            }
        }

        public DataTable GetPackMaster(string [] filter)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindPackMaster(filter);
            }
        }
        //�ͻ�������ѯ
        public DataTable GetPackDetail()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindPackDetail();
            }
        }

        public DataTable GetPackDetail(string orderId)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindPackDetail(orderId);
            }
        }

        public void UpdateQuantity(string sortNo, string orderId, string channelName, string cigaretteCode,int quantity)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                pm.BeginTransaction();
                try
                {
                    OrderDao orderDao = new OrderDao();
                    orderDao.UpdateQuantity(sortNo, orderId, channelName, cigaretteCode, quantity);
                    pm.Commit();
                }
                catch (Exception)
                {
                    pm.Rollback();
                }
            }
        }
    }
}
