
namespace Iris.Web.IrisMembership
{
    public interface IFormsAuthenticationService
    {
        void SignIn(IrisUser user, bool createPersistentCookie);
        void SignOut();
    }
}