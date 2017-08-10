using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using BuisenessLogicLayer.Abstractions;
using BuisenessLogicLayer.Models;

namespace BuisenessLogicLayer.Services
{
    public class MailMessageService : IMailMessageService
    {
        public string GetMessage<T>(T messageInfo) where T : BaseEmailMessageInfo
        {
            string message = null;
            var mailTo = GetMailTo(messageInfo.Email);
            var urlLink = GetUrlLink(messageInfo.Host);
            //var templateInstance = InstanceCreator<T>();

            //if (!string.IsNullOrWhiteSpace(messageInfo.EmailTemplateId))
            //{
            //    var emailTemplateItem = Context.Database.GetItem(messageInfo.EmailTemplateId);
            //    if (emailTemplateItem == null)
            //    {
            //        Log.Error("MailMessageService.GetMessage: emailTemplateItem is null, switched to GetSimpleMessage", this);
            //        message = templateInstance.GetSimpleMessage(mailTo, urlLink, messageInfo);
            //    }
            //    else
            //    {
            //        var emailTemplate = emailTemplateItem.Fields["Email Template"].Value;
            //        message = templateInstance.GetTemplatedMessage(emailTemplate, mailTo, urlLink, messageInfo);
            //    }
            //}
            //else
            //{
            //    Log.Error("MailMessageService.GetMessage: emailTemplateId is null, switched to GetSimpleMessage", this);
            //    message = templateInstance.GetSimpleMessage(mailTo, urlLink, messageInfo);
            //}

            return message;
        }

        //public SpeakerForm MapSpeakerForm(SpeakerRequestInfo info)
        //{
        //    return new SpeakerForm
        //    {
        //        EmailAddress = info.Email,
        //        FirstName = info.FirstName,
        //        LastName = info.LastName,
        //        Title = info.Title,
        //        Organization = info.Organization,
        //        BusinessPhone = info.BusinessPhone,
        //        EventName = info.EventName,
        //        EventDate = info.EventDate,
        //        EventTopic = info.EventTopic,
        //        EventLocation = info.EventLocation,
        //        RequestedSpeaker = info.RequestedSpeaker,
        //        Comments = info.Comments
        //    };
        //}

        //public SiteFeedbackForm MapSiteFeedbackForm(SiteFeedbackFormInfo info, string email)
        //{
        //    return new SiteFeedbackForm
        //    {
        //        Email = email,
        //        Purpose = info.Purpose,
        //        Message = info.MessageBody
        //    };
        //}

        //public MediaForm MapMediaForm(MediaContactInfo info)
        //{
        //    return new MediaForm
        //    {
        //        Email = info.Email,
        //        Name = info.Name,
        //        Organization = info.Organization,
        //        BusinessPhone = info.BusinessPhone,
        //        Deadline = info.Deadline,
        //        YourMessage = info.YourMessage
        //    };
        //}

        //private IFormMesssage InstanceCreator<T>() where T : BaseEmailMessageInfo
        //{
        //    if (typeof(T) == typeof(SiteFeedbackFormInfo))
        //    {
        //        return new SiteFeedbackFormMessage();
        //    }
        //    else if (typeof(T) == typeof(SpeakerRequestInfo))
        //    {
        //        return new SpeakerRequestFormMessage();
        //    }
        //    else if (typeof(T) == typeof(MediaContactInfo))
        //    {
        //        return new MediaContactFormMessage();
        //    }


        //    return null;
        //}

        private static string GetMailTo(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "anonimous";
            }

            return $"<a href='mailto:{email}'>{email}</a>";
        }

        private static string GetUrlLink(string site)
        {
            if (string.IsNullOrWhiteSpace(site))
            {
                return "unknown";
            }

            return $"<a href='{site}'>{site}</a>";
        }
    }
}
