using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Dao;
using THOK.Util;

namespace THOK.AS.Dal
{
    public class BatchDal
    {
        public void AddBatch(string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                batchDao.InsertEntity(orderDate, batchNo);
            }
        }

        public DataTable GetAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                table = batchDao.FindAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }

        public int GetCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                count = batchDao.FindCount(filter);
            }
            return count;
        }

        public DataTable GetBatch(string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                return batchDao.FindBatch(orderDate, batchNo);
            }
        }

        public DataTable GetBatchNo(string orderDate)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                table = batchDao.FindBatch(orderDate);
            }
            return table;
        }

        public void SaveExecuter(string user, string ip, string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                batchDao.UpdateExecuter(user, ip, orderDate, batchNo);
            }
        }

        public void SaveUploadUser(string user, string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                batchDao.UpdateNoOnePro(orderDate, batchNo, user);
            }
        }

        public object Findbatch()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsExistBatch()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                DataTable dt = batchDao.IsExistBatch();
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["parametervalue"].ToString() == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateBatch()
        {
            using (PersistentManager ssPM = new PersistentManager("OuterConnection"))
            {
                SalesSystemDao ssDao = new SalesSystemDao();
                DataTable dt = ssDao.FindBatch();
                using (PersistentManager pm = new PersistentManager())
                {
                    BatchDao batchDao = new BatchDao();
                    batchDao.BatchInsertBatch(dt);
                }
            }
        }

        public void Save(string orderDate, string sortBatch, string no1Batch, string no1UpLoadState)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                BatchDao batchDao = new BatchDao();
                batchDao.UpdateEntity(orderDate, sortBatch, no1Batch, no1UpLoadState);
            }
        }
    }
}
