using System.ComponentModel;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Handlers.WinForms.UI;

public abstract class BaseForm : Form
{
    private readonly BackgroundWorker _backgroundWorker;

    protected BaseForm(CancellationToken cancellationToken)
    {
        _backgroundWorker = new BackgroundWorker
        {
            WorkerSupportsCancellation = true
        };
        _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
        _backgroundWorker.RunWorkerAsync(cancellationToken);
        this.Closed += OnClosed;
    }

    public SolutionResultType SolutionResultType { get; protected set; } = SolutionResultType.Skipped;

    private async void BackgroundWorkerOnDoWork(object? sender, DoWorkEventArgs e)
    {
        CancellationToken ct = (CancellationToken)e.Argument!;
        while (ct.IsCancellationRequested == false && e.Cancel == false)
        {
            await Task.Delay(10);
        }
        
        if (e.Cancel)
            return;

        SolutionResultType = SolutionResultType.Canceled;
        Invoke(Close);
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (SolutionResultType is not SolutionResultType.Canceled and not SolutionResultType.Solved)
            SolutionResultType = SolutionResultType.Skipped;
        
        this.Invoke(() => _backgroundWorker.CancelAsync());
    }
}