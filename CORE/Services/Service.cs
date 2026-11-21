using CORE.Models;
using System.Globalization;
namespace CORE.Services;
    
public abstract class Service
{
    private string _culture;

    protected string Culture
    {
        get { return _culture; }

        set
        {
            _culture = value;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Culture);
        }
    }

    protected Service()
    {
        Culture = "en-US";
    }
    
    protected CommandResponse Success (int id, string message = default) =>
    new CommandResponse(true, message ?? "Operation Successful", id);
    
    protected CommandResponse Error(int id, string message = default) =>
    new CommandResponse(false, message ?? "Operation Failed");
}