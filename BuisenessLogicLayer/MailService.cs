using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisenessLogicLayer
{
    public class MailService
    {
        public List<MailSetting> GetSettigsList()
        {
            List<MailSetting> result = null;
            using (var context = new MySmtpEntities())
            {
                try
                {
                    result = context.MailSettings.OrderBy(x => x.Email).ToList();
                   
                }
                catch (Exception ex)
                {
                    ServerHelper.WriteError(ex);
                    return result;
                }
            }

            return result;
        }

    }
}
