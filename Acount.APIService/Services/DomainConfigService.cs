using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;
using Acount.APIService.Utility;

namespace Acount.APIService.Services
{
    public class DomainConfigService
    {
        public static MailFormatModelDTO GetBasicMailFormat(PGDBContext context)
        {
            //PGDBContext context = new BusinessDBContext().GetDataContext();

            //Declare and initialize model
            MailFormatModelDTO mailFormatModelDTO = new()
            {
                ReturnCode = ResultCodes.R_UNKNOWN
            };
            try
            {
                //Execute query for get basic mail format
                mailFormatModelDTO.Data = (from dt in context.smtpConfiguration
                                           where dt.IsDeleted == 0
                                           select new MailFormatModel
                                           {
                                               Host = Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, dt.Host, 0),
                                               UserName = Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, dt.Username, 0),
                                               Password = Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, dt.Password, 0),
                                               SenderEmail = Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, dt.Sender, 0),
                                               SenderName = dt.SenderName,
                                               SMTPPort = dt.SmtpPort
                                           }).FirstOrDefault();

                mailFormatModelDTO.Data.SecureSocketOption = MailKit.Security.SecureSocketOptions.Auto;
                mailFormatModelDTO.Description = "Success";
                mailFormatModelDTO.ReturnCode = ResultCodes.R_SUCCESS;
            }
            catch (Exception ex)
            {
                mailFormatModelDTO.ReturnCode = ResultCodes.R_DB_ERROR;
                mailFormatModelDTO.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
                SystemLogger.LogData<DomainConfigService>().LogError(ex, ex.Message);
            }
            finally
            {
                //context?.Dispose();
            }
            return mailFormatModelDTO;
        }
    }

}
