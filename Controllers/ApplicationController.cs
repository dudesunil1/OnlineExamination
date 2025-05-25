using UCDArch.Web.Controller;
using UCDArch.Web.Attributes;

namespace OnlineExamination.Controllers
{
    [Version(MajorVersion = 3)]
    //[ServiceMessage("OnlineExamination", ViewDataKey = "ServiceMessages", MessageServiceAppSettingsKey = "MessageService")]
    public abstract class ApplicationController : SuperController
    {
    }
}