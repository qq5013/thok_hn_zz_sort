using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Dao;
using THOK.Util;

namespace THOK.AS.Dal
{
    public class HandleCigaretteDal
    {
        public DataTable GetLineAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                table = hcDao.FindLineAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }

        public DataTable GetLineAllDetail(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                table = hcDao.FindLineAllDetail((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }

        public int GetLineCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                count = hcDao.FindLineCount(filter);
            }
            return count;
        }
        public int GetLineDetailCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                count = hcDao.FindLineDetailCount(filter);
            }
            return count;
        }

        public DataTable GetRouteAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                table = hcDao.FindRouteAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }

        public int GetRouteCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                HandleCigaretteDao hcDao = new HandleCigaretteDao();
                count = hcDao.FindRouteCount(filter);
            }
            return count;
        }
    }
}
