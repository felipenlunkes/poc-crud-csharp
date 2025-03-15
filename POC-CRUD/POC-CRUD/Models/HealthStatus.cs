namespace POC_CRUD.Models;

public class HealthStatus
{
    public Guid Id { get; set; }
    public string name {get; set;}
    public string version {get; set;}
    public string build {get; set;}
    public string releaseDate {get; set;}
    public string status {get; set;}
    public int code {get; set;}

    public HealthStatus(string name, string version, string build, string releaseDate, string status, int code)
    {
        this.Id = Guid.NewGuid();
        this.name = name;
        this.version = version;
        this.build = build;
        this.releaseDate = releaseDate;
        this.status = status;
        this.code = code;
    }
}