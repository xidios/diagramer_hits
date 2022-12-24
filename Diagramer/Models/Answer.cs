namespace Diagramer.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public String Diagramm { get; set; } = null;
    
}