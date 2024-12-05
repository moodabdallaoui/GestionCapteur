namespace GestionCapteurs.Data.Model
{
public class Capteur
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Unit { get; set; }
    public DateTime CreatedAt { get; set; }
}
}