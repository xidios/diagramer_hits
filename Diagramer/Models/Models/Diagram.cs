namespace Diagramer.Models;

public class Diagram
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public String XML { get; set; }
}