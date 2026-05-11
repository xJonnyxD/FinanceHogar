namespace FinanceHogar.Web.Services;

public class ToastService
{
    public event Action<string, string>? OnShow;
    public void Show(string message, string type = "success") => OnShow?.Invoke(message, type);
    public void Success(string msg) => Show(msg, "success");
    public void Error(string msg) => Show(msg, "danger");
    public void Warning(string msg) => Show(msg, "warning");
}
