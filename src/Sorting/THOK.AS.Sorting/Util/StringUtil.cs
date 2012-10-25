using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace THOK.AS.Sorting.Util
{
    class StringUtil
    {
        /// תȫ�ǵĺ���(SBC case)
        ///
        ///�����ַ���
        ///ȫ���ַ���
        ///
        ///ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
        ///�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
        ///
        public static String ToSBC(String input)
        {
            // ���תȫ�ǣ�
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        // /
        // / ת��ǵĺ���(DBC case)
        // /
        // /�����ַ���
        // /����ַ���
        // /
        // /ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
        // /�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
        // /
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
    }
}
