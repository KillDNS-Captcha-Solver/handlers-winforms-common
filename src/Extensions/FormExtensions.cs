namespace KillDNS.CaptchaSolver.Handlers.WinForms.Extensions;

public static class FormExtensions
{
    private static void ApplicationRunProc(object state)
    {
        Application.Run(state as Form);
    }

    public static void RunInNewThreadAndWait(this Form form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form));
        
        if (form.IsHandleCreated)
            throw new InvalidOperationException("Form is already running.");
        
        Thread thread = new Thread(ApplicationRunProc!);
        
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start(form);

        thread.Join();
    }
}