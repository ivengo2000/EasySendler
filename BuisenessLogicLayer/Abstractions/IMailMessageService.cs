using BuisenessLogicLayer.Models;

namespace BuisenessLogicLayer.Abstractions
{
    public interface IMailMessageService
    {
        string GetMessage<T>(T messageInfo) where T : BaseEmailMessageInfo;

        //SpeakerForm MapSpeakerForm(SpeakerRequestInfo info);

        //SiteFeedbackForm MapSiteFeedbackForm(SiteFeedbackFormInfo info, string email);

        //MediaForm MapMediaForm(MediaContactInfo info);
    }
}