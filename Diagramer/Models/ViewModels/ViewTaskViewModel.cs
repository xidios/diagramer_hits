using Microsoft.AspNet.SignalR.Infrastructure;

namespace Diagramer.Models.ViewModels;

public class ViewTaskViewModel
{
    public Guid? GroupId { get; set; } = null;
    public Models.Task Task { get; set; }
    public Answer? UserAnswer { get; set; }
}