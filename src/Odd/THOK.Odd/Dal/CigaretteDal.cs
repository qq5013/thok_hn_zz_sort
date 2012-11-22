using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Odd.Dao;

namespace THOK.Odd.Dal
{
    public class CigaretteDal
    {
        private CigaretteDao cigaretteDao = null;

        public CigaretteDal()
        {
            cigaretteDao = new CigaretteDao();
        }

        public DataTable GetCigarette()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                return cigaretteDao.Find();
            }
        }

        public void SaveCigarette(DataTable table)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                cigaretteDao.Update(table);
            }
        }

        public void SaveCigarette(string code, string isAbnormity)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                cigaretteDao.Update(code, isAbnormity);
            }
        }
    }
}
